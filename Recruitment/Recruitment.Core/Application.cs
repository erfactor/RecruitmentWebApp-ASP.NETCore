using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Recruitment.Core
{
    public class Application
    {
        public Application()
        {
            
        }
        public Application(int applicationId, int jobOfferId, string offerName, string name, string communicationEmail, string userEmail, string phone, string info, string cvFile)
        {
            ApplicationId = applicationId;
            JobOfferId = jobOfferId;
            OfferName = offerName;
            Name = name;
            CommunicationEmail = communicationEmail;
            UserEmail = userEmail;
            Phone = phone;
            Info = info;
            CvFile = cvFile;
        }

        public int ApplicationId { get; set; }
        
        public int JobOfferId { get; set; }
        [ForeignKey("JobOfferId")]
        public JobOffer JobOffer { get; set; }
        
        [EmailAddress]
        public string UserEmail { get; set; }
        public string OfferName { get; set; }
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string CommunicationEmail { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public string Info { get; set; }
        public string CvFile { get; set; }
        public string State { get; set; } = "Idle";

//        [NotMapped]
//        public IFormFile RealFile { get; set; }
    }
}