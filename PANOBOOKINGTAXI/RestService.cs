using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PANOBOOKINGTAXI
{
    public class RestService
    {
        HttpClient client;


        public RestService()
        {
            client = new HttpClient();


        }
    }
}
