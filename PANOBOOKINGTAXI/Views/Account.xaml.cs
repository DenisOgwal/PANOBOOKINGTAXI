using Newtonsoft.Json;
using PANOBOOKINGTAXI.Models;
using Passingwind.UserDialogs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PANOBOOKINGTAXI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        public string Names { get; set; }
        public List<BookingsListViewItem> items;
        public Account()
        {
            InitializeComponent();
            try
            {
                var myValue = Preferences.Get("drivername", "drivername");
                Names = "Hello Mr. " + myValue;
                BindingContext = this;
                username.Text = Names;
                populatelist();
            }
            catch (Exception ex)
            {

                DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }
        private async void populatelist()
        {
            var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Loading...")
            {
                Cancellable = true,
                // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

            });
            try
            {

                var taxino = Preferences.Get("taxino", "taxino");

                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("taxinos", taxino));
                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.SelectAllBooking;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ChatResponse>(result);
                    items = new List<BookingsListViewItem>();
                    foreach (var objs in obj.error)
                    {
                        if (objs.OrderCode.Trim() == "No Booking Found for this Vehicle so Far")
                        {
                            dialog.Hide();
                            await DisplayAlert("Information", "No Booking Found for this Vehicle so Far", "OK");
                            await Navigation.PopModalAsync();
                            return;
                        }
                        else
                        {
                            items.Add(new BookingsListViewItem { OrderCode = objs.OrderCode.Trim(), Cost = "$" + objs.Cost, Date = objs.Date, Facility = objs.Facility, Quantity = objs.Quantity, User = objs.User, DatesFrom = objs.DatesFrom, DatesTo = objs.DatesTo, Taken = objs.Taken });
                        }
                    }
                    dialog.Hide();

                }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Information", response.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                dialog.Hide();
                await DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }

        }

    }
}