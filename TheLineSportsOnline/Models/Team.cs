using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheLineSportsOnline.Models
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Display(Name ="Team")]
        public string Name { get; set; }

        [Display(Name = "Conference")]
        public byte ConferenceId { get; set; }
        [Display(Name = "Conference")]
        public Conference Conference { get; set; }
    }
}