using System.ComponentModel.DataAnnotations.Schema;

namespace TheLineSportsOnline.Models
{
    public class Conference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
    }
}