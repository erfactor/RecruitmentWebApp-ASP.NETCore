using System.Collections.Generic;
using Recruitment.Core;

namespace Recruitment.Data
{
    public class InMemoryJobOfferData //: IJobOfferData
    {
        private List<JobOffer> _jobOffers;

        public InMemoryJobOfferData()
        {
            _jobOffers = new List<JobOffer>()
            {
                new JobOffer(1, "Programmer", "Low-level c-programming"),
                new JobOffer(2, "Graphic Designer", "Creating graphics for android games")
            };
        }
        public IEnumerable<JobOffer> GetAll()
        {
            return _jobOffers;
        }
    }
}