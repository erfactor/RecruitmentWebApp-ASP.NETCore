using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Recruitment.Core;

namespace Recruitment.Data
{
    public class SqlApplicationData : IApplicationData
    {
        private readonly RecruitmentDbContext _db;

        public SqlApplicationData(RecruitmentDbContext db)
        {
            _db = db;
        }
        
        public void Add(Application application)
        {
            _db.Applications.Add(application);
        }

        public void Update(Application updatedApplication)
        {
            var entity = _db.Applications.Attach(updatedApplication);
            entity.State = EntityState.Modified;
        }

        public IEnumerable<Application> GetAll()
        {
            return _db.Applications;
        }

        public IEnumerable<Application> GetUserApplications(string email)
        {
            return _db.Applications.Where(a => a.UserEmail == email);
        }

        public IEnumerable<Application> GetHrMemberApplications(string email)
        {
            var offers = _db.JobOffers.Where(o => o.HrEmail == email).Select(o => o.JobOfferId);
            return _db.Applications.Where(a => offers.Any(o => o == a.JobOfferId));
        }

        public Application FindById(int id)
        {
            return _db.Applications.Find(id);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }
    }
}