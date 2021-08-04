
using System;
using System.ComponentModel.DataAnnotations;

namespace RicModel.RoomRent.Audits
{
    public class AuditAccount : IAudit
    {
        public int AuditAccountId { get; set; }

        public int Id { get; set; }
        
        public string Name { get; set; }

        public string TimeZone { get; set; }

        public bool IsActive { get; set; }

        public string Street { get; set; }

        public string SubUrb { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

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

        public DateTime AuditDateTime { get; set; }

        public string Username { get; set; }

        public string AuditAction { get; set; }

        public bool IsSelected { get; set; }

        public virtual Account Account { get; set; }

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
