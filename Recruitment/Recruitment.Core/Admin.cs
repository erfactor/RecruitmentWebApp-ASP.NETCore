using System.ComponentModel.DataAnnotations;

namespace Recruitment.Core
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string AdNameIdentifier { get; set; }
    }
}