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
    [Index(nameof(PokemonMove.Pokemon), IsUnique = false, Name = "IndexPokemonMovePokemon")]
    [Index(nameof(PokemonMove.Move), IsUnique = false, Name = "IndexPokemonMoveMove")]
    public class PokemonMove
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public int Pokemon { get; set; }
        [Required]
        public int Move { get; set; }
        public int? LevelLearnedAt { get; set; }
        public string? LearnMethod { get; set; }
    }
}
