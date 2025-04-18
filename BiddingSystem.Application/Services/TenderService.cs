using BiddingSystem.Application.DTOs.TenderDtos;
using BiddingSystem.Application.Interfaces;
using BiddingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Fabric.Query;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Services
{

    public class TenderService : ITenderService
    {
        private readonly ITenderRepository _tenderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;
        private readonly IBidRepository _bidRepository;

        public TenderService(
            ITenderRepository tenderRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IDocumentService documentService,
            IBidRepository bidRepository)
        {
            _tenderRepository = tenderRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _documentService = documentService;
            _bidRepository = bidRepository;
        }

        public async Task<TenderDto> CreateTenderAsync(CreateTenderDto dto, int userId)
        {
            var tender = new Tender(
                dto.Title,
                GenerateReferenceNumber(),
                dto.Description,
                dto.IssuedBy,
                dto.IssueDate,
                dto.ClosingDate,
                dto.TenderTypeId,
                dto.TenderCategoryId,
                dto.BudgetRange,
                dto.ContactEmail,
                userId);

            await _tenderRepository.AddAsync(tender);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<TenderDto> UpdateTenderAsync(UpdateTenderDto dto, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(dto.Id);
            if (tender == null || tender.CreatedById != userId)
            {
                return null;
            }

            if (tender.Status != TenderStatus.Draft)
            {
                throw new InvalidOperationException("Only draft tenders can be modified");
            }

            tender.UpdateDetails(
                dto.Title,
                dto.Description,
                dto.IssuedBy,
                dto.IssueDate,
                dto.ClosingDate,
                dto.TenderTypeId,
                dto.TenderCategoryId,
                dto.BudgetRange,
                dto.ContactEmail);

            await _unitOfWork.CommitAsync();
            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<bool> PublishTenderAsync(int tenderId, int userId)
        {
            var tender = await _tenderRepository.GetByIdWithDocumentsAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Documents.Count == 0)
            {
                throw new InvalidOperationException("Cannot publish tender without documents");
            }

            tender.Publish();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> CloseTenderAsync(int tenderId, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            tender.Close();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> SetToEvaluationAsync(int tenderId, int userId)
        {
            var tender = await _tenderRepository.GetByIdWithBidsAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Bids.Count == 0)
            {
                throw new InvalidOperationException("Cannot evaluate tender without bids");
            }

            tender.SetToEvaluation();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> AwardTenderAsync(int tenderId, int winningBidId, int userId)
        {
            var tender = await _tenderRepository.GetByIdWithBidsAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            var bidExists = await _bidRepository.ExistsForTenderAsync(winningBidId, tenderId);
            if (!bidExists)
            {
                return false;
            }

            tender.Award(winningBidId, userId);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> CancelTenderAsync(int tenderId, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            tender.Cancel();
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> AddDocumentAsync(int tenderId, AddDocumentDto dto, int userId)
        {
            var tender = await _tenderRepository.GetByIdAsync(tenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Status != TenderStatus.Draft)
            {
                throw new InvalidOperationException("Documents can only be added to draft tenders");
            }

            var documentPath = await _documentService.UploadTenderDocumentAsync(dto.File);
            var document = new TenderDocument(
                tenderId,
                dto.DocumentType,
                documentPath,
                dto.File.FileName);

            tender.AddDocument(document);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> RemoveDocumentAsync(int documentId, int userId)
        {
            var document = await _tenderRepository.GetDocumentByIdAsync(documentId);
            if (document == null)
            {
                return false;
            }

            var tender = await _tenderRepository.GetByIdAsync(document.TenderId);
            if (tender == null || tender.CreatedById != userId)
            {
                return false;
            }

            if (tender.Status != TenderStatus.Draft)
            {
                throw new InvalidOperationException("Documents can only be removed from draft tenders");
            }

            _documentService.DeleteDocument(document.FilePath);
            tender.RemoveDocument(document);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<PagedList<TenderDto>> GetTendersAsync(TenderParameters parameters)
        {
            var tenders = await _tenderRepository.GetTendersAsync(parameters);
            return _mapper.Map<PagedList<TenderDto>>(tenders);
        }

        public async Task<TenderDto> GetTenderByIdAsync(int id)
        {
            var tender = await _tenderRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<TenderDto>(tender);
        }

        public async Task<PagedList<TenderDto>> GetUserTendersAsync(int userId, TenderParameters parameters)
        {
            var tenders = await _tenderRepository.GetTendersByUserAsync(userId, parameters);
            return _mapper.Map<PagedList<TenderDto>>(tenders);
        }

        public async Task<PagedList<TenderDto>> GetOpenTendersAsync(TenderParameters parameters)
        {
            parameters.Status = TenderStatus.Published;
            var tenders = await _tenderRepository.GetTendersAsync(parameters);
            return _mapper.Map<PagedList<TenderDto>>(tenders);
        }

        private string GenerateReferenceNumber()
        {
            return $"TMS-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }

}
