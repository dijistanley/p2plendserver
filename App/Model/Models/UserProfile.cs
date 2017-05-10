using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            Address = new Address();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string UserId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Address Address { get; set; }
    }
}
