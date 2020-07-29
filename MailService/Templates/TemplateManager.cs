using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;

namespace MailService.Templates
{
    public class TemplateManager
    {
        public static BodyBuilder CreateBodyFromTemplate(string TemplatePath, Dictionary<string, string> parameters)
        {
            BodyBuilder template = new BodyBuilder();

            using (StreamReader SourceReader = File.OpenText(TemplatePath))
            {
                string rawBody = SourceReader.ReadToEnd();
                foreach (var param in parameters)
                {
                    rawBody = rawBody.Replace("{" + param.Key + "}", param.Value);
                }
                template.HtmlBody = rawBody;
            }
            return template;
        }
    }
}
