using Recruitment.Data;

namespace Recruitment.Utils
{
    public static class UserTypeManager
    {
        public static readonly IStaffMemberData StaffData = ServiceLocator.Current.GetInstance<IStaffMemberData>();
    }
}