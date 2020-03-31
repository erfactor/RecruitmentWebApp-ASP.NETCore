using System.ComponentModel.DataAnnotations;

namespace Recruitment.Core
{
    public class JobOffer
    {
        public int JobOfferId { get; set; }
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public string HrEmail { get; set; }

        public JobOffer()
        {
            
        }
        public JobOffer(int jobOfferId, string name, string description)
        {
            JobOfferId = jobOfferId;
            Name = name;
            Description = description;
        }
    }
}