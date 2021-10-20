
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PANOBOOKINGTAXI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public string Name { get; set; }
        public MainMenu()
        {
            InitializeComponent();
            var myValue = Preferences.Get("drivername", "drivername");
            Name = "Hello " + myValue;
            BindingContext = this; 
            mainlabel.Text = Name;
           
        }
        private async void Movetobooking(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new Bookings());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }

        }
        private async void Movetoschedule(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UpcomingSchedule());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }

        }
        private async void Movetoupdatedetails(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new UpdateDetails());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }

        }
        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private async void Movetoaccount(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new Account());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }
        }

        private async void Movetologout(object sender, EventArgs e)
        {
            try
            {
                string action = await DisplayActionSheet("Confirm Logout?", "Cancel", null, "Yes", "No");
                if (action == "Yes")
                {
                    Preferences.Clear();
                    await DisplayAlert("Information", "Successfully Logged Out", "Ok");
                    await Navigation.PopModalAsync();
                }
                else if (action == "Yes")
                {
                }

                }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Cancel");
            }
        }
    }
   
}