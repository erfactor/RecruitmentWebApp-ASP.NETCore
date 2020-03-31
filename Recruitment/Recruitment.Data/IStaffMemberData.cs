using System.Collections.Generic;
using Recruitment.Core;

namespace Recruitment.Data
{
    public interface IStaffMemberData
    {
        UserType CheckUserType(string identifier);
        IEnumerable<HrMember> GetAllHr();
        void AddHr(HrMember member);
        void DeleteHr(int hrId);
        int Commit();
    }
}