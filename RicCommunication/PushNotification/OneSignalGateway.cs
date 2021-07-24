using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RicCommunication.Interface;

namespace RicCommunication.PushNotification
{
    public class OneSignalGateway : IPushNotificationGateway
    {
        private readonly string _oneSignalAuthKey;
        private readonly string _oneSignalAppId;

        public OneSignalGateway(string oneSignalAuthKey, string oneSignalAppId)
        {
            if (string.IsNullOrEmpty(oneSignalAuthKey)) throw new ArgumentNullException("oneSignalAuthKey");
            if (string.IsNullOrEmpty(oneSignalAppId)) throw new ArgumentNullException("oneSignalAppId");

            _oneSignalAppId = oneSignalAppId;
            _oneSignalAuthKey = oneSignalAuthKey;
        }

        public bool SendNotification(string portalUserId, IEnumerable<string> devicesIds, string title, string message)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(portalUserId))
                success = SendNotificationToExternalIds(new List<string> { portalUserId }, title, message);
            if (!success)
                success = SendNotificationToPlayerIds(devicesIds?.ToList(), title, message);
            return success;
        }

        public bool SendNotification(IList<string> portalUserIds, IEnumerable<string> devicesIds, string title, string message)
        {
            bool success = SendNotificationToExternalIds(portalUserIds, title, message);
            if (!success)
                success = SendNotificationToPlayerIds(devicesIds?.ToList(), title, message);
            return success;
        }

        private bool SendNotificationToPlayerIds(IList<string> deviceIds, string title, string message)
        {
            if (deviceIds == null || !deviceIds.Any())
                return false;

            if (string.IsNullOrEmpty(title))
                title = "Ric Monitoring";

            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", $"Basic {_oneSignalAuthKey}");

            var obj = new
            {
                app_id = _oneSignalAppId,
                ios_sound = "bingbong.aiff",
                contents = new { en = message },
                headings = new { en = title },
                ios_badgeType = "Increase",
                ios_badgeCount = 1,
                include_player_ids = deviceIds.ToArray()
            };
            var param = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        var oneSignalResponse = JsonConvert.DeserializeObject<OneSignalResponse>(responseContent);
                        if (oneSignalResponse != null && oneSignalResponse.errors != null)
                            return false;
                    }
                }
            }
            catch (WebException ex)
            {
                string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return false;
            }
            return true;
        }

        private bool SendNotificationToExternalIds(IList<string> portalUserIds, string title, string message)
        {
            if (portalUserIds == null || !portalUserIds.Any())
                return false;

            if (string.IsNullOrEmpty(title))
                title = "Ric Monitoring";

            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", $"Basic {_oneSignalAuthKey}");

            var obj = new
            {
                app_id = _oneSignalAppId,
                ios_sound = "bingbong.aiff",
                contents = new { en = message },
                headings = new { en = title },
                include_external_user_ids = portalUserIds.ToArray(),
                ios_badgeType = "Increase",
                ios_badgeCount = 1,
                channel_for_external_user_ids = "PUSH"
            };
            var param = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        var oneSignalResponse = JsonConvert.DeserializeObject<OneSignalResponse>(responseContent);
                        if (oneSignalResponse != null && oneSignalResponse.errors != null)
                            return false;
                    }
                }
            }
            catch (WebException ex)
            {
                string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                return false;
            }
            return true;
        }

        public OneSignalDeviceInfo IsDeviceIdValid(string deviceId)
        {
            var request = WebRequest.Create($"https://onesignal.com/api/v1/players/{deviceId}?app_id={_oneSignalAppId}") as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", $"Basic {_oneSignalAuthKey}");

            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        var oneSignalResponse = JsonConvert.DeserializeObject<OneSignalDeviceInfo>(responseContent);
                        return oneSignalResponse;
                    }
                }
            }
            catch (WebException ex)
            {
                string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return null;
        }

        public bool UpdateDeviceExternalUserId(string deviceId, string externalUserId)
        {
            var request = WebRequest.Create($"https://onesignal.com/api/v1/players/{deviceId}") as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "PUT";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("authorization", $"Basic {_oneSignalAuthKey}");

            var obj = new
            {
                app_id = _oneSignalAppId,
                external_user_id = externalUserId
            };
            var param = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        return true;
                    }
                }
            }
            catch (WebException ex)
            {
                string responseContent = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }
            return false;
        }
    }

    public class OneSignalResponse
    {
        public string id { get; set; }
        public int recipients { get; set; }
        public string external_id { get; set; }
        public object errors { get; set; }
    }

    public class OneSignalDeviceInfo
    {
        public string identifier { get; set; }
        public int session_count { get; set; }
        public string language { get; set; }
        public string device_os { get; set; }
        public int device_type { get; set; }
        public string device_model { get; set; }
        public bool invalid_identifier { get; set; }
        public string external_user_id { get; set; }
    }
}
