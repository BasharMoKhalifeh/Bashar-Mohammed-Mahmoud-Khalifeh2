using BiddingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;

namespace TMS.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(u => u.UserType)
                    .WithMany(ut => ut.Users)
                    .HasForeignKey(u => u.UserTypeId);
            });

            // UserType configurations
            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(ut => ut.Id);
                entity.Property(ut => ut.Name).IsRequired().HasMaxLength(100);
                entity.Property(ut => ut.Description).HasMaxLength(500);
                entity.HasIndex(ut => ut.Name).IsUnique();
            });

            // Tender configurations
            modelBuilder.Entity<Tender>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.ReferenceNumber).IsRequired().HasMaxLength(50);
                entity.HasIndex(t => t.ReferenceNumber).IsUnique();
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).IsRequired();
                entity.Property(t => t.IssuedBy).IsRequired().HasMaxLength(200);
                entity.Property(t => t.BudgetRange).HasMaxLength(100);
                entity.Property(t => t.ContactEmail).IsRequired().HasMaxLength(255);
                entity.Property(t => t.Status).HasDefaultValue(TenderStatus.Draft);
                entity.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");


                entity.HasOne(t => t.TenderType)
                    .WithMany(tt => tt.Tenders)
                    .HasForeignKey(t => t.TenderTypeId);

                entity.HasOne(t => t.TenderCategory)
                    .WithMany(tc => tc.Tenders)
                    .HasForeignKey(t => t.TenderCategoryId);

                entity.HasOne(t => t.CreatedBy)
                    .WithMany()
                    .HasForeignKey(t => t.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);


            });

            // TenderType configurations
            modelBuilder.Entity<TenderType>(entity =>
            {
                entity.HasKey(tt => tt.Id);
                entity.Property(tt => tt.Name).IsRequired().HasMaxLength(100);
                entity.Property(tt => tt.Description).HasMaxLength(500);
                entity.HasIndex(tt => tt.Name).IsUnique();
            });

            // TenderCategory configurations
            modelBuilder.Entity<TenderCategory>(entity =>
            {
                entity.HasKey(tc => tc.Id);
                entity.Property(tc => tc.Name).IsRequired().HasMaxLength(100);
                entity.Property(tc => tc.Description).HasMaxLength(500);
                entity.HasIndex(tc => tc.Name).IsUnique();
            });

            // TenderDocument configurations
            modelBuilder.Entity<TenderDocument>(entity =>
            {
                entity.HasKey(td => td.Id);
                entity.Property(td => td.DocumentType).IsRequired().HasMaxLength(100);
                entity.Property(td => td.FilePath).IsRequired();
                entity.Property(td => td.FileName).IsRequired().HasMaxLength(255);
                entity.Property(td => td.UploadDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(td => td.Tender)
                    .WithMany(t => t.Documents)
                    .HasForeignKey(td => td.TenderId);
            });

            // Bid configurations
            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.TechnicalProposal).IsRequired();
                entity.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Status).HasDefaultValue(BidStatus.Submitted);
                entity.Property(b => b.SubmissionDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(b => b.Tender)
                    .WithMany(t => t.Bids)
                    .HasForeignKey(b => b.TenderId);

                entity.HasOne(b => b.Bidder)
                    .WithMany(u => u.Bids)
                    .HasForeignKey(b => b.BidderId);
            });

            // BidItem configurations
            modelBuilder.Entity<BidItem>(entity =>
            {
                entity.HasKey(bi => bi.Id);
                entity.Property(bi => bi.Description).IsRequired().HasMaxLength(500);
                entity.Property(bi => bi.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(bi => bi.TotalPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(bi => bi.Bid)
                    .WithMany(b => b.Items)
                    .HasForeignKey(bi => bi.BidId);
            });

            // BidDocument configurations
            modelBuilder.Entity<BidDocument>(entity =>
            {
                entity.HasKey(bd => bd.Id);
                entity.Property(bd => bd.DocumentType).IsRequired().HasMaxLength(100);
                entity.Property(bd => bd.FilePath).IsRequired();
                entity.Property(bd => bd.FileName).IsRequired().HasMaxLength(255);
                entity.Property(bd => bd.UploadDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(bd => bd.Bid)
                    .WithMany(b => b.Documents)
                    .HasForeignKey(bd => bd.BidId);
            });

            // EvaluationCriteria configurations
            modelBuilder.Entity<EvaluationCriteria>(entity =>
            {
                entity.HasKey(ec => ec.Id);
                entity.Property(ec => ec.Name).IsRequired().HasMaxLength(200);
                entity.Property(ec => ec.Description).HasMaxLength(1000);
                entity.Property(ec => ec.Weight).HasColumnType("decimal(5,2)");

                entity.HasOne(ec => ec.Tender)
                    .WithMany(t => t.EvaluationCriteria)
                    .HasForeignKey(ec => ec.TenderId);
            });

            // BidEvaluation configurations
            modelBuilder.Entity<BidEvaluation>(entity =>
            {
                entity.HasKey(be => be.Id);
                entity.Property(be => be.TotalScore).HasColumnType("decimal(5,2)");
                entity.Property(be => be.Comments).HasMaxLength(1000);
                entity.Property(be => be.EvaluationDate).HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(be => be.Bid)
                    .WithMany(b => b.Evaluations)
                    .HasForeignKey(be => be.BidId);

                entity.HasOne(be => be.Evaluator)
                    .WithMany(u => u.Evaluations)
                    .HasForeignKey(be => be.EvaluatorId);
            });

            // CriteriaScore configurations
            modelBuilder.Entity<CriteriaScore>(entity =>
            {
                entity.HasKey(cs => cs.Id);
                entity.Property(cs => cs.Score).HasColumnType("decimal(5,2)");
                entity.Property(cs => cs.Comments).HasMaxLength(500);

                entity.HasOne(cs => cs.Evaluation)
                    .WithMany(be => be.CriteriaScores)
                    .HasForeignKey(cs => cs.EvaluationId);

                entity.HasOne(cs => cs.Criteria)
                    .WithMany(ec => ec.Scores)
                    .HasForeignKey(cs => cs.CriteriaId);
            });

            // Award configurations
            modelBuilder.Entity<Award>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AwardDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(a => a.Notes).HasMaxLength(1000);

                entity.HasOne(a => a.Tender)
                    .WithOne(t => t.Award)
                    .HasForeignKey<Award>(a => a.TenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.WinningBid)
                    .WithMany()
                    .HasForeignKey(a => a.WinningBidId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.AwardedBy)
                    .WithMany()
                    .HasForeignKey(a => a.AwardedById)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure enums as strings
            modelBuilder.Entity<Tender>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Bid>()
                .Property(b => b.Status)
                .HasConversion<string>();
        }

    }
}