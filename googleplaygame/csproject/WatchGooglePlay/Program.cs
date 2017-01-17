using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WatchGooglePlay
{
    class Program
    {
        static void Main(string[] args)
        {
            int elementPerPage = 90;

            string html = "";

            for (int i = 0; i < 6; i++)
            {
                int start = i * elementPerPage;
                string url = "https://play.google.com/store/apps/category/GAME/collection/topgrossing?start=" + start + "&num=" + elementPerPage;
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                html += wc.DownloadString(url);
            }

            string filepath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".htm";
            File.WriteAllText(filepath, html);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string xPath_div = "/html/body[1]/div[5]/div[6]/div[1]/div[1]/div[1]/div[1]/div[1]/div[1]/div/div[*]";

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath_div);
            Console.WriteLine("items : " + nodes.Count);

            foreach (HtmlNode node in nodes)
            {
                HtmlNode nodeTitle = node.SelectSingleNode("./div[1]/div[2]/a[2]");
                HtmlNode nodeCompany = node.SelectSingleNode("./div[1]/div[2]/div[1]/a[1]");

                string bundleName = node.Attributes["data-docid"].Value;
                string title = nodeTitle.Attributes["title"].Value;
                string company = nodeCompany.Attributes["title"].Value;
                string rankAndTitle = nodeTitle.InnerText;
                string rank = "";
                int pos = rankAndTitle.IndexOf(".");
                if (pos != -1) rank = rankAndTitle.Substring(0, pos).Trim();
                //Console.WriteLine(rankAndTitle);
                Console.WriteLine(rank + ",\"" + title + "\",\"" + company + "\",\"" + bundleName+"\"");
            }
        }
    }
}
