using TMS.Domain.Entities;

public class UserType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        // Navigation properties
        public ICollection<User> Users { get; private set; }

        // Constructor
        public UserType(string name, string description)
        {
            Name = name;
            Description = description;
            Users = new List<User>();
        }

        // For EF Core
        private UserType() { }
    }
