using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio;

namespace RainNotice
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckWeather("94158") || CheckWeather("95113"))
            {
                SendTextMessage("It's going to rain today! Bring an umbrella!");
            }
        }

        static void SendTextMessage(string text)
        {
            string AccountSid = REMOVED_KEY;
            string AuthToken = REMOVED_TOKEN;
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            var message = twilio.SendSmsMessage("+14155085433", REMOVED_PHONE_NUMBER_TO, text, "");
  
        }

        static bool CheckWeather(string zip)
        {
            var s = GetJsonResult(string.Format("http://api.wunderground.com/api/" + REMOVED_KEY + "/hourly/q/{0}.json",zip));
            var obj = (JObject)JsonConvert.DeserializeObject(s);
            foreach (var i in obj["hourly_forecast"])
            {
                if (Convert.ToInt32(i["FCTTIME"]["mday"].ToString()) == (int)DateTime.Now.Day)
                {
                    if (i["condition"].ToString().ToLower().IndexOf("rain") > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static string GetJsonResult(string url)
        {
            // Create a request for the URL. 		
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            //Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content. 
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }

    }
}
