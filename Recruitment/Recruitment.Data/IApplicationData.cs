using System.Collections.Generic;
using Recruitment.Core;

namespace Recruitment.Data
{
    public interface IApplicationData
    {
        void Add(Application application);
        void Update(Application updatedApplication);
        IEnumerable<Application> GetAll();
        IEnumerable<Application> GetUserApplications(string email);
        IEnumerable<Application> GetHrMemberApplications(string email);
        Application FindById(int id);
        
        int Commit();
    }
}