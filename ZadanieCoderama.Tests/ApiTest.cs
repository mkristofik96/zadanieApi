using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;

namespace ZadanieCoderama.Tests
{
    [TestClass]
    public class ApiTest
    {
        private readonly HttpClient httpClient;
        public ApiTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            httpClient = webAppFactory.CreateDefaultClient();

        }
        [TestMethod]
        public async Task TestXmlToJson()
        {
            var inputXML = HttpUtility.UrlEncode(System.IO.File.ReadAllText(@"..\..\..\xmlExample.txt"));
            var outputJson = System.IO.File.ReadAllText(@"..\..\..\jsonExample.txt");
            var response = await httpClient.GetAsync($"/api/Converter/ConvertFromUrl?urlFile={inputXML}&type=xml");
            var stringResult = await response.Content.ReadAsStringAsync();


            

            var str = Regex.Replace(outputJson, @"\s", "");
            var str2 = Regex.Replace(stringResult, @"\s", "");


            Assert.AreEqual(str, str2);

        }

        [TestMethod]
        public async Task TestJsonToXml()
        {
            var inputJson = System.IO.File.ReadAllText(@"..\..\..\jsonExample.txt");
            var outputXml = System.IO.File.ReadAllText(@"..\..\..\xmlExample.txt");
            var response = await httpClient.GetAsync($"/api/Converter/ConvertFromUrl?urlFile={inputJson}&type=json");
            var stringResult = await response.Content.ReadAsStringAsync();

            var str = Regex.Replace(outputXml, @"\s", "");
            var str2 = Regex.Replace(stringResult, @"\s", "");

            Assert.AreEqual(str, str2);

        }
    }
}