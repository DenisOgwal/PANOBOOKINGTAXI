using System;
using System.Collections.Generic;
using System.Net.Http;
using Passingwind.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PANOBOOKINGTAXI.DetailedViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScheduleDetail : ContentPage
    {
        public ScheduleDetail(string OrderCodes, string Costs, string Dates, string Facilitys, string Quantitys, string Users, string DatesFroms, string DatesTos, string Takens)
        {
            InitializeComponent();
            OrderCode.Text = OrderCodes;
            Cost.Text = Costs;
            Date.Text = Dates;
            Facility.Text = Facilitys;
            Quantity.Text = Quantitys;
            DatesFrom.Text = DatesFroms;
            DatesTo.Text = DatesTos;
            User.Text = Users;
        }

        private async void Rejectbutton_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                string reason = await DisplayPromptAsync("Reason", "Please tell us you reasn for not taking on the Request?");
                if (reason == null)
                {
                    return;
                }
                var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Submitting...")
                {
                    Cancellable = true,
                    // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

                });

                var taxinos = Preferences.Get("taxino", "taxino"); ;
                var ordercode = OrderCode.Text;
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("taxinos", taxinos));
                postData.Add(new KeyValuePair<string, string>("ordercode", ordercode));
                postData.Add(new KeyValuePair<string, string>("reason", reason));

                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.RejectBooking;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (result.Trim() == "Success")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Booking Reject Confirmed", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Booking Reject Failed, Try Again Later", "OK");
                        return;
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

                await DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }

        private async void Confirmbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Confirming...")
                {
                    Cancellable = true,
                    // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

                });

                var taxinos = Preferences.Get("taxino", "taxino"); ;
                var ordercode = OrderCode.Text;
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("taxinos", taxinos));
                postData.Add(new KeyValuePair<string, string>("ordercode", ordercode));

                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.ConfirmBooking;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (result.Trim() == "Success")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Booking Confirmed", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Confirmation Failed, Try Again Later", "OK");
                        return;
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

                await DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }
      
    }
}