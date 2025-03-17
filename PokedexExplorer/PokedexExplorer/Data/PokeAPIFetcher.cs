using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
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

            int?[] abilities = new int?[] {null, null, null};
            foreach (JsonNode a in node["abilities"].AsArray())
            {
                int? value = GetURLIntValue(a["ability"]["url"].GetValue<string>());
                int index = a["slot"].GetValue<int>();
                abilities[index] = value;
            }

            int? baseExperience = node["base_experience"].GetValue<int?>();
            int? height = node["height"].GetValue<int?>();
            int? weight = node["weight"].GetValue<int?>();
            int id = node["id"].GetValue<int>();
            int? order = node["order"].GetValue<int?>();
            string name = node["name"].GetValue<string>();

            //TODO
            return null;
        }
        static public PokemonSpecies ParsePokemonSpecies(JsonNode node)
        {
            //TODO
            return null;
        }
        static public EvolutionChain ParseEvolutionChain(JsonNode node)
        {
            //TODO
            return null;
        }
        static public PokemonMove ParsePokemonMove(JsonNode node)
        {
            //TODO
            return null;
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
