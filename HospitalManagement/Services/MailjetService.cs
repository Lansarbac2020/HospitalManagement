using Mailjet.Client;
using Newtonsoft.Json.Linq;
using Mailjet.Client.Resources;
using System.Threading.Tasks;

namespace HospitalManagement.Services
{
    public class MailjetService
    {
        private readonly string _apiKey;
        private readonly string _secretKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public MailjetService(IConfiguration configuration)
        {
            var mailjetSettings = configuration.GetSection("MailjetSettings");
            _apiKey = mailjetSettings["APIKey"];
            _secretKey = mailjetSettings["SecretKey"];
            _senderEmail = mailjetSettings["SenderEmail"];
            _senderName = mailjetSettings["SenderName"];
        }

        public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string htmlContent)
        {
            var client = new MailjetClient(_apiKey, _secretKey);

            var request = new MailjetRequest
            {
                Resource = Send.Resource
            }
            .Property(Send.FromEmail, _senderEmail)
            .Property(Send.FromName, _senderName)
            .Property(Send.Subject, subject)
            .Property(Send.HtmlPart, htmlContent)
            .Property(Send.Recipients, new JArray
            {
            new JObject
            {
                { "Email", recipientEmail }
            }
            });

            var response = await client.PostAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
