using System.Collections.Generic;

namespace Recruitment.Core
{
    public class ApplicationsViewModel
    {
        public IEnumerable<Application> Applications { get; set; }
        public UserType UserType { get; set; }
    }
}