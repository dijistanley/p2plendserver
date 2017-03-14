using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace P2PLend.Requests
{
    [DataContract]
    public class Address
    {
        [DataMember(Name = "street_number")]
        public string StreetNumber { get; set; }

        [DataMember(Name = "street_name")]
        public string StreetName { get; set; }

        [DataMember(Name = "street_type")]
        public string StreetType { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "province")]
        public string Province { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
    }
}