using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PokedexExplorer.Model
{
    [Index(nameof(Pokemon.ID), IsUnique = true, Name = "IndexPokemonID")]
    [Index(nameof(Pokemon.Name), IsUnique = true, Name = "IndexPokemonName")]
    [Index(nameof(Pokemon.Height), IsUnique = true, Name = "IndexPokemonHeight")]
    [Index(nameof(Pokemon.Weight), IsUnique = true, Name = "IndexPokemonWeight")]
    [Index(nameof(Pokemon.HP), IsUnique = true, Name = "IndexPokemonHp")]
    [Index(nameof(Pokemon.Attack), IsUnique = true, Name = "IndexPokemonAttack")]
    [Index(nameof(Pokemon.Defense), IsUnique = true, Name = "IndexPokemonDefense")]
    [Index(nameof(Pokemon.SpecialAttack), IsUnique = true, Name = "IndexPokemonSpecialAttack")]
    [Index(nameof(Pokemon.SpecialDefense), IsUnique = true, Name = "IndexPokemonSpecialDefense")]
    [Index(nameof(Pokemon.Speed), IsUnique = true, Name = "IndexPokemonSpeed")]
    public class Pokemon
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public int BaseExperience { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public int Order { get; set; }
        [ForeignKey("Ability")]
        public int? PrimaryAbility { get; set; }
        [ForeignKey("Ability")]
        public int? SecondaryAbility { get; set; }
        [ForeignKey("Ability")]
        public int? HiddenAbility { get; set; }
        [ForeignKey("PokemonSpecies")]
        [Required]
        public int Species { get; set; }
        [Required]
        public int HP { get; set; }
        [Required]
        public int HPEffort { get; set; }
        [Required]
        public int Attack { get; set; }
        [Required]
        public int AttackEffort { get; set; }
        [Required]
        public int Defense { get; set; }
        [Required]
        public int DefenseEffort { get; set; }
        [Required]
        public int SpecialAttack { get; set; }
        [Required]
        public int SpecialAttackEffort { get; set; }
        [Required]
        public int SpecialDefense { get; set; }
        [Required]
        public int SpecialDefenseEffort { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int SpeedEffort { get; set; }
        [Required]
        public string SpriteFrontDefault { get; set; }
        public string? SpriteFrontFemale { get; set; }
        public string? SpriteFrontShinyFemale { get; set; }
        public string? SpriteFrontShiny { get; set; }
        public string? SpriteBackDefault { get; set; }
        public string? SpriteBackFemale { get; set; }
        public string? SpriteBackShinyFemale { get; set; }
        public string? SpriteBackShiny { get; set; }
        public string? Cry { get; set; }
        public string? CryLegacy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PrimaryType { get; set; }
        public string? SecondaryType { get; set; }

        public Pokemon(int iD, int baseExperience, int height, int weight, int order, int species, int hP, int hPEffort, int attack, int attackEffort, int defense, int defenseEffort, int specialAttack, int specialAttackEffort, int specialDefense, int specialDefenseEffort, int speed, int speedEffort, string spriteFrontDefault, string name, string primaryType)
        {
            ID = iD;
            BaseExperience = baseExperience;
            Height = height;
            Weight = weight;
            Order = order;
            Species = species;
            HP = hP;
            HPEffort = hPEffort;
            Attack = attack;
            AttackEffort = attackEffort;
            Defense = defense;
            DefenseEffort = defenseEffort;
            SpecialAttack = specialAttack;
            SpecialAttackEffort = specialAttackEffort;
            SpecialDefense = specialDefense;
            SpecialDefenseEffort = specialDefenseEffort;
            Speed = speed;
            SpeedEffort = speedEffort;
            SpriteFrontDefault = spriteFrontDefault;
            Name = name;
            PrimaryType = primaryType;
        }
    }
}
