using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    [Table("Users", Schema = "ecd")]
    public class User
    {
        [Key, Column("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }

        [Column("UserFirstName")]
        public string firstName { get; set; }

        [Column("UserLastName")]
        public string lastName { get; set; }
        [Column("UserEmail")]
        public string email { get; set; }
        [Column("UserPassword")]
        public string password { get; set; }

        public User() { }

        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public User(string firstName, string lastName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
        }

        public User(int userId, string firstName, string lastName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
        }
    }
}
