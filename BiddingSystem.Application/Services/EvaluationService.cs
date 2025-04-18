using BiddingSystem.Application.DTOs.BidDtos;
using BiddingSystem.Application.Interfaces;
using BiddingSystem.Domain.Enums;
using BiddingSystem.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ITenderRepository _tenderRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IEvaluationCriteriaRepository _criteriaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EvaluationService(
            ITenderRepository tenderRepository,
            IBidRepository bidRepository,
            IEvaluationCriteriaRepository criteriaRepository,
            IUnitOfWork unitOfWork)
        {
            _tenderRepository = tenderRepository;
            _bidRepository = bidRepository;
            _criteriaRepository = criteriaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> EvaluateBidAsync(EvaluateBidDto dto, int evaluatorId)
        {
            var bid = await _bidRepository.GetByIdWithEvaluationsAsync(dto.BidId);
            if (bid == null) return false;

            var existingEvaluation = bid.Evaluations.FirstOrDefault(e => e.EvaluatorId == evaluatorId);
            if (existingEvaluation != null)
            {
                // Update existing evaluation
                foreach (var scoreDto in dto.Scores)
                {
                    var existingScore = existingEvaluation.CriteriaScores
                        .FirstOrDefault(cs => cs.CriteriaId == scoreDto.CriteriaId);

                    if (existingScore != null)
                    {
                        existingScore.UpdateScore(scoreDto.Score, scoreDto.Comments);
                    }
                    else
                    {
                        existingEvaluation.AddCriteriaScore(new CriteriaScore(
                            existingEvaluation.Id,
                            scoreDto.CriteriaId,
                            scoreDto.Score,
                            scoreDto.Comments));
                    }
                }
            }
            else
            {
                // Create new evaluation
                var evaluation = new BidEvaluation(dto.BidId, evaluatorId, dto.Comments);

                foreach (var scoreDto in dto.Scores)
                {
                    evaluation.AddCriteriaScore(new CriteriaScore(
                        evaluation.Id,
                        scoreDto.CriteriaId,
                        scoreDto.Score,
                        scoreDto.Comments));
                }

                bid.AddEvaluation(evaluation);
            }

            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> AwardTenderAsync(AwardTenderDto dto, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(dto.TenderId);
            if (tender == null || tender.Status != TenderStatus.UnderEvaluation)
            {
                return false;
            }

            tender.Award(dto.WinningBidId, userId);
            await _unitOfWork.CommitAsync();

            return true;
        }

        // Other methods implemented similarly...
    }
}
}
