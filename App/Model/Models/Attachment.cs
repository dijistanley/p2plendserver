using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model.Models
{
    public class Attachment
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public string DataLocation { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public DateTime DateCreated { get; set; }
    }
}
