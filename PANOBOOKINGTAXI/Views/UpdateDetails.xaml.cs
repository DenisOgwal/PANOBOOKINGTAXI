using Newtonsoft.Json;
using Passingwind.UserDialogs;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PANOBOOKINGTAXI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDetails : ContentPage
    {
        public UpdateDetails()
        {
            InitializeComponent();
            Fetchuserdetails();
        }
        string CarType = null;
        int countimages = 0;
        private async void Fetchuserdetails()
        {
            var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Fetching Details...")
            {
                Cancellable = true,
                // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

            });

            try
            {
               
                var taxinos = Preferences.Get("taxino", "taxino");
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("taxinos", taxinos));
              
                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.SelectUserDetails;
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
                        fullnames.Text = responses.DriverName.ToString();
                        email.Text= responses.EmailAddress.ToString();
                        phonenumber.Text = responses.TelephoneNumber.ToString();
                        carspecifications.Text = responses.CarSpec.ToString();
                        parkinglocation.Text = responses.CurrentLocation.ToString();
                        description.Text = responses.Repayments.ToString();
                        CarType= responses.CarType.ToString();
                        countimages = Convert.ToInt32(responses.PhotoUri);
                        if (responses.DOB.ToString() !="")
                        {
                            DateTime det2 = DateTime.ParseExact(responses.DOB.ToString().Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            dob.Date = det2;
                        }
                    }
                    else
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Fetching Details Failed, Try Again Later", "OK");
                        await Navigation.PopModalAsync();
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
                dialog.Hide();
                await DisplayAlert("Error", ex.Message, "Cancel");
                await Navigation.PopModalAsync();
                return;
            }
        }
        private async void Updatebutton_Clicked(object sender, EventArgs e)
        {
            var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Updating Details ...")
            {
                Cancellable = true,
                // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

            });
            try
            {
               
                var fullnamess = fullnames.Text;
                var emails = email.Text;
                var phonenumbers = phonenumber.Text;
                var dobs = dob.Date.ToString("dd/MM/yyyy");
                var registrationnumbers = Preferences.Get("taxino", "taxino");
                var carspecificationss = carspecifications.Text;
                var parkinglocations = parkinglocation.Text;
                var descriptions = description.Text;
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("fullnames", fullnamess));
                postData.Add(new KeyValuePair<string, string>("emails", emails));
                postData.Add(new KeyValuePair<string, string>("phonenumbers", phonenumbers));
                postData.Add(new KeyValuePair<string, string>("dobs", dobs));
                postData.Add(new KeyValuePair<string, string>("registrationnumbers", registrationnumbers));
                postData.Add(new KeyValuePair<string, string>("carspecificationss", carspecificationss));
                postData.Add(new KeyValuePair<string, string>("parkinglocations", parkinglocations));
                postData.Add(new KeyValuePair<string, string>("descriptions", descriptions));
                postData.Add(new KeyValuePair<string, string>("pickers", CarType));
                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.UpdateUserDetails;
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
                        Preferences.Set("drivername", fullnamess);
                        await DisplayAlert("Information", "Your Details Update is Successful.", "OK");
                       // await Navigation.PushModalAsync(new MainPage());
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Your Details Update Failed.", "OK");
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
                dialog.Hide();
                await DisplayAlert("Error", ex.Message, "Cancel");
                await Navigation.PopModalAsync();
            }
        }

        private async void Updatepasswordbutton_Clicked(object sender, EventArgs e)
        {
            var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Updating Password ...")
            {
                Cancellable = true,
                // CancelAction = () => DisplayAlert("Canceled","you canceled.","Cancel"),

            });
            try
            {
                
               
                var registrationnumbers = Preferences.Get("taxino", "taxino");
                var newpasswords = newpassword.Text;
                var oldpasswords = oldpassword.Text;
                var confirmnewpasswords = confirmnewpassword.Text;
                
                bool resultss = newpasswords.Equals(confirmnewpasswords);
                if (resultss == true) { }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Error", "Confirm Password does Not Match New Password", "OK");
                    confirmnewpassword.Text = "";
                    return;
                }
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("password", oldpasswords));
                postData.Add(new KeyValuePair<string, string>("registrationnumbers", registrationnumbers));
                postData.Add(new KeyValuePair<string, string>("newpassword", newpasswords));
                postData.Add(new KeyValuePair<string, string>("pickers", CarType));
                var content = new FormUrlEncodedContent(postData);
                HttpClient client = new HttpClient();
                string con = Constants.Constants.UpdatePassword;
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
                        await DisplayAlert("Information", "Your Password Update is Successful.", "OK");
                        await Navigation.PopModalAsync();
                    }
                    else if (result.Trim() == "Failed")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Your Password Update is Failed.", "OK");
                       
                    }
                    else if (result.Trim() == "Wrong Old Password")
                    {
                        dialog.Hide();
                        await DisplayAlert("Information", "Wrong Old Password.", "OK");
                        oldpassword.Text = "";
                        oldpassword.Focus();
                        return;
                    }
                }
                else
                {
                    dialog.Hide();
                    await DisplayAlert("Information", response.StatusCode.ToString(), "OK");
                    await Navigation.PopModalAsync();
                }
            }
            catch (Exception ex)
            {
                dialog.Hide();
                await DisplayAlert("Error", ex.Message, "Cancel");
                await Navigation.PopModalAsync();
               
            }
        }
        public class Root
        {
            public string error { get; set; }
            public string DriverName { get; set; }
            public string CarSpec { get; set; }
            public string Repayments { get; set; }
            public string CurrentLocation { get; set; }
            public string EmailAddress { get; set; }
            public string TelephoneNumber { get; set; }
            public string CarType { get; set; }
            public string DOB { get; set; }
            public string PhotoUri { get; set; }
        }
        //private MediaFile result;
        private FileResult result;
        string registrationnumbers = Preferences.Get("taxino", "taxino");
        string PhotoPath = null;
        string fileName = null;
        private async void Choosefromgallerybutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();
                string action = await DisplayActionSheet("Choose options", "Cancel", null, "Camera", "Gallery");
                if (action == "Camera")
                {

                    //result = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { Directory = "PanoImages", Name = registrationnumbers });
                    result = await MediaPicker.CapturePhotoAsync();
                    var stream = await result.OpenReadAsync();
                    viewmyimage.Source = ImageSource.FromStream(() => stream);
                    var paths = result.FullPath;
                    fileName = result.FileName;
                    await LoadPhotoAsync(result);
                    //viewmyimage.Source = ImageSource.FromStream(() => { return result.GetStream(); });
                    //await DisplayAlert("Error", PhotoPath, "Ok");
                }
                else if (action == "Gallery")
                {
                    result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Please Choose Image from Gallery" });                    
                    var stream = await result.OpenReadAsync();
                    viewmyimage.Source = ImageSource.FromStream(() => stream);
                    var paths = result.FullPath;
                    fileName = result.FileName;
                    await LoadPhotoAsync(result);
                    // viewmyimage.Source=ImageSource.FromStream(() => { return result.GetStream();});
                    //await DisplayAlert("Error", paths, "Ok");
                }

            }
            catch (Exception ex )
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
       
        private async void Takephotobutton_Clicked(object sender, EventArgs e)
        {
            var dialog = UserDialogs.Instance.Progress(new ProgressConfig("Uploading...")
            {
                Cancellable = true,
            });
            try {
                fileName = result.FileName;
                string fileExt = fileName.Substring(fileName.Length - 4);
                string fName = null;
                if (countimages == 0)
                {
                    fName = registrationnumbers + fileExt.ToLower();
                }
                else
                {
                    fName = registrationnumbers+ countimages + fileExt.ToLower();
                }
                byte[] bitmapData;
                var streams = new MemoryStream();
                var stream = await result.OpenReadAsync();
                stream.CopyTo(streams);
                bitmapData = streams.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "fileToUpload",
                    FileName = fName
                };
                string boundary = "---8393774hhy37373773";
                var content = new MultipartFormDataContent(boundary);
                content.Add(fileContent);
         
                var httpclient = new HttpClient(){ Timeout = TimeSpan.FromSeconds(900)};
                var uploadServiceBaseAddress = Constants.Constants.UploadImages;
                httpclient.BaseAddress = new Uri(Constants.Constants.Baseadd);
                var httpResponseMessage = await httpclient.PostAsync(uploadServiceBaseAddress, content);
                var remotepath =httpResponseMessage.Content.ReadAsStringAsync().Result;
              
                if (httpResponseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await DisplayAlert("Information", remotepath, "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Information", remotepath, "Ok");
                    await Navigation.PopModalAsync();
                }
                dialog.Hide();
            }
            catch (Exception ex)
            {
                dialog.Hide();
                await DisplayAlert("Error", ex.Message, "Ok");
            }
            
        }
        public static byte[] ToArray(Stream s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.CanRead)
                throw new ArgumentException("Stream cannot be read");

            MemoryStream ms = s as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            long pos = s.CanSeek ? s.Position : 0L;
            if (pos != 0L)
                s.Seek(0, SeekOrigin.Begin);

            byte[] result = new byte[s.Length];
            s.Read(result, 0, result.Length);
            if (s.CanSeek)
                s.Seek(pos, SeekOrigin.Begin);
            return result;
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
            await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }


    }
}