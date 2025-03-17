using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PokedexExplorer.Model;

namespace PokedexExplorer.Data
{
    public class PokeAPIFetcher
    {
        static public JsonNode RetrieveJSON(string name, int? id = 0)
        {
            string url = "https://pokeapi.co/api/v2/" + name + "/";
            if (id != null) url += id + "/";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;

                    return JsonObject.Parse(jsonResponse);
                }
                catch
                {
                    return null;
                }
            }
        }

        static public int GetCount(string name)
        {
            JsonNode json = RetrieveJSON(name);
            if (json == null) return -1;
            return json["count"].GetValue<int>();
        }

        static public Ability ParseAbility(JsonNode node)
        {
            if (node == null) return null;
            try
            {
                int id = node["id"].GetValue<int>();
                int generation = node["generation"].GetValue<int>();

                JsonNode effectNode = GetEnglishNode(node["effect_entries"].GetValue<JsonNode>());
                string effect = null;
                string shortEffect = null;
                if (effectNode != null)
                {
                    effect = effectNode["effect"].GetValue<string>();
                    effect = effectNode["short_effect"].GetValue<string>();
                }

                JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
                string description = null;
                if (descriptionNode != null)
                {
                    description = descriptionNode["flavor_text"].GetValue<string>();
                }

                JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
                string name = node["name"].GetValue<string>();
                if (nameNode != null)
                {
                    name = nameNode["name"].GetValue<string>();
                }

                Ability ability = new Ability(id, name);
                ability.Generation = generation;
                ability.Effect = effect;
                ability.ShortEffect = shortEffect;
                ability.Description = description;
                return ability;
            }
            catch
            {
                return null;
            }
        }
        static public Move ParseMove(JsonNode node)
        {
            if (node == null) return null;

            int? accuracy = node["accuracy"].GetValue<int?>();
            string? damageClass = node["damage_class"]["name"].GetValue<string?>();
            int? effectChance = node["effectChance"].GetValue<int?>();
            int? generation = GetURLIntValue(node["generation"]["url"].GetValue<string?>());
            int id = node["id"].GetValue<int>();

            string? ailment = node["meta"] == null ? null : (node["meta"]["ailment"] == null ? null : node["meta"]["ailment"]["name"].GetValue<string>());
            int? ailmentChance = node["meta"] == null ? null : node["meta"]["ailment_chance"].GetValue<int?>();
            int? critRate = node["meta"] == null ? null : node["meta"]["crit_rate"].GetValue<int?>();
            int? drain = node["meta"] == null ? null : node["meta"]["drain"].GetValue<int?>();
            int? flinchChance = node["meta"] == null ? null : node["meta"]["flinch_chance"].GetValue<int?>();
            int? healing = node["meta"] == null ? null : node["meta"]["healing"].GetValue<int?>();
            int? maxHits = node["meta"] == null ? null : node["meta"]["max_hits"].GetValue<int?>();
            int? maxTurns = node["meta"] == null ? null : node["meta"]["max_turns"].GetValue<int?>();
            int? minHits = node["meta"] == null ? null : node["meta"]["min_hits"].GetValue<int?>();
            int? minTurns = node["meta"] == null ? null : node["meta"]["min_turns"].GetValue<int?>();
            int? statChance = node["meta"] == null ? null : node["meta"]["stat_chance"].GetValue<int?>();

            JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
            string name = node["name"].GetValue<string>();
            if (nameNode != null)
            {
                name = nameNode["name"].GetValue<string>();
            }

            int? power = node["power"].GetValue<int?>();
            int pp = node["pp"].GetValue<int>();
            int priority = node["priority"].GetValue<int>();
            string target = node["target"].GetValue<string>();
            string type = node["type"].GetValue<string>();

            JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
            string description = null;
            if (descriptionNode != null)
            {
                description = descriptionNode["flavor_text"].GetValue<string>();
            }

            Move move = new Move(id, name, pp, priority, target, type);
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
        static public Pokemon ParsePokemon(JsonNode node)
        {
            if (node == null) return null;

            int?[] abilities = new int?[] { null, null, null };
            foreach (JsonNode a in node["abilities"].AsArray())
            {
                int? value = GetURLIntValue(a["ability"]["url"].GetValue<string>());
                int index = a["slot"].GetValue<int>();
                abilities[index] = value;
            }
            int? primaryAbility = abilities[0];
            int? secondaryAbility = abilities[1];
            int? hiddenAbility = abilities[2];

            int baseExperience = node["base_experience"].GetValue<int>();
            int height = node["height"].GetValue<int>();
            int weight = node["weight"].GetValue<int>();
            int id = node["id"].GetValue<int>();
            int order = node["order"].GetValue<int>();
            string name = node["name"].GetValue<string>();

            string spriteFrontDefault = node["sprite_front_default"].GetValue<string>();
            string? spriteFrontFemale = node["sprite_front_female"].GetValue<string?>();
            string? spriteFrontShiny = node["sprite_front_shiny"].GetValue<string?>();
            string? spriteFrontShinyFemale = node["sprite_front_shiny_female"].GetValue<string?>();
            string? spriteBackDefault = node["sprite_back_default"].GetValue<string?>();
            string? spriteBackFemale = node["sprite_back_female"].GetValue<string?>();
            string? spriteBackShiny = node["sprite_back_shiny"].GetValue<string?>();
            string? spriteBackShinyFemale = node["sprite_back_shiny_female"].GetValue<string?>();

            int species = node["species"].GetValue<int>();

            string? cry = node["cry"].GetValue<string?>();
            string? cryLegacy = node["cry"].GetValue<string?>();

            int hp = node["stats"][0]["base_stat"].GetValue<int>();
            int hpEffort = node["stats"][0]["effort"].GetValue<int>();
            int attack = node["stats"][1]["base_stat"].GetValue<int>();
            int attackEffort = node["stats"][1]["effort"].GetValue<int>();
            int defense = node["stats"][2]["base_stat"].GetValue<int>();
            int defenseEffort = node["stats"][2]["effort"].GetValue<int>();
            int specialAttack = node["stats"][3]["base_stat"].GetValue<int>();
            int specialAttackEffort = node["stats"][3]["effort"].GetValue<int>();
            int specialDefense = node["stats"][4]["base_stat"].GetValue<int>();
            int specialDefenseEffort = node["stats"][4]["effort"].GetValue<int>();
            int speed = node["stats"][5]["base_stat"].GetValue<int>();
            int speedEffort = node["stats"][5]["effort"].GetValue<int>();

            string primaryType = node["types"][0].GetValue<string>();
            string? secondaryType = node["types"].AsArray().Count == 1 ? null : node["types"][1].GetValue<string?>();

            Pokemon pokemon = new Pokemon(id, baseExperience, height, weight, order, species, hp, hpEffort, attack, attackEffort,
                defense, defenseEffort, specialAttack, specialAttackEffort, specialDefense, specialDefenseEffort, speed, speedEffort, spriteFrontDefault, name, primaryType);
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
            pokemon.SecondaryType = secondaryType;
            return pokemon;
        }
        static public PokemonSpecies ParsePokemonSpecies(JsonNode node)
        {
            int baseHappiness = node["base_happiness"].GetValue<int>();
            int captureRate = node["capture_rate"].GetValue<int>();
            int genderRate = node["gender_rate"].GetValue<int>();
            int? hatchCounter = node["hatch_counter"].GetValue<int?>();
            int id = node["id"].GetValue<int>();
            int order = node["order"].GetValue<int>();
            bool isBaby = node["is_baby"].GetValue<bool>();
            bool isLegendary = node["is_legendary"].GetValue<bool>();
            bool isMythical = node["is_mythical"].GetValue<bool>();
            string color = node["color"].GetValue<string>();
            string growthRate = node["growth_rate"].GetValue<string>();
            string habitat = node["habitat"].GetValue<string>();
            string shape = node["shape"].GetValue<string>();
            int generation = node["generation"].GetValue<int>();

            JsonNode generaNode = GetEnglishNode(node["genera"].GetValue<JsonNode>());
            string genera = "";
            if (generaNode != null)
            {
                genera = generaNode["genus"].GetValue<string>();
            }

            JsonNode nationalPokedexNumberNode = GetEnglishNode(node["national_pokedex_number"].GetValue<JsonNode>());
            int nationalPokedexNumber = -1;
            if (generaNode != null)
            {
                nationalPokedexNumber = nationalPokedexNumberNode["entry_number"].GetValue<int>();
            }

            JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
            string name = node["name"].GetValue<string>();
            if (nameNode != null)
            {
                name = nameNode["name"].GetValue<string>();
            }

            JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
            string? description = null;
            if (descriptionNode != null)
            {
                description = descriptionNode["flavor_text"].GetValue<string>();
            }

            PokemonSpecies species = new PokemonSpecies(id, baseHappiness, captureRate, genderRate, order, generation, nationalPokedexNumber, isBaby, isLegendary, isMythical, color, growthRate, habitat, shape, genera, name);
            species.Description = description;
            species.HatchCounter = hatchCounter;
            return species;
        }
        static public List<EvolutionChain> ParseEvolutionChain(JsonNode node, List<EvolutionChain> list = null)
        {
            if (list == null)
            {
                list = new List<EvolutionChain>();
                ParseEvolutionChain(node["chain"], list);
                return list;
            }

            foreach (JsonNode evolution in node["evolves_to"].AsArray())
            {
                foreach (JsonNode details in node["evolution_details"].AsArray())
                {
                    int from = (int)GetURLIntValue(node["species"]["url"].GetValue<string>());
                    int to = (int)GetURLIntValue(evolution["species"]["url"].GetValue<string>());

                    int id = -1;

                    EvolutionChain chain = new EvolutionChain(id, from, to);

                    chain.Gender = details["gender"].GetValue<int?>();
                    chain.MinBeauty = details["min_beauty"].GetValue<int?>();
                    chain.MinHappiness = details["min_happiness"].GetValue<int?>();
                    chain.MinLevel = details["min_level"].GetValue<int?>();
                    chain.TradeSpecies = GetURLIntValue(details["trade_species"].GetValue<string?>());
                    chain.RelativePhysicalStats = details["relative_physical_stats"].GetValue<int?>();
                    chain.Item = details["item"].GetValue<string?>();
                    chain.HeldItem = details["helpItem"].GetValue<string?>();
                    chain.KnownMove = GetURLIntValue(details["known_move"].GetValue<string?>());
                    chain.KnownMoveType = details["known_move_type"].GetValue<string?>();
                    chain.Trigger = details["trigger"].GetValue<string?>();
                    chain.PartySpecies = GetURLIntValue(details["party_species"].GetValue<string?>());
                    chain.PartyType = details["party_type"].GetValue<string?>();
                    chain.TimeOfDay = details["time_of_day"].GetValue<string?>();
                    chain.NeedsOverworldRain = details["needs_overworld_rain"].GetValue<bool?>();
                    chain.TurnUpsideDown = details["turn_upside_down"].GetValue<bool?>();

                    list.Add(chain);
                }

                ParseEvolutionChain(node, list);
            }

            return list;
        }
        static public List<PokemonMove> ParsePokemonMove(JsonNode pokemonJson)
        {
            List<PokemonMove> list = new List<PokemonMove>();

            int pokemon = pokemonJson["id"].GetValue<int>();
            foreach (JsonNode m in pokemonJson["moves"].AsArray())
            {
                int index = m["version_group_details"].AsArray().Count() - 1;
                int move = (int)GetURLIntValue(m["move"]["url"].GetValue<string>());
                int? levelLearnedAt = m["version_group_details"][index]["level_learned_at"].GetValue<int>();
                string? learnMethod = m["version_group_details"][index]["learn_method"]["name"].GetValue<string>();

                PokemonMove pm = new PokemonMove(index, pokemon, move);
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
            return int.Parse(split[split.Length - 1]);
        }
        static private JsonNode GetEnglishNode(JsonNode node)
        {
            if (node == null) return null;
            foreach (JsonNode n in node.AsArray())
            {
                if (n == null) continue;
                if (n["language"] == null) continue;
                if (n["language"]["name"] == null) continue;
                if (n["language"]["name"].Equals("n")) return n;
            }
            return null;
        }
    }
}
