﻿using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Customer
{
    public class CustomerInfo
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }
        public int PhoneNumber { get; set; }
        public string UserId { get; set; }
    }
}
