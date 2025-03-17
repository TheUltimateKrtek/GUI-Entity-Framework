using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace PokedexExplorer.Model
    {
        public class EvolutionChain
        {
            [Key]
            [Required]
            public int ID { get; set; }
            [ForeignKey("Pokemon")]
            [Required]
            public int EvolvesFrom { get; set; }
            [ForeignKey("Pokemon")]
            [Required]
            public int EvolvesTo { get; set; }
            public int? Gender { get; set; }
            public int? MinBeauty { get; set; }
            public int? MinHappiness { get; set; }
            public int? MinLevel { get; set; }
            [ForeignKey("Pokemon")]
            public int? TradeSpecies { get; set; }
            public int? RelativePhysicalStats { get; set; }
            public string? Item { get; set; }
            public string? HeldItem { get; set; }
            [ForeignKey("Move")]
            public int? KnownMove { get; set; }
            public string? KnownMoveType { get; set; }
            public string? Trigger { get; set; }
            [ForeignKey("Pokemon")]
            public int? PartySpecies { get; set; }
            public string? PartyType { get; set; }
            public string? TimeOfDay { get; set; }
            public bool? NeedsOverworldRain { get; set; }
            public bool? TurnUpsideDown { get; set; }
            
            public EvolutionChain(int id, int evolvesFrom, int evolvesTo)
            {
                this.ID = id;
                this.EvolvesFrom = evolvesFrom;
                this.EvolvesTo = evolvesTo;
            }
        }
    }
