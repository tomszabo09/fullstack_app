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
    [Table("Party members")]
    public class PartyMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberID {get; set;}

        [ForeignKey(nameof(Party))]
        public int PartyID { get; set; }

        [Range(18,int.MaxValue)]
        public int Age { get; set; }

        [MaxLength(20)]
        [Required]
        public string LastName { get; set; }
        
        [NotMapped]
        [JsonIgnore]
        public virtual Party Party { get; set; }
    }
}
