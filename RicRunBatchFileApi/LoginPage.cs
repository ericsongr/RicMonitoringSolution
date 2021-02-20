using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Net.Http;
using RicRunBatchFileApi.Properties;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using RicRunBatchFileApi.ViewModels;
using Microsoft.Win32;

namespace RicRunBatchFileApi
{
    public partial class LoginPage : Form
    {
        private string authAddress = "";
        private string baseAddress = "";
        private int startRunInMinutes = 0;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void SetSettings()
        {
            var appSettings = ConfigurationSettings.AppSettings;
            authAddress = appSettings["AuthAddress"];
            baseAddress = appSettings["BaseAddress"];
            startRunInMinutes = Int16.Parse(appSettings["StartRunInMinutes"]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetSettings();

            
            Form form = (Form)sender;
            form.Location = new Point(-10000, -10000);
            form.ShowInTaskbar = false;
            form.Opacity = 0;

            Displaynotify();
        }

        protected void Displaynotify()
        {
            try
            {
                var minutes = startRunInMinutes;

                //NotifyUser.Icon = new System.Drawing.Icon(Path.GetFullPath(@"images\gear.ico"));
                NotifyUser.Text = "Run Daily Batch File";
                NotifyUser.Visible = true;
                NotifyUser.BalloonTipTitle = $"Run Daily Batch file in {minutes} minutes";
                NotifyUser.BalloonTipText = "...";
                NotifyUser.ShowBalloonTip(100);

                timer1.Enabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected void DisplayNotification(string message)
        {
            try
            {
                NotifyUser.Icon = new System.Drawing.Icon(Path.GetFullPath(@"images\gear.ico"));
                NotifyUser.Text = "Run Daily Batch File";
                NotifyUser.Visible = true;
                NotifyUser.BalloonTipTitle = message;
                NotifyUser.BalloonTipText = "...";
                NotifyUser.ShowBalloonTip(100);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected void RunDailyBatchFile()
        {
            using (var authClient = new HttpClient())
            {
                authClient.BaseAddress = new Uri(authAddress);
                authClient.DefaultRequestHeaders.Clear();
                authClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = JsonConvert.SerializeObject(new LoginInputModel
                {
                    DeviceId = "9267EDAE-2A8C-4EE2-AF49-84F57171F552",
                    Password = "Pa$$w0rd",
                    Platform = "android",
                    Username = "RunDailyBatch"
                });
                
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = authClient.PostAsync("api/account/login", content).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var dataResponse = response.Content.ReadAsStringAsync().Result;
                    var t = JsonConvert.DeserializeObject<BaseRestApiModel>(dataResponse);
                    var payload = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(t.Payload));
                    var accessData = payload["accessToken"];

                    using (var baseClient = new HttpClient())
                    {
                        baseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessData);
                        baseClient.BaseAddress = new Uri(baseAddress);
                        baseClient.DefaultRequestHeaders.Accept.Clear();
                        baseClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage responseBatch = baseClient.PostAsync("api/exec-store-proc", null).Result;
                        if (responseBatch.StatusCode == HttpStatusCode.OK)
                        {
                            DisplayNotification("Daily batch has been completed.");
                        }
                        else
                        {
                            DisplayNotification("Error on Daily batch. Please contact system administrator.");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Error occurred. Please contact system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            this.Close();
            this.Dispose();
        }

        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            int seconds = startRunInMinutes * 60;
            count++;
            if (count == seconds)
            {
                timer1.Enabled = false;
                count = 0;

                DisplayNotification("Daily Batch File - Start running...");

                RunDailyBatchFile();

            }
        }
    }
}
