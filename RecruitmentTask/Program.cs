using System;
using System.Net;
using Newtonsoft.Json;
using System.IO;


namespace ExampleOfDownloading
{
    class Program
    {

        public static bool IsReachableLink(string inputLink)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputLink); // Creating the HttpWebRequest from recieved link
            request.Timeout = 15000; // Setting the request timeout and method
            request.Method = "HEAD";

            try
            {
                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode == HttpStatusCode.OK; // After getting the Web response it returns TRUE if the status code == 200 which is OK
            }
            catch (WebException) // Any exception will return false
            {
                return false;
            }
        }

        public static bool IsActiveLink(string inputLink)
        {
            try
            {
                // Creating a new Uri from inputLink and comparing it to schemes of both Http and Https
                bool result = Uri.TryCreate(inputLink, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }
            catch (WebException) // Again any exception will return false
            {
                return false;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Input your URL links: ");
            string inputLinks = Console.ReadLine(); // Reading input from a single line 
            string[] separatedLinks = inputLinks.Split(';'); // Splitting each url by ';' char into an array
            Console.WriteLine("Input your destination path for downloads: ");
            string path = Console.ReadLine(); // Providing destination folder
            int nameNum = 1; // For naming convenience

            foreach (var link in separatedLinks) // Looping through each separate link
            {
                if (IsReachableLink(link) && IsActiveLink(link)) // Checking if links are reachable and active
                {
                    var httpRequest = (HttpWebRequest)WebRequest.Create(link); // Creating a Web request that accepts applicaiton/json
                    httpRequest.Accept = "application/json";

                    var httpResponse = (HttpWebResponse)httpRequest.GetResponse(); // After receiving response from the request assign it to new var
                    using var streamReader = new StreamReader(httpResponse.GetResponseStream(), true); // Saving the data with StreamReader
                    var resultFile = streamReader.ReadToEnd();
                    string fileName = "JsonFile" + nameNum + ".json";
                    File.WriteAllText(path + fileName, resultFile); // Writing a new file with all 
                }
                else
                {
                    Console.WriteLine("Provided URLs are not valid");
                }
                nameNum++; // Increasing for next link that is provided
            }
        }
    }
}