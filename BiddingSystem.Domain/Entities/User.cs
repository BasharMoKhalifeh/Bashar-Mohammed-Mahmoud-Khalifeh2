using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace TMS.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public int UserTypeId { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        // Navigation properties
        public UserType UserType { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
        public ICollection<BidEvaluation> Evaluations { get; private set; }

        // Constructor
        public User(string firstName, string lastName, string email, string passwordHash, int userTypeId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            UserTypeId = userTypeId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;

            Bids = new List<Bid>();
            Evaluations = new List<BidEvaluation>();
        }

        // For EF Core
        private User() { }

        // Domain methods
        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
        }

        public void UpdateLoginTime()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
        }
    }
}
