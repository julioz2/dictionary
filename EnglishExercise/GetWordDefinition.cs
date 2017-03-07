using System;
using Newtonsoft.Json;

namespace EnglishExercise
{
    public class GetWordDefinition
    {
        private static string _appId = "0e0cada6";
        private static string _appKey = "44ebc91ccbad961c1fb0cdf470a16014";
        private static string _lang = "en";
        private static string _word = "";
        private static string _url = "";

        public void OxfordRequest(string word)
        {
            _word = word.ToLower();
            _url = $"https://od-api.oxforddictionaries.com:443/api/v1/entries/{_lang}/{_word}";

            try
            {
                var webRequest = System.Net.WebRequest.Create(_url);

                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";

                    webRequest.Headers.Add("app_id", _appId);
                    webRequest.Headers.Add("app_key", _appKey);

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            GetDefinitions(jsonResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("No definition for this word, sorry.");
            }
        }

        private void GetDefinitions(string data)
        {
            dynamic dynJson = JsonConvert.DeserializeObject(data);

            foreach (var obj in dynJson.results)
            {
                foreach (var obj2 in obj.lexicalEntries)
                {
                    foreach (var obj3 in obj2.entries)
                    {
                        foreach (var def in obj3.senses)
                        {
                            Console.WriteLine(def.definitions[0]);
                            Console.WriteLine("");
                        }
                    }
                }
            }
        }
    }
}
 