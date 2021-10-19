using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PANOBOOKINGTAXI.DetailedViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingDetails : ContentPage
    {
        public BookingDetails(string OrderCodes, string Costs, string Dates, string Facilitys, string Quantitys, string Users, string DatesFroms, string DatesTos, string Takens)
        {
            InitializeComponent();
            OrderCode.Text = OrderCodes;
            Cost.Text = Costs;
            Date.Text = Dates;
            Facility.Text = Facilitys;
            Quantity.Text = Quantitys;
            DatesFrom.Text = DatesFroms;
            DatesTo.Text = DatesTos;
            Taken.Text = Takens;
            User.Text = Users;
        }
    }
}