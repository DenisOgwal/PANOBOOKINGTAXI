using PANOBOOKINGTAXI.Models;
using PANOBOOKINGTAXI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using Newtonsoft.Json;
using Passingwind.UserDialogs;
using PANOBOOKINGTAXI.Views;
using System.Collections.ObjectModel;
using PANOBOOKINGTAXI.DetailedViews;

namespace PANOBOOKINGTAXI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

   
    public partial class Bookings : ContentPage
    {
       
       public List<BookingsListViewItem> items;

        public Bookings()
        {
            InitializeComponent();
            populatelist();
           dosearchiteams();
           
        }
       
        private async void populatelist(){
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
                string con = Constants.Constants.SelectBookings;
                Uri uri = new Uri(string.Format(con, string.Empty));
                client.BaseAddress = new Uri(con);
                client.DefaultRequestHeaders.Accept.Clear();
                client.Timeout = new TimeSpan(0, 2, 0);
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ChatResponse> (result);
                    dialog.Hide();
                    items = new List<BookingsListViewItem>();
                    foreach (var objs in obj.error)
                    {
                        items.Add(new BookingsListViewItem { OrderCode = objs.OrderCode.Trim(), Cost = "$"+objs.Cost, Date = objs.Date, Facility = objs.Facility, Quantity = objs.Quantity, User = objs.User, DatesFrom = objs.DatesFrom, DatesTo = objs.DatesTo, Taken = objs.Taken });
                    }
                    bookingslist.ItemsSource = items;

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
            try { 
            searchbooking.TextChanged += (s, e) => FilterItem(searchbooking.Text);
            searchbooking.SearchButtonPressed += (s, e) => FilterItem(searchbooking.Text);
            }
            catch (Exception ex)
            { 
                DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }

        private void FilterItem(string filter)
        {
            try { 
            bookingslist.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter)) {
               bookingslist.ItemsSource = items;
            }
            else{
                bookingslist.ItemsSource = items.Where(x => x.OrderCode.ToLower().Contains(filter) );
                //bookingslist.ItemsSource = items.Where(x => x.itemcomcode.ToLower().Contains(filter) || x => x.dates.ToLower().Contains(filter) || (x => x.quantity.ToLower().Contains(filter)));
            }
            bookingslist.EndRefresh();
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Cancel");
                return;
            }
        }

        private async void bookingitem_IsTabbed(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                //var realindex = (bookingslist.ItemsSource as List<BookingsListViewItem>).IndexOf(e.SelectedItem as Person);
                var realindex2 = e.ItemIndex.ToString();
                var mydetails = e.Item as BookingsListViewItem;
                var oder = mydetails.OrderCode;
                //DisplayAlert("Error", realindex2.ToString(), "Cancel");
                // DisplayAlert("Error", oder.ToString(), "Cancel");
                await Navigation.PushModalAsync(new BookingDetails( mydetails.OrderCode, mydetails.Cost, mydetails.Date, mydetails.Facility, mydetails.Quantity, mydetails.User, mydetails.DatesFrom, mydetails.DatesTo, mydetails.Taken));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }
           
        }
     
    }
   
}