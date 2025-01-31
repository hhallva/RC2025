using DataLayer.RSS;
using System.Xml.Linq;

namespace DataLayer.Services
{
    public class RssService(HttpClient httpClient)
    {
        private readonly string url = "https://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml"; // для примера ссылка на RSS который будет дан на чемп

        public async Task<List<RssItem>> GetRssItemsAsync()
        {
            var response = await httpClient.GetStringAsync(url);
            response = response.Replace("media:content", "image"); // media:content - ошибка при парсинге

            var doc = XDocument.Parse(response);
            return doc.Descendants("item") // получение всех узлов с названием item
                .Select(item => new RssItem()
                {
                    Title = (string)item.Element("title"),
                    Image = (string)item.Element("image")?.Attribute("url"),
                    Description = (string)item.Element("description"),
                    PublicationDate = DateTime.Parse((string)item.Element("pubDate"))
                }).ToList();
        }
    }

}
