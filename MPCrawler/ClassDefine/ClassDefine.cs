using System.Collections.Generic;
using System.Net.Http;

namespace MpCrawler
{
    public class HtmlText
    {
        // define a class to store url and htmlText of that url content

        public string url;
        public string htmlText;

        public HtmlText(string url)
        {
            // Function:
            //          get html text
            // Todo:
            //          how do we determine is url is responding and content is valid
            this.url = url;
            var httpClient = new HttpClient();
            var html = httpClient.GetStringAsync(url);
            htmlText = html.Result;
        }

        public HtmlText()
        {
        }
    }
}
