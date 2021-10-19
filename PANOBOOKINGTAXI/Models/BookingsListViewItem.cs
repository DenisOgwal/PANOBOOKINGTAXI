using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PANOBOOKINGTAXI.Models
{
    public class BookingsListViewItem
    {
        public string OrderCode { get; set; }
        public string Cost { get; set; }
        public string Date { get; set; }
        public string error { get; set; }
         public string Facility { get; set; }
         public string Quantity { get; set; }
         public string User { get; set; }
         public string DatesFrom { get; set; }
         public string DatesTo { get; set; }
         public string Taken { get; set; }
    }
    public class ChatResponse
    {
        public List<BookingsListViewItem> error { get; set; }

    }
    /* public ChatResponse oldproduct;
     public productdetails productdesc;
     public ObservableCollection<ChatResponse> Products { get; set; }
     public BookingsListViewItem() {
         Products = new ObservableCollection<ChatResponse>()
         {
             new ChatResponse
             {
             OrderCode = oldproduct.OrderCode,
             Cost= oldproduct.OrderCode,
             Date= oldproduct.OrderCode,
             plusimage="plus.jpg",
             minusimage="minus.png",
             IsVisible =false
            /* productdetailss=new ObservableCollection<productdetails>
             {
                 new  productdetails {Facility=productdesc.Facility,Quantity=productdesc.Quantity,User=productdesc.User,DatesFrom=productdesc.DatesFrom,DatesTo=productdesc.DatesTo,Taken=productdesc.Taken }
             }
             }
         };
     }
     public void show_hide(ChatResponse product)
     {

     }

     public void updateproduct(ChatResponse product)
     {
         var index = Products.IndexOf(product);
         Products.Remove(product);
         Products.Insert(index, product);
     }
 }
 public class ChatResponse
 {
     public string OrderCode { get; set; }
     public string Cost { get; set; }
     public string Date { get; set; }
     public string plusimage { get; set; }
     public string minusimage { get; set; }
     public bool IsVisible { get; set; }
     //public ObservableCollection<productdetails> productdetailss { get; set; }
 }
 public class productdetails
 {
     public string Facility { get; set; }
     public string Quantity { get; set; }
     public string User { get; set; }
     public string DatesFrom { get; set; }
     public string DatesTo { get; set; }
     public string Taken { get; set; }
 }*/
}
