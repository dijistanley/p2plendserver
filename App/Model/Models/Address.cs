using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Address
    {
        public Address()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public AddressUse Use { get; set; }
        public AddressType Type { get; set; }
        // text representation of this address
        public string Text { get; set; }
        public string Line { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Period { get; set; }

        public override bool Equals(object obj)
        {
            Address other = obj as Address;
            if (other == null)
                return false;

            return Country == other.Country &&
                PostalCode == other.PostalCode &&
                State == other.State &&
                City == other.City &&
                District == other.District &&
                Line == other.Line;
        }

    }
}
