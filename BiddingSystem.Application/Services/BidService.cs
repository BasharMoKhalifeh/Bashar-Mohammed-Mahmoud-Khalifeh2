using AutoMapper;
using BiddingSystem.Application.DTOs.BidDtos;
using BiddingSystem.Application.Interfaces;
using BiddingSystem.Application.Repositories.Interfaces;
using BiddingSystem.Infrastructure.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly ITenderRepository _tenderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentService _documentService;

        public BidService(
            IBidRepository bidRepository,
            ITenderRepository tenderRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IDocumentService documentService)
        {
            _bidRepository = bidRepository;
            _tenderRepository = tenderRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _documentService = documentService;
        }

        public async Task<BidDto> SubmitBidAsync(CreateBidDto dto, int bidderId)
        {
            var tender = await _tenderRepository.GetByIdAsync(dto.TenderId);
            if (tender == null || !tender.CanAcceptBids())
            {
                throw new InvalidOperationException("Tender not available for bidding");
            }

            var bid = new Bid(
                dto.TenderId,
                bidderId,
                dto.TotalAmount,
                dto.TechnicalProposal);

            foreach (var itemDto in dto.Items)
            {
                bid.AddItem(new BidItem(
                    bid.Id,
                    itemDto.Description,
                    itemDto.Quantity,
                    itemDto.UnitPrice));
            }

            await _bidRepository.AddAsync(bid);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<BidDto>(bid);
        }

        public async Task<bool> AddBidDocumentAsync(int bidId, AddBidDocumentDto dto, int bidderId)
        {
            var bid = await _bidRepository.GetByIdAsync(bidId);
            if (bid == null || bid.BidderId != bidderId)
            {
                return false;
            }

            var documentPath = await _documentService.UploadBidDocumentAsync(dto.File);
            var document = new BidDocument(
                bidId,
                dto.DocumentType,
                documentPath,
                dto.File.FileName);

            bid.AddDocument(document);
            await _unitOfWork.CommitAsync();

            return true;
        }

        // Other methods implemented similarly...
    }
}

