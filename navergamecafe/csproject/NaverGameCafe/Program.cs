using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NaverGameCafe
{
    class Program
    {
        static void Main(string[] args)
        {            
            string html = "";
            for (int page = 1; page <= 10; page++)
            {                
                string url = "http://section.cafe.naver.com/GameCafeList.nhn?search.sortType=rank&search.page=" + page;
                
                WebClient wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                html += wc.DownloadString(url);
            }

            string filepath = DateTime.Now.ToString("yyyyMMddHHmmss") + ".htm";
            File.WriteAllText(filepath, html);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string xPath_li = "/html[*]/body[1]/div[2]/div[2]/div[1]/div[1]/div[3]/div[2]/ul[1]/li[*]";
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xPath_li);
            Console.WriteLine("items : " + nodes.Count);

            foreach (HtmlNode node in nodes)
            {
                HtmlNode nodeCafeLink = node.SelectSingleNode("./div[2]/div[1]/h4[1]/a[1]");
                string link = nodeCafeLink.Attributes["href"].Value;
                string cafeid = Path.GetFileName(link);
                string member = node.SelectSingleNode("./div[2]/div[3]/span[2]").InnerHtml;
                string post = node.SelectSingleNode("./div[2]/div[3]/span[5]").InnerHtml;

                Console.WriteLine(member.PadLeft(10) + post.PadLeft(10) + " " + nodeCafeLink.InnerHtml);
            }
        }
    }
}
