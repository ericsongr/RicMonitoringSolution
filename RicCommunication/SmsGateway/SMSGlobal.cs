using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RicCommunication.Interface;
using RicCommunication.Model;

namespace RicCommunication.SmsGateway
{
    public class SMSGlobal : ISMSGateway
    {
        string _gatewayUrl = "https://www.smsglobal.com/http-api.php?";

        public string Username { get; set; }

        public string Password { get; set; } = string.Empty;

        public SMSGlobal(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            Username = username;
            Password = password;
        }

        public SMSGlobal(string username, string password, string gatewayUrl)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(gatewayUrl)) throw new ArgumentNullException(nameof(gatewayUrl));

            Username = username;
            Password = password;
            _gatewayUrl = gatewayUrl;
        }

        public SMSGlobal()
        { }

        public string ProviderName => "SMSGlobal";

        public bool SendSMS(string fromNumber, string toNumber, string text)
        {
            if (string.IsNullOrWhiteSpace(fromNumber)) throw new ArgumentNullException(nameof(fromNumber));
            if (string.IsNullOrWhiteSpace(toNumber)) throw new ArgumentNullException(nameof(toNumber));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            var asciiSmsText = string.Concat(text.Normalize(NormalizationForm.FormD).Where(
                c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

            const int minNumberLength = 7;

            if (toNumber.Length > minNumberLength)
                return !SendSms(_gatewayUrl, SMSGlobalData(Username, Password, fromNumber, toNumber, asciiSmsText)).StartsWith("ERROR");

            return false;
        }

        public IList<SmsMessage> GetReplies()
        {
            return new List<SmsMessage>();
        }

        public bool DeleteReply(string messageId)
        {
            return false;
        }

        public string SendSMSv2(string fromNumber, string toNumber, string text)
        {
            if (string.IsNullOrWhiteSpace(fromNumber)) throw new ArgumentNullException(nameof(fromNumber));
            if (string.IsNullOrWhiteSpace(toNumber)) throw new ArgumentNullException(nameof(toNumber));
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            const int minNumberLength = 7;

            if (toNumber.Length > minNumberLength)
                return SendSms(_gatewayUrl, SMSGlobalData(Username, Password, fromNumber, toNumber, text));
            return "";
        }

        private static string SendSms(string url, string postData)
        {

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);
            string responseFromServer = string.Empty;

            HttpWebRequest smsRequest = (HttpWebRequest)System.Net.WebRequest.Create(url);
            smsRequest.Method = "POST";
            smsRequest.ContentType = "application/x-www-form-urlencoded";
            smsRequest.ContentLength = data.Length;

            System.IO.Stream smsDataStream = null;
            smsDataStream = smsRequest.GetRequestStream();
            smsDataStream.Write(data, 0, data.Length);
            smsDataStream.Close();

            System.Net.WebResponse smsResponse = smsRequest.GetResponse();

            using (Stream responseStream = smsResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                responseFromServer = reader.ReadToEnd();
            }

            return responseFromServer;
        }

        private string SMSGlobalData(string username, string password, string source, string destination, string text)
        {

            var postData = new System.Text.StringBuilder("action=sendsms");
            postData.Append("&user=");
            postData.Append(System.Web.HttpUtility.UrlEncode(username));
            postData.Append("&password=");
            postData.Append(System.Web.HttpUtility.UrlEncode(password));
            postData.Append("&from=");
            postData.Append(System.Web.HttpUtility.UrlEncode(source));
            postData.Append("&to=");
            postData.Append(System.Web.HttpUtility.UrlEncode(destination));
            postData.Append("&text=");
            postData.Append(System.Web.HttpUtility.UrlEncode(text));

            return postData.ToString();

        }

    }
}
