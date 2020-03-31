using System.ComponentModel.DataAnnotations;

namespace Recruitment.Core
{
    public class HrMember
    {
        public HrMember()
        {
        }

        public HrMember(string name, string surname, string email)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [EmailAddress]
        [Required] public string Email { get; set; }
    }
}