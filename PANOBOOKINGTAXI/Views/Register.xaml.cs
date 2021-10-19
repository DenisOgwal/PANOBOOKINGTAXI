using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Passingwind.UserDialogs;

namespace PANOBOOKINGTAXI.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }
        private async void Movetologin(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync();
            await Navigation.PushModalAsync(new MainPage());
           
        }
      
            private async void registerbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Registering ...")
                {
                    Cancellable = true,
                    // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

                });
                var fullnamess = fullnames.Text;
                var emails = email.Text;
                var phonenumbers = phonenumber.Text;
                var dobs = dob.Date.ToString("dd/MM/yyyy");
                var registrationnumbers = registrationnumber.Text;
                var carspecificationss = carspecifications.Text;
                var parkinglocations = parkinglocation.Text;
                var descriptions = description.Text;
                var passwords = password.Text;
                var confirmpasswords = confirmpassword.Text;
                var pickers = picker.SelectedItem.ToString();
                bool resultss = passwords.Equals(confirmpasswords);
                if (resultss == true) { }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Error", "Passwords do Not Match", "OK");
                    confirmpassword.Text = "";
                    return;
                }
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("fullnames", fullnamess));
                postData.Add(new KeyValuePair<string, string>("password", passwords));
                postData.Add(new KeyValuePair<string, string>("emails", emails));
                postData.Add(new KeyValuePair<string, string>("phonenumbers", phonenumbers));
                postData.Add(new KeyValuePair<string, string>("dobs", dobs));
                postData.Add(new KeyValuePair<string, string>("registrationnumbers", registrationnumbers));
                postData.Add(new KeyValuePair<string, string>("carspecificationss", carspecificationss));
                postData.Add(new KeyValuePair<string, string>("parkinglocations", parkinglocations));
                postData.Add(new KeyValuePair<string, string>("descriptions", descriptions));
                postData.Add(new KeyValuePair<string, string>("pickers", pickers));
                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.RegisterDriver;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if(result.Trim()== "Success")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Your Registration is Successful. \n Wait for Approval from App Administrator to Login", "OK");
                        await Navigation.PushModalAsync(new MainPage());
                        //await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Information", response.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Cancel");
                return;
            }
        }
    }
}