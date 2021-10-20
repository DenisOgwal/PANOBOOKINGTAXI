using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Passingwind.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PANOBOOKINGTAXI.Models;
using Xamarin.Essentials;
using PANOBOOKINGTAXI.DetailedViews;

namespace PANOBOOKINGTAXI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingSchedule : ContentPage
    {
        public List<BookingsListViewItem> items;
        public UpcomingSchedule()
        {
            InitializeComponent();
            populatelist();
            dosearchiteams();
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
                string con = Constants.Constants.SelectSchedule;
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
                    dialog.Hide();
                    items = new List<BookingsListViewItem>();
                    foreach (var objs in obj.error)
                    {
                        if (objs.OrderCode.Trim() == "No Schedule Found for this Vehicle so Far")
                        {
                            dialog.Hide();
                            await DisplayAlert("Information", "No Schedule Found for this Vehicle so Far", "OK");
                            await Navigation.PopModalAsync();
                            return;
                        }
                        else
                        {
                            items.Add(new BookingsListViewItem { OrderCode = objs.OrderCode.Trim(), Cost = "$" + objs.Cost, Date = objs.Date, Facility = objs.Facility, Quantity = objs.Quantity, User = objs.User, DatesFrom = objs.DatesFrom, DatesTo = objs.DatesTo, Taken = objs.Taken });
                        }
                    }
                    schedulelist.ItemsSource = items;
                    //}
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
        void dosearchiteams()
        {
            try
            {
                searchschedule.TextChanged += (s, e) => FilterItem(searchschedule.Text);
                searchschedule.SearchButtonPressed += (s, e) => FilterItem(searchschedule.Text);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }

        private void FilterItem(string filter)
        {
            try
            {
                schedulelist.BeginRefresh();
                if (string.IsNullOrWhiteSpace(filter))
                {
                    schedulelist.ItemsSource = items;
                }
                else
                {
                    schedulelist.ItemsSource = items.Where(x => x.OrderCode.ToLower().Contains(filter));
                    //bookingslist.ItemsSource = items.Where(x => x.itemcomcode.ToLower().Contains(filter) || x => x.dates.ToLower().Contains(filter) || (x => x.quantity.ToLower().Contains(filter)));
                }
                schedulelist.EndRefresh();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }

        private async void scheduleitem_IsTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {

                var realindex2 = e.ItemIndex.ToString();
                var mydetails = e.Item as BookingsListViewItem;
                var oder = mydetails.OrderCode;
                await Navigation.PushModalAsync(new ScheduleDetail(mydetails.OrderCode, mydetails.Cost, mydetails.Date, mydetails.Facility, mydetails.Quantity, mydetails.User, mydetails.DatesFrom, mydetails.DatesTo, mydetails.Taken));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
}