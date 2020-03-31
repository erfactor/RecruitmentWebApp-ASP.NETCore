using System.Collections.Generic;
using System.Linq;
using Recruitment.Core;

namespace Recruitment.Data
{
    public class SqlStaffMemberData : IStaffMemberData
    {
        private readonly RecruitmentDbContext _db;

        public SqlStaffMemberData(RecruitmentDbContext db)
        {
            _db = db;
        }
        public UserType CheckUserType(string identifier)
        {
            if (_db.Admins.Any(a => a.AdNameIdentifier == identifier))
                return UserType.Admin;
            if (_db.HrMembers.Any(h => h.Email == identifier))
                return UserType.Hr;
            
            return UserType.User;
        }

        public IEnumerable<HrMember> GetAllHr()
        {
            return _db.HrMembers;
        }

        public void AddHr(HrMember member)
        {
            _db.HrMembers.Add(member);
        }

        public void DeleteHr(int hrId)
        {
            var hrMember = _db.HrMembers.Find(hrId);
            if (hrMember != null)
            {
                _db.Remove(hrMember);
            }
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }
    }
}