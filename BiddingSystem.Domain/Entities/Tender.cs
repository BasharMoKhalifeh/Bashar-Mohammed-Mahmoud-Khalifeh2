using BiddingSystem.Domain.Enums;
using TMS.Domain.Entities;

public class Tender
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string ReferenceNumber { get; private set; }
        public string Description { get; private set; }
        public string IssuedBy { get; private set; }
        public DateTime IssueDate { get; private set; }
        public DateTime ClosingDate { get; private set; }
        public int TenderTypeId { get; private set; }
        public int TenderCategoryId { get; private set; }
        public string BudgetRange { get; private set; }
        public string ContactEmail { get; private set; }
        public TenderStatus Status { get; private set; }
        public int CreatedById { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ModifiedAt { get; private set; }

        // Navigation properties
        public TenderType TenderType { get; private set; }
        public TenderCategory TenderCategory { get; private set; }
        public User CreatedBy { get; private set; }
        public ICollection<TenderDocument> Documents { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
        public ICollection<EvaluationCriteria> EvaluationCriteria { get; private set; }
        public Award Award { get; private set; }

        // Constructor
        public Tender(
            string title,
            string referenceNumber,
            string description,
            string issuedBy,
            DateTime issueDate,
            DateTime closingDate,
            int tenderTypeId,
            int tenderCategoryId,
            string budgetRange,
            string contactEmail,
            int createdById)
        {
            Title = title;
            ReferenceNumber = referenceNumber;
            Description = description;
            IssuedBy = issuedBy;
            IssueDate = issueDate;
            ClosingDate = closingDate;
            TenderTypeId = tenderTypeId;
            TenderCategoryId = tenderCategoryId;
            BudgetRange = budgetRange;
            ContactEmail = contactEmail;
            Status = TenderStatus.Draft;
            CreatedById = createdById;
            CreatedAt = DateTime.UtcNow;

            Documents = new List<TenderDocument>();
            Bids = new List<Bid>();
            EvaluationCriteria = new List<EvaluationCriteria>();
        }

        // For EF Core
        private Tender() { }

        // Domain methods
        public void Publish()
        {
            if (Status != TenderStatus.Draft)
                throw new InvalidOperationException("Only draft tenders can be published");

            Status = TenderStatus.Published;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Close()
        {
            if (Status != TenderStatus.Published)
                throw new InvalidOperationException("Only published tenders can be closed");

            Status = TenderStatus.Closed;
            ModifiedAt = DateTime.UtcNow;
        }

        public void SetToEvaluation()
        {
            if (Status != TenderStatus.Closed)
                throw new InvalidOperationException("Only closed tenders can be set to evaluation");

            Status = TenderStatus.UnderEvaluation;
            ModifiedAt = DateTime.UtcNow;
        }

        public void AwardTender(int winningBidId, int awardedById)
        {
            if (Status != TenderStatus.UnderEvaluation)
                throw new InvalidOperationException("Only tenders under evaluation can be awarded");

            if (!Bids.Any(b => b.Id == winningBidId))
                throw new InvalidOperationException("Winning bid must be associated with this tender");

            Status = TenderStatus.Awarded;
            var Award = new Award(Id, winningBidId, awardedById);
            ModifiedAt = DateTime.UtcNow;
        }

        public bool CanAcceptBids()
        {
            return Status == TenderStatus.Published && ClosingDate > DateTime.UtcNow;
        }

        public void AddDocument(TenderDocument document)
        {
            Documents.Add(document);
        }

        public void AddEvaluationCriteria(EvaluationCriteria criteria)
        {
            EvaluationCriteria.Add(criteria);
        }
    }
