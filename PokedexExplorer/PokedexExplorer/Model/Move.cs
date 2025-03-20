using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokedexExplorer.Model
{
    [Index(nameof(Move.Name), IsUnique = false, Name = "IndexMoveName")]
    public class Move
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Accuracy { get; set; }
        public string? DamageClass { get; set; }
        public int? EfectChance { get; set; }
        public int? Generation { get; set; }
        public string? Ailment { get; set; }
        public int? AilmentChance { get; set; }
        public int? CritRate { get; set; }
        public int? Drain { get; set; }
        public int? FlinchChance { get; set; }
        public int? Healing { get; set; }
        public int? MaxHits { get; set; }
        public int? MaxTurns { get; set; }
        public int? MinHits { get; set; }
        public int? MinTurns { get; set; }
        public int? StatChance { get; set; }
        public int? Power { get; set; }
        [Required]
        public int PP { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public string Target { get; set; }
        [Required]
        public string Type { get; set; }
        public string? Description { get; set; }
    }
}
