using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BV3N92_HFT_2021221.Models
{
    public enum Ideologies
    {
        Socialist, Conservative, Nationalist
    }

    [Table("Parties")]
    public class Party
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartyID { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string PartyName { get; set; }

        [ForeignKey(nameof(Parliament))]
        public int ParliamentID { get; set; }

        [MaxLength(20)]
        [Required]
        public string Ideology { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<PartyMember> PartyMembers { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual Parliament Parliament { get; set; }
    }
}
