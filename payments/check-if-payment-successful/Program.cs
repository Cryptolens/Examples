using RestSharp;
using System;

namespace ConsoleApp14
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://app.cryptolens.io/Ext");

            var request = new RestRequest("CheckPayment", Method.POST);
            request.AddParameter("id", "3"); // adds to POST or URL querystring based on Method
            request.AddParameter("reference", "ch_1EhXLc20eEPYPuUEx0j4EZQa");
            request.AddParameter("orderId", "1908");

            IRestResponse response = client.Execute(request);
            var content = response.Content;

            if (content.Contains("error"))
            {
                Console.WriteLine("The order does not exist");
            }
            else
            {
                Console.WriteLine("The order exists");
            }


            Console.WriteLine(content);
        }
    }
}
