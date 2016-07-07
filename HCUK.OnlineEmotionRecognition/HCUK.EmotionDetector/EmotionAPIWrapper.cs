using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;
using System.Collections.Generic;

namespace HCUK.EmotionDetector
{
    public class EmotionAPIWrapper
    {
      
        static string accountKey = "4cba6e3d7d6a46cfb447015613c2de83";

        public static string GetEmotions(byte[] imageBytes)
        {
            string emotion = "";

            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", accountKey);

            var uri = "https://api.projectoxford.ai/emotion/v1.0/recognize?" + queryString;

            string response;
                      

            using (var content = new ByteArrayContent(imageBytes))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = client.PostAsync(uri, content).Result.Content.ReadAsStringAsync().Result;
            }

            try
            {

                var emotionScores = response.Split('}')[1].Split('{')[1].Split(',');

                List<string> emotions = new List<string>();
                List<Double> scores = new List<double>();

                foreach (var value in emotionScores)
                {
                    emotions.Add(value.Split(':')[0]);
                    scores.Add(double.Parse(value.Split(':')[1]));
                }


                var emotionArray = emotions.ToArray();
                var scoreArray = scores.ToArray();

                Array.Sort(scoreArray, emotionArray);
                

                emotion = emotionArray[emotionArray.Length - 1].Replace("\"", "");
            }
            catch(Exception ex) {
            }

            return emotion;

        }
    }
}
