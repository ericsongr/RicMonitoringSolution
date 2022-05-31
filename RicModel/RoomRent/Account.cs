
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RicModel.RicXplorer;
using RicModel.RoomRent.Audits;

namespace RicModel.RoomRent
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        [RegularExpression("^[^0]+", ErrorMessage = @"Please select a valid timezone")]
        public string Timezone { get; set; }

        public bool IsActive { get; set; }

        public string Street { get; set; }

        public string SubUrb { get; set; }

        public string State { get; set; }

        [StringLength(100)]
        public string PostalCode { get; set; }

        [StringLength(500)]
        [RegularExpression(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Please enter a valid e-mail address")]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        
        public string WebsiteUrl { get; set; }

        public string FacebookUrl { get; set; }
        
        public string AddressLine1 { get; set; }
        
        public string City { get; set; }

        public string DialingCode { get; set; }
        
        public string BusinessOwnerName { get; set; }

        public string BusinessOwnerPhoneNumber { get; set; }

        public string BusinessOwnerEmail { get; set; }

        public string GeoCoordinates { get; set; }
        
        public int? CompanyFeeFailedPaymentCount { get; set; }
        
        public DateTime? PaymentIssueSuspensionDate { get; set; }

        public bool IsSelected { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<AuditAccount> AuditAccounts { get; set; }
        public virtual ICollection<GuestBookingDetail> GuestBookingDetails { get; set; }

        #region functions

        public string FullAddress
        {
            get
            {
                var address = string.Empty;

                if (!string.IsNullOrWhiteSpace(AddressLine1))
                    address += $"{AddressLine1}, ";

                if (!string.IsNullOrWhiteSpace(Street))
                    address += $"{Street}, ";

                if (!string.IsNullOrWhiteSpace(SubUrb))
                    address += $"{SubUrb}, ";

                if (!string.IsNullOrWhiteSpace(State))
                    address += $"{State}, ";

                if (!string.IsNullOrWhiteSpace(PostalCode))
                    address += $"{PostalCode}";

                return address;
            }
        }

        #endregion
    }
}
