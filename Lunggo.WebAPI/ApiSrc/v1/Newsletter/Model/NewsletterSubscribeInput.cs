using System.ComponentModel.DataAnnotations;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter.Model
{
    public class NewsletterSubscribeInput
    {
        [Required]
        [EmailAddress]
        public string Address { get; set; }
        public string Name { get; set; }
    }
}