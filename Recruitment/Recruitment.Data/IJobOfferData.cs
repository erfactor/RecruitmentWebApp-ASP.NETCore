using System.Collections.Generic;
using Recruitment.Core;

namespace Recruitment.Data
{
    public interface IJobOfferData
    {
        IEnumerable<JobOffer> GetAll();
        JobOffer FindById(int id);
        void Create(JobOffer jobOffer);
        int Commit();
        void Delete(int id);
        string GetHrEmail(int jobOfferId);
        bool CheckIfEmailIsTaken(string email, int jobOfferId);
        Dictionary<int, int> GetNumOfApplicants();
    }
}