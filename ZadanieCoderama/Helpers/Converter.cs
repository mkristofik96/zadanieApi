using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace ZadanieCoderama.Helpers
{
    public static class Converter
    {
        private static readonly XDeclaration _defaultDeclaration = new("1.0", null, null);
        public static string XmlToJson(string xml)
        {

            xml = xml.Replace("%2F", "/");
            var doc = XDocument.Parse(xml);
            doc.Declaration = null;
            string jsonText = JsonConvert.SerializeXNode(doc, Formatting.Indented);
            return jsonText;
        }

        public static string JsonToXml(string json)
        {
            XDocument doc = new XDocument();             
            var token = JToken.Parse(json);

            if (token is JArray)
            {
                doc = JsonConvert.DeserializeXNode("{\"object\":" + json + "}", "object")!;
            }
            else if (token is JObject)
            {
                doc = JsonConvert.DeserializeXNode(json);
            }
            var declaration = doc.Declaration ?? _defaultDeclaration;
            return $"{declaration}{Environment.NewLine}{doc}";
        }
    }
}
