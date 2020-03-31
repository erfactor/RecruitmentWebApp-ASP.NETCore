using System.Collections.Generic;
using System.Linq;
using Recruitment.Core;

namespace Recruitment.Data
{
    public class SqlJobOfferData : IJobOfferData
    {
        private readonly RecruitmentDbContext _db;

        public SqlJobOfferData(RecruitmentDbContext db)
        {
            _db = db;
        }
        public IEnumerable<JobOffer> GetAll()
        {
            return _db.JobOffers;
        }

        public JobOffer FindById(int id)
        {
            return _db.JobOffers.FirstOrDefault(o => o.JobOfferId == id);
        }

        public void Create(JobOffer jobOffer)
        {
            _db.JobOffers.Add(jobOffer);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var offer = _db.JobOffers.Find(id);
            if (offer != null)
            {
                _db.Remove(offer);
            }
        }

        public string GetHrEmail(int jobOfferId)
        {
            var hrEmail = _db.JobOffers.Where(o => o.JobOfferId == jobOfferId).Select(o => o.HrEmail).FirstOrDefault();
            return hrEmail;
        }

        public bool CheckIfEmailIsTaken(string email, int jobOfferId)
        {
            return _db.Applications.Where(a => a.JobOfferId == jobOfferId).Any(a => a.CommunicationEmail == email);
        }

        public Dictionary<int, int> GetNumOfApplicants()
        {
            var result = new Dictionary<int,int>();
            var jobOffers = GetAll();
            foreach (var jobOffer in jobOffers)
            {
                int numOfApplications = _db.Applications.Count(a => a.JobOfferId == jobOffer.JobOfferId);
                result.Add(jobOffer.JobOfferId,numOfApplications);
            }

            return result;
        }
    }
}