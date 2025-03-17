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
    [Index(nameof(PokemonSpecies.Name), IsUnique = true, Name = "IndexPokemonName")]
    [Index(nameof(PokemonSpecies.Generation), IsUnique = true, Name = "IndexPokemonSpeciesGeneration")]
    public class PokemonSpecies
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public int BaseHappiness { get; set; }
        [Required]
        public int CaptureRate { get; set; }
        [Required]
        public int GenderRate { get; set; }
        public int? HatchCounter { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int Generation { get; set; }
        [Required]
        public int? NationalPokedexNumber { get; set; }
        [Required]
        public bool IsBaby { get; set; }
        [Required]
        public bool IsLegendary { get; set; }
        [Required]
        public bool IsMythical { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string GrowthRate { get; set; }
        [Required]
        public string Habitat { get; set; }
        [Required]
        public string Shape { get; set; }
        [Required]
        public string Genera { get; set; }
        [Required]
        public string Name { get; set; }
        public string? EggGroups { get; set; }
        public string? Varieties { get; set; }
        public string? Description { get; set; }

        public PokemonSpecies(int iD, int baseHappiness, int captureRate, int genderRate, int order, int generation, int nationalPokedexNumber, bool isBaby, bool isLegendary, bool isMythical, string color, string growthRate, string habitat, string shape, string genera, string name)
        {
            ID = iD;
            BaseHappiness = baseHappiness;
            CaptureRate = captureRate;
            GenderRate = genderRate;
            Order = order;
            Generation = generation;
            NationalPokedexNumber = nationalPokedexNumber;
            IsBaby = isBaby;
            IsLegendary = isLegendary;
            IsMythical = isMythical;
            Color = color;
            GrowthRate = growthRate;
            Habitat = habitat;
            Shape = shape;
            Genera = genera;
            Name = name;
        }
    }
}
