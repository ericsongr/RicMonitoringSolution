using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RicAuthJwtServer.Application.Interfaces;
using RicAuthJwtServer.Data.Exception;
using RicAuthJwtServer.Data.Persistence.Interfaces;
using RicAuthJwtServer.Models;
using RicAuthJwtServer.Models.Constants;

namespace RicAuthJwtServer.Application
{
    public class RegisteredDeviceService : IRegisteredDeviceService
    {
        private readonly IRegisteredDeviceRepository _registeredDeviceRepository;

        public RegisteredDeviceService(IRegisteredDeviceRepository registeredDeviceRepository)
        {
            _registeredDeviceRepository = registeredDeviceRepository ?? throw new ArgumentNullException(nameof(registeredDeviceRepository));
        }

        public void Delete(string userId)
        {
            _registeredDeviceRepository.DeleteWhere(o => o.AspNetUsersId == userId);
            _registeredDeviceRepository.Commit();
        }

        public List<RegisteredDevice> FindAll(string userId)
        {
            try
            {
                var registeredDevices = _registeredDeviceRepository
                    .FindBy(f => f.AspNetUsersId == userId)
                    .GroupBy(g => new { g.DeviceId, g.Platform})
                    .Select(o => new RegisteredDevice
                    {
                        DeviceId = o.Key.DeviceId,
                        Platform = o.Key.Platform,
                        LastAccessOnUtc = o.Max(o => o.LastAccessOnUtc)
                    }).ToList();

                return registeredDevices;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }

        public RegisteredDevice Find(string userId, string deviceId)
        {
            try
            {
                var registeredDevice = _registeredDeviceRepository                    
                    .GetSingleAsync(f => f.AspNetUsersId == userId && f.DeviceId == deviceId)
                    .GetAwaiter().GetResult();
                return registeredDevice;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }

        public List<RegisteredDevice> FindIncomingDueDatePushNotifications()
        {
            try
            {
                var registeredDevices = _registeredDeviceRepository                    
                    .FindBy(f => f.User.IsIncomingDueDatePushNotification && f.Platform == PlatformConstant.Android)
                    .ToList();
                return registeredDevices;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }
        
        public List<RegisteredDevice> FindReceiveDueDatePushNotifications()
        {
            try
            {
                var registeredDevices = _registeredDeviceRepository                    
                    .FindBy(f => f.User.IsReceiveDueDateAlertPushNotification && f.Platform == PlatformConstant.Android)
                    .ToList();
                return registeredDevices;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }

        public List<RegisteredDevice> FindIsPaidPushNotifications()
        {
            try
            {
                var registeredDevices = _registeredDeviceRepository
                    .FindBy(f => f.User.IsPaidPushNotification && f.Platform == PlatformConstant.Android)
                    .ToList();
                return registeredDevices;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }
        
        public List<RegisteredDevice> FindIsBatchProcessCompletedPushNotification()
        {
            try
            {
                var registeredDevices = _registeredDeviceRepository
                    .FindBy(f => f.User.IsBatchProcessCompletedPushNotification && f.Platform == PlatformConstant.Android)
                    .ToList();
                return registeredDevices;
            }
            catch (DataException de)
            {
                throw new RepositoryException(de).ErrorUnableToFetchRecord();
            }
        }

        public void Save(RegisteredDevice item)
        {
            if (item.Id > 0)
                _registeredDeviceRepository.Update(item);
            else
                _registeredDeviceRepository.Add(item);

            _registeredDeviceRepository.Commit();
        }

        public void Save(int id, string userId, string deviceId, string platform)
        {
            var entity = new RegisteredDevice
            {
                Id = id,
                AspNetUsersId = userId,
                DeviceId = deviceId,
                Platform = platform,
                LastAccessOnUtc = DateTime.UtcNow
            };

            Save(entity);
        }
        
    }
}
