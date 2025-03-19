using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Printing.IndexedProperties;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PokedexExplorer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace PokedexExplorer.Data
{
    public class PokeAPIFetcher
    {
        static public JObject RetrieveJSON(string name, int? id = null)
        {
            string url = "https://pokeapi.co/api/v2/" + name + "/";
            if (id != null) url += id + "/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;

                    return JObject.Parse(jsonResponse);
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        static public int GetCount(string name)
        {
            JObject json = RetrieveJSON(name);
            if (json == null) return -1;
            return json["count"].ToObject<int>();
        }

        static public Ability ParseAbility(JObject node)
        {
            if (node == null) return null;
            string at = "";
            try
            {
                int id = node["id"]?.ToObject<int>() ?? - 1;
                int generation = GetURLIntValue(node["generation"]["url"].ToString()) ?? 0;
                
                at = "effect_entries: " + node["effect_entries"];
                JObject effectNode = GetEnglishNode(node["effect_entries"]?.ToObject<JArray>() ?? null);
                string effect = "No effect description.";
                string shortEffect = "No short effect description.";
                if (effectNode != null)
                {
                    effect = effectNode["effect"]?.ToString() ?? "No effect description.";
                    shortEffect = effectNode["short_effect"]?.ToString() ?? "No short effect description.";
                }

                at = "flavor_text_entries: " + node["flavor_text_entries"];
                JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
                string description = "No description.";
                if (descriptionNode != null)
                {
                    description = descriptionNode["flavor_text"].ToObject<string>();
                }

                at = "names: " + node["names"];
                JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
                string name = node["name"]?.ToObject<string>() ?? "<unknown>";
                if (nameNode != null)
                {
                    name = nameNode["name"].ToObject<string>();
                }

                Ability ability = new Ability();
                ability.ID = id;
                ability.Name = name;
                ability.Generation = generation;
                ability.Effect = effect;
                ability.ShortEffect = shortEffect;
                ability.Description = description;
                return ability;
            }
            catch (Exception e)
            {
                throw new Exception(at, e);
                return null;
            }
        }
        static public Move ParseMove(JObject node)
        {
            if (node == null) return null;

            int? accuracy = node["accuracy"]?.ToObject<int?>() ?? null;
            string? damageClass = node["damage_class"]["name"].ToObject<string?>();
            int? effectChance = node["effectChance"]?.ToObject<int?>() ?? null;
            int? generation = GetURLIntValue(node["generation"]["url"]?.ToObject<string?>() ?? null);
            int id = node["id"]?.ToObject<int>() ?? -1;

            string? ailment = node["meta"] == null ? null : (node["meta"]["ailment"] == null ? null : node["meta"]["ailment"]["name"].ToObject<string>());
            int? ailmentChance = node["meta"] == null ? null : node["meta"]["ailment_chance"].ToObject<int?>();
            int? critRate = node["meta"] == null ? null : node["meta"]["crit_rate"].ToObject<int?>();
            int? drain = node["meta"] == null ? null : node["meta"]["drain"].ToObject<int?>();
            int? flinchChance = node["meta"] == null ? null : node["meta"]["flinch_chance"].ToObject<int?>();
            int? healing = node["meta"] == null ? null : node["meta"]["healing"].ToObject<int?>();
            int? maxHits = node["meta"] == null ? null : node["meta"]["max_hits"].ToObject<int?>();
            int? maxTurns = node["meta"] == null ? null : node["meta"]["max_turns"].ToObject<int?>();
            int? minHits = node["meta"] == null ? null : node["meta"]["min_hits"].ToObject<int?>();
            int? minTurns = node["meta"] == null ? null : node["meta"]["min_turns"].ToObject<int?>();
            int? statChance = node["meta"] == null ? null : node["meta"]["stat_chance"].ToObject<int?>();

            JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
            string name = node["name"]?.ToObject<string>() ?? "<unknown>";
            if (nameNode != null)
            {
                name = nameNode["name"]?.ToObject<string>() ?? "<unknown>";
            }

            int? power = node["power"]?.ToObject<int?>() ?? null;
            int pp = node["pp"]?.ToObject<int>() ?? -1;
            int priority = node["priority"]?.ToObject<int>() ?? -1;
            string target = node["target"]?.ToObject<string>() ?? null;
            string type = node["type"]?.ToObject<string>() ?? "normal";

            JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
            string description = null;
            if (descriptionNode != null)
            {
                description = descriptionNode["flavor_text"]?.ToObject<string>() ?? null;
            }

            Move move = new Move();
            move.ID = id;
            move.Name = name;
            move.PP = pp;
            move.Priority = priority;
            move.Target = target;
            move.Type = type;
            move.Accuracy = accuracy;
            move.DamageClass = damageClass;
            move.EfectChance = effectChance;
            move.Generation = generation;
            move.Ailment = ailment;
            move.AilmentChance = ailmentChance;
            move.CritRate = critRate;
            move.Drain = drain;
            move.FlinchChance = flinchChance;
            move.Healing = healing;
            move.MaxHits = maxHits;
            move.MaxTurns = maxTurns;
            move.MinHits = minHits;
            move.MinTurns = minTurns;
            move.StatChance = statChance;
            move.Power = power;
            move.Description = description;
            return move;
        }
        static public Pokemon ParsePokemon(JObject node)
        {
            if (node == null) return null;

            int?[] abilities = new int?[] { null, null, null };
            if(node["abilities"] != null) foreach (JObject a in node["abilities"].ToObject<JArray>())
            {
                int? value = GetURLIntValue(a["ability"]["url"]?.ToObject<string>() ?? null);
                int index = a["slot"].ToObject<int>();
                abilities[index] = value;
            }
            int? primaryAbility = abilities[0];
            int? secondaryAbility = abilities[1];
            int? hiddenAbility = abilities[2];

            int baseExperience = node["base_experience"].ToObject<int>();
            int height = node["height"].ToObject<int>();
            int weight = node["weight"].ToObject<int>();
            int id = node["id"].ToObject<int>();
            int order = node["order"].ToObject<int>();
            string name = node["name"].ToObject<string>();

            string spriteFrontDefault = node["sprite_front_default"].ToObject<string>();
            string? spriteFrontFemale = node["sprite_front_female"].ToObject<string?>();
            string? spriteFrontShiny = node["sprite_front_shiny"].ToObject<string?>();
            string? spriteFrontShinyFemale = node["sprite_front_shiny_female"].ToObject<string?>();
            string? spriteBackDefault = node["sprite_back_default"].ToObject<string?>();
            string? spriteBackFemale = node["sprite_back_female"].ToObject<string?>();
            string? spriteBackShiny = node["sprite_back_shiny"].ToObject<string?>();
            string? spriteBackShinyFemale = node["sprite_back_shiny_female"].ToObject<string?>();

            int species = node["species"].ToObject<int>();

            string? cry = node["cry"].ToObject<string?>();
            string? cryLegacy = node["cry"].ToObject<string?>();

            int hp = node["stats"][0]["base_stat"].ToObject<int>();
            int hpEffort = node["stats"][0]["effort"].ToObject<int>();
            int attack = node["stats"][1]["base_stat"].ToObject<int>();
            int attackEffort = node["stats"][1]["effort"].ToObject<int>();
            int defense = node["stats"][2]["base_stat"].ToObject<int>();
            int defenseEffort = node["stats"][2]["effort"].ToObject<int>();
            int specialAttack = node["stats"][3]["base_stat"].ToObject<int>();
            int specialAttackEffort = node["stats"][3]["effort"].ToObject<int>();
            int specialDefense = node["stats"][4]["base_stat"].ToObject<int>();
            int specialDefenseEffort = node["stats"][4]["effort"].ToObject<int>();
            int speed = node["stats"][5]["base_stat"].ToObject<int>();
            int speedEffort = node["stats"][5]["effort"].ToObject<int>();

            string primaryType = node["types"][0].ToObject<string>();
            string? secondaryType = node["types"].ToObject<JArray>().Count == 1 ? null : node["types"][1].ToObject<string?>();

            Pokemon pokemon = new Pokemon();
            pokemon.ID = id;
            pokemon.Name = name;
            pokemon.BaseExperience = baseExperience;
            pokemon.Height = height;
            pokemon.Weight = weight;
            pokemon.Order = order;
            pokemon.Species = species;
            pokemon.HP = hp;
            pokemon.Attack = attack;
            pokemon.Defense = defense;
            pokemon.SpecialAttack = specialAttack;
            pokemon.SpecialDefense = specialDefense;
            pokemon.Speed = speed;
            pokemon.HPEffort = hpEffort;
            pokemon.AttackEffort = attackEffort;
            pokemon.DefenseEffort = defenseEffort;
            pokemon.SpecialAttackEffort = specialAttackEffort;
            pokemon.SpecialDefenseEffort= specialDefenseEffort;
            pokemon.SpeedEffort = speedEffort;
            pokemon.SpriteFrontDefault = spriteFrontDefault;
            pokemon.PrimaryAbility = primaryAbility;
            pokemon.SecondaryAbility = secondaryAbility;
            pokemon.HiddenAbility = hiddenAbility;
            pokemon.SpriteFrontFemale = spriteFrontFemale;
            pokemon.SpriteFrontShiny = spriteFrontShiny;
            pokemon.SpriteFrontShinyFemale = spriteFrontShinyFemale;
            pokemon.SpriteBackDefault = spriteBackDefault;
            pokemon.SpriteBackFemale = spriteBackFemale;
            pokemon.SpriteBackShiny = spriteBackShiny;
            pokemon.SpriteBackShinyFemale = spriteBackShinyFemale;
            pokemon.Cry = cry;
            pokemon.CryLegacy = cryLegacy;
            pokemon.PrimaryType = primaryType;
            pokemon.SecondaryType = secondaryType;
            return pokemon;
        }
        static public PokemonSpecies ParsePokemonSpecies(JObject node)
        {
            int baseHappiness = node["base_happiness"].ToObject<int>();
            int captureRate = node["capture_rate"].ToObject<int>();
            int genderRate = node["gender_rate"].ToObject<int>();
            int? hatchCounter = node["hatch_counter"].ToObject<int?>();
            int id = node["id"].ToObject<int>();
            int order = node["order"].ToObject<int>();
            bool isBaby = node["is_baby"].ToObject<bool>();
            bool isLegendary = node["is_legendary"].ToObject<bool>();
            bool isMythical = node["is_mythical"].ToObject<bool>();
            string color = node["color"].ToObject<string>();
            string growthRate = node["growth_rate"].ToObject<string>();
            string habitat = node["habitat"].ToObject<string>();
            string shape = node["shape"].ToObject<string>();
            int generation = node["generation"].ToObject<int>();

            JObject generaNode = GetEnglishNode(node["genera"]?.ToObject<JArray>() ?? null);
            string genera = "";
            if (generaNode != null)
            {
                genera = generaNode["genus"].ToObject<string>();
            }

            JObject nationalPokedexNumberNode = GetEnglishNode(node["national_pokedex_number"]?.ToObject<JArray>() ?? null);
            int nationalPokedexNumber = -1;
            if (generaNode != null)
            {
                nationalPokedexNumber = nationalPokedexNumberNode["entry_number"].ToObject<int>();
            }

            JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
            string name = node["name"].ToObject<string>();
            if (nameNode != null)
            {
                name = nameNode["name"].ToObject<string>();
            }

            JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
            string? description = null;
            if (descriptionNode != null)
            {
                description = descriptionNode["flavor_text"].ToObject<string>();
            }

            PokemonSpecies species = new PokemonSpecies();
            species.ID = id;
            species.Name = name;
            species.BaseHappiness = baseHappiness;
            species.CaptureRate = captureRate;
            species.GenderRate = genderRate;
            species.Order = order;
            species.Generation = generation;
            species.NationalPokedexNumber = nationalPokedexNumber;
            species.IsBaby = isBaby;
            species.IsLegendary = isLegendary;
            species.IsMythical = isMythical;
            species.Color = color;
            species.GrowthRate = growthRate;
            species.Habitat = habitat;
            species.Shape = shape;
            species.Genera = genera;
            species.Description = description;
            species.HatchCounter = hatchCounter;
            return species;
        }
        static public List<EvolutionChain> ParseEvolutionChain(JObject node, List<EvolutionChain> list = null)
        {
            if (list == null)
            {
                list = new List<EvolutionChain>();
                ParseEvolutionChain(node["chain"].ToObject<JObject>(), list);
                return list;
            }

            foreach (JObject evolution in node["evolves_to"]?.ToObject<JArray>() ?? null)
            {
                foreach (JObject details in node["evolution_details"]?.ToObject<JArray>())
                {
                    int from = (int)GetURLIntValue(node["species"]["url"].ToObject<string>());
                    int to = (int)GetURLIntValue(evolution["species"]["url"].ToObject<string>());

                    int id = -1;

                    EvolutionChain chain = new EvolutionChain();

                    chain.ID = id;
                    chain.EvolvesFrom = from;
                    chain.EvolvesTo = to;
                    chain.Gender = details["gender"].ToObject<int?>();
                    chain.MinBeauty = details["min_beauty"].ToObject<int?>();
                    chain.MinHappiness = details["min_happiness"].ToObject<int?>();
                    chain.MinLevel = details["min_level"].ToObject<int?>();
                    chain.TradeSpecies = GetURLIntValue(details["trade_species"].ToObject<string?>());
                    chain.RelativePhysicalStats = details["relative_physical_stats"].ToObject<int?>();
                    chain.Item = details["item"].ToObject<string?>();
                    chain.HeldItem = details["helpItem"].ToObject<string?>();
                    chain.KnownMove = GetURLIntValue(details["known_move"].ToObject<string?>());
                    chain.KnownMoveType = details["known_move_type"].ToObject<string?>();
                    chain.Trigger = details["trigger"].ToObject<string?>();
                    chain.PartySpecies = GetURLIntValue(details["party_species"].ToObject<string?>());
                    chain.PartyType = details["party_type"].ToObject<string?>();
                    chain.TimeOfDay = details["time_of_day"].ToObject<string?>();
                    chain.NeedsOverworldRain = details["needs_overworld_rain"].ToObject<bool?>();
                    chain.TurnUpsideDown = details["turn_upside_down"].ToObject<bool?>();

                    list.Add(chain);
                }

                ParseEvolutionChain(node, list);
            }

            return list;
        }
        static public List<PokemonMove> ParsePokemonMove(JObject pokemonJson)
        {
            List<PokemonMove> list = new List<PokemonMove>();

            int pokemon = pokemonJson["id"].ToObject<int>();
            foreach (JObject m in pokemonJson["moves"]?.ToObject<JArray>())
            {
                int index = (m["version_group_details"]?.ToObject<JArray>().Count() ?? 1) - 1;
                int move = (int)GetURLIntValue(m["move"]["url"].ToObject<string>());
                int? levelLearnedAt = m["version_group_details"][index]["level_learned_at"].ToObject<int>();
                string? learnMethod = m["version_group_details"][index]["learn_method"]["name"].ToObject<string>();

                PokemonMove pm = new PokemonMove();
                pm.ID = -1;
                pm.Pokemon = pokemon;
                pm.Move = move;
                pm.LearnMethod = learnMethod;
                pm.LevelLearnedAt = levelLearnedAt;

                list.Add(pm);
            }
            return list;
        }

        static private int? GetURLIntValue(string url)
        {
            if (url == null) return null;
            string[] split = url.Split('/');
            return int.Parse(split[split.Length - 2]);
        }
        static private JObject GetEnglishNode(JArray node)
        {
            if (node == null) return null;
            foreach (JObject n in node)
            {
                if (n == null) continue;
                if (n["language"] == null) continue;
                if (n["language"]["name"] == null) continue;
                if (n["language"]["name"].ToString().Equals("en")) return n;
            }
            return null;
        }
    }
}
