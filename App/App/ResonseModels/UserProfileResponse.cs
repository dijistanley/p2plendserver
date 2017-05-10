using Model.Models;

namespace App.ResonseModels
{
    public class UserProfileResponse
    {

        // ApplicationUser
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        
        // UserProfile
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public AddressResponse Address { get; set; }
    }

    public class AddressResponse
    {
        public AddressResponse(Address address)
        {
            this.Text = address.Text;
            this.Line = address.Line;
            this.City = address.City;
            this.District = address.District;
            this.PostalCode = address.PostalCode;
            this.State = address.State;
            this.Country = address.Country;
            this.Period = address.Period;
        }

        public string Text { get; set; }
        public string Line { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Period { get; set; }
    }
}