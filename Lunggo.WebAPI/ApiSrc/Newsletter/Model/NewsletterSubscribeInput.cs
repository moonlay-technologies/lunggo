using System.ComponentModel.DataAnnotations;

namespace Lunggo.WebAPI.ApiSrc.Newsletter.Model
{
    public class NewsletterSubscribeInput
    {
        [Required]
        [EmailAddress]
        public string Address { get; set; }
        public string Name { get; set; }
    }
}