using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class NewsService(HttpClient client)
    {
        private readonly string url = "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml";

        public async Task<List<NewsDto>> GetAllAsync()
        {
            var response = await client.GetStringAsync(url);
            response = response.Replace("media:content", "image");

            var doc = XDocument.Parse(response);
            return doc.Descendants("item")
                .Select(item => new NewsDto()
                {
                    Title = (string)item.Element("title"),
                    Image = (string)item.Element("image")?.Attribute("url"),
                    Description = (string)item.Element("description"),
                    PublicationDate = DateTime.Parse((string)item.Element("pubDate"))
                }).ToList();

        }
    }
}
