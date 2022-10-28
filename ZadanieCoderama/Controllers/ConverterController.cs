using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using ZadanieCoderama.Helpers;
using Formatting = Newtonsoft.Json.Formatting;
using System;
using Microsoft.Net.Http.Headers;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.StaticFiles;

namespace ZadanieCoderama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private readonly ILogger<ConverterController> logger;
        public ConverterController(ILogger<ConverterController> logger)
        {
            this.logger = logger;
        }


        [HttpGet("ConvertFromUrl")]
        public async Task<IActionResult> ConvertFromUrl(string urlFile, string type, string? receiver)
        {
            try
            {
                if(string.IsNullOrEmpty(urlFile))
                {
                    return BadRequest();
                }
                string fileContents = urlFile;
                string result = Conversion(type, fileContents, receiver);
                if (type.Contains("xml"))
                {                   
                    return Content(result, "text/json");
                }
                else if (type.Contains("json"))
                {                   
                    return Content(result, "text/xml");
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Incoret format");
                return StatusCode(500, ex);
            }

        }


        [HttpPost("ConvertFromDisk")]
        public async Task<IActionResult> ConvertFromDisk(IFormFile file,string type,string? receiver)
        {
            try
            {
                if (file == null || file.Length == 0) return BadRequest();
                string fileContents;
                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    fileContents = await reader.ReadToEndAsync();
                }

                string result = Conversion(type ,fileContents,receiver); 

                if(type.Contains("xml"))
                {
                    return Content(result, "text/json");
                }
                else if(type.Contains("json"))
                {                  
                    return Content(result, "text/xml");
                }
                            
                return BadRequest();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Incoret format");
                return StatusCode(500,ex);
            }          
        }

        private string Conversion(string type,string fileContents,string receiver)
        {
            string result = " ";
            if (type.Contains("xml"))
            {
                result = Converter.XmlToJson(fileContents);
                if (!string.IsNullOrEmpty(receiver))
                {
                    EmailClient.SendMail(result, receiver);
                }
                return result;

            }
            else if (type.Contains("json"))
            {
                result = Converter.JsonToXml(fileContents);
                if (!string.IsNullOrEmpty(receiver))
                {
                    EmailClient.SendMail(result, receiver);
                }
                return result;
            }
            return result;
        }
    }
}
