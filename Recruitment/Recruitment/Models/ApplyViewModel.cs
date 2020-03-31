using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Recruitment.Core;

namespace Recruitment.Models
{
    public class ApplyViewModel
    {
        public ApplyViewModel() {}

        public int JobOfferId { get; set; }
        public int ApplicationId { get; set; }
        public string OfferName { get; set; }
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string CommunicationEmail { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Info { get; set; }
        public IFormFile RealFile { get; set; }
        public string CvFile { get; set; }
    }
}