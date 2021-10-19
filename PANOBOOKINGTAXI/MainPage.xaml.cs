using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Passingwind.UserDialogs;
using Xamarin.Essentials;

namespace PANOBOOKINGTAXI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            bool hasKey = Preferences.ContainsKey("taxino");
            if (hasKey==true)
            {
               Navigation.PushModalAsync(new Views.MainMenu());
            }
        }
        private void Movetoregister(object sender, EventArgs e)
        {
            App.Current.MainPage = new Views.Register();
            //Navigation.RemovePage(this);
        }

        private async void loginbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Logging In...")
                {
                    Cancellable = true,
                   // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

                });

                var taxinos = taxino.Text;
                var passwords = password.Text;
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("taxinos", taxinos));
                postData.Add(new KeyValuePair<string, string>("passwords", passwords));

                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.LoginDriver;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Root responses = JsonConvert.DeserializeObject<Root>(result);

                    if (responses.error == "Correct Info")
                    {
                        dialog.Hide();
                        string taxisn = responses.TaxiNo.ToString();
                        string drivername = responses.DriverName.ToString();
                        Preferences.Set("taxino", taxisn);
                        Preferences.Set("drivername", drivername);
                        //await Navigation.PopModalAsync();
                        // await DisplayAlert("Information", drivername, "OK");
                        await Navigation.PushModalAsync(new Views.MainMenu());
                    }
                    else if (responses.error == "Something Un Expected Happened, Try Again Later")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Something Un Expected Happened, Try Again Later", "OK");
                        taxino.Text = "";
                        password.Text = "";
                        return;
                    }
                    else if (responses.error == "Password User Name Do not Match any Approved Account")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Password User Name Do not Match any Approved Account", "OK");
                        taxino.Text = "";
                        password.Text = "";
                        return;
                    }
                    else
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Unexpected Result, Try Again", "OK");
                        taxino.Text = "";
                        password.Text = "";
                        return;
                    }
                }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Information", response.StatusCode.ToString(), "OK");
                }
              
            }
            catch (Exception)
            {
                await DisplayAlert("Error","Account Logins Provided are Wrong","Cancel");
                return;
            }

        }
        public class Root
        {
            public string error { get; set; }
            public string DriverName { get; set; }
            public string TaxiNo { get; set; }
        }
        
    }
}
