﻿namespace NexusConnectCRM.Data.Models.Misc
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public bool? IsContacted { get; set; }
        public bool? IsHelped { get; set; }
        public bool? NeedsContact { get; set; }
        public bool? NeedsHelp { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
