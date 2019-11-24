using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Translator
{
    public class YandexTranslator
    {
        public string Translate(string s, string lang)
        {
            if (s.Length > 0)
            {
                WebRequest request = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?"
                    + "key=trnsl.1.1.20190816T102227Z.aad0b17cc5d38426.363c6c1bfc7a774b5b138ee6aa317ddceab2ad88"
                    + "&text=" + s
                    + "&lang=" + lang);

                WebResponse response = request.GetResponse();

                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    string line;

                    if ((line = stream.ReadLine()) != null)
                    {
                        Translation translation = JsonConvert.DeserializeObject<Translation>(line);

                        s = "";

                        foreach (string str in translation.text)
                        {
                            s += str;
                        }
                    }
                }

                return s;
            }
            else
                return "";
        }
    }
}