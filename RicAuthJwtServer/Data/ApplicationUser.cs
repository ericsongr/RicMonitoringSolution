using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using RicAuthJwtServer.Models;
using RicAuthJwtServer.ViewModels;

namespace RicAuthJwtServer.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        { }

        public ApplicationUser(UserEntryViewModel userEntry)
        {
            UserName = userEntry.UserName;
            FirstName = userEntry.FirstName;
            LastName = userEntry.LastName;
            Email = userEntry.Email;
            MobileNumber = userEntry.MobileNumber;
            PhoneNumber = userEntry.PhoneNumber;
            IsReceiveDueDateAlertPushNotification = userEntry.IsReceiveDueDateAlertPushNotification;
            IsPaidPushNotification = userEntry.IsPaidPushNotification;
        }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string MobileNumber { get; set; }

        public bool IsReceiveDueDateAlertPushNotification { get; set; }
        public bool IsPaidPushNotification { get; set; }
        public bool IsIncomingDueDatePushNotification { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public virtual ICollection<RegisteredDevice> RegisteredDevices { get; set; }
        
        //for multiple result
        public static Expression<Func<ApplicationUser, UserViewModel>> Projection
        {
            get
            {
                return u => new UserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    PhoneNumber = u.PhoneNumber,
                    IsReceiveDueDateAlertPushNotification = u.IsReceiveDueDateAlertPushNotification,
                    IsPaidPushNotification = u.IsPaidPushNotification,
                    IsIncomingDueDatePushNotification = u.IsIncomingDueDatePushNotification,
                    Role = u.Role,
                };
            }
        }

        
    }
}
