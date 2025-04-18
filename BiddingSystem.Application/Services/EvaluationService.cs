using BiddingSystem.Application.DTOs.BidDtos;
using BiddingSystem.Application.DTOs.TenderDtos;
using BiddingSystem.Application.Interfaces;
using BiddingSystem.Application.Repositories.Interfaces;
using BiddingSystem.Entities;
using BiddingSystem.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ITenderRepository _tenderRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IEvaluationCriteriaRepository _criteriaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EvaluationService(
            ITenderRepository tenderRepository,
            IBidRepository bidRepository,
            IEvaluationCriteriaRepository criteriaRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _tenderRepository = tenderRepository;
            _bidRepository = bidRepository;
            _criteriaRepository = criteriaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddEvaluationCriteriaAsync(AddCriteriaDto dto, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(dto.TenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Status != TenderStatus.Draft)
            {
                throw new InvalidOperationException("Criteria can only be added to draft tenders");
            }

            var criteria = new EvaluationCriteria(
                dto.TenderId,
                dto.Name,
                dto.Description,
                dto.Weight);

            await _criteriaRepository.AddAsync(criteria);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> EvaluateBidAsync(EvaluateBidDto dto, int evaluatorId)
        {
            var bid = await _bidRepository.GetByIdWithEvaluationsAsync(dto.BidId);
            if (bid == null)
            {
                return false;
            }

            var tender = await _tenderRepository.GetByIdAsync(bid.TenderId);
            if (tender?.Status != TenderStatus.UnderEvaluation)
            {
                throw new InvalidOperationException("Tender is not in evaluation phase");
            }

            var existingEvaluation = bid.Evaluations.FirstOrDefault(e => e.EvaluatorId == evaluatorId);
            if (existingEvaluation != null)
            {
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
            var tender = await _tenderRepository.GetByIdWithBidsAsync(dto.TenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Status != TenderStatus.UnderEvaluation)
            {
                throw new InvalidOperationException("Tender must be in evaluation phase to be awarded");
            }

            var bidExists = tender.Bids.Any(b => b.Id == dto.WinningBidId);
            if (!bidExists)
            {
                throw new InvalidOperationException("Winning bid must belong to this tender");
            }

            tender.Award(dto.WinningBidId, userId);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<EvaluationSummaryDto> GetEvaluationSummaryAsync(int tenderId)
        {
            var tender = await _tenderRepository.GetByIdWithEvaluationsAsync(tenderId);
            if (tender == null)
            {
                throw new InvalidOperationException("Tender not found");
            }

            var summary = new EvaluationSummaryDto
            {
                TenderId = tender.Id,
                TenderTitle = tender.Title,
                Bids = new List<BidEvaluationSummaryDto>()
            };

            foreach (var bid in tender.Bids)
            {
                var bidSummary = new BidEvaluationSummaryDto
                {
                    BidId = bid.Id,
                    BidderName = $"{bid.Bidder.FirstName} {bid.Bidder.LastName}",
                    TotalAmount = bid.TotalAmount,
                    AverageScore = bid.Evaluations.Any() ?
                        bid.Evaluations.Average(e => e.TotalScore) : 0,
                    EvaluationCount = bid.Evaluations.Count
                };

                summary.Bids.Add(bidSummary);
            }

            return summary;
        }
    }
}