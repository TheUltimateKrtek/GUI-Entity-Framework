using Microsoft.EntityFrameworkCore;
using PokedexExplorer.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PokedexExplorer.Data{
    public class ShowPokemonDetail{
        //definování potřebných vlastností
        public string Name { get; set; }
        public string? SpriteFrontDefault { get; set; }
        public string? PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
        public List<string> Moves { get; set; }
        public List<string> Abilities { get; set; }
        public string? Legendary { get; set; }
        public string? Color { get; set; }
        public string? Shape { get; set; }
        public string? Description { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? HP { get; set; }
        public string? Defense { get; set; }
        public string? Attack { get; set; }
        public string? Speed { get; set; }

        public string PrimaryColor { get { if (PrimaryType == "NORMAL") return "#A8A77A"; if (PrimaryType == "FIRE") return "#EE8130"; if (PrimaryType == "WATER") return "#6390F0"; if (PrimaryType == "ELECTRIC") return "#F7D02C"; if (PrimaryType == "GRASS") return "#7AC74C"; if (PrimaryType == "ICE") return "#96D9D6"; if (PrimaryType == "FIGHTING") return "#C22E28"; if (PrimaryType == "POISON") return "#A33EA1"; if (PrimaryType == "GROUND") return "#E2BF65"; if (PrimaryType == "FLYING") return "#A98FF3"; if (PrimaryType == "PSYCHIC") return "#F95587"; if (PrimaryType == "BUG") return "#A6B91A"; if (PrimaryType == "ROCK") return "#B6A136"; if (PrimaryType == "GHOST") return "#735797"; if (PrimaryType == "DRAGON") return "#6F35FC"; if (PrimaryType == "DARK") return "#705746"; if (PrimaryType == "STEEL") return "#B7B7CE"; if (PrimaryType == "FAIRY") return "#D685AD"; return "#00FFFFFF"; }}
        //zpracování obrázků
        public BitmapImage? SpriteImage { get; set; }
        static private readonly Dictionary<string, BitmapImage> _imageCache = new Dictionary<string, BitmapImage>();
        static public BitmapImage GetImage(string url){
            if (url == null) return null;
            if (!_imageCache.ContainsKey(url)){
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                _imageCache[url] = bitmap;
            }
            return _imageCache[url];
        }
        public ShowPokemonDetail(string name, string? spriteFrontDefault, string type, string secondaryType, List<string> moves, List<string> abilities, 
                                bool is_legendary, string color, string shape, string description,
                                int height, int weight, int hp, int defense, int attack, int speed){
            Name = name?.ToUpper() ?? "UNKOWN";
            SpriteFrontDefault = spriteFrontDefault; SpriteImage = GetImage(spriteFrontDefault);
            PrimaryType = type.ToUpper();
            SecondaryType = secondaryType?.ToUpper() ?? "UNKOWN";
            Moves = moves != null && moves.Count > 0 ? moves : new List<string> { "UNKNOWN" };
            Abilities = abilities != null && abilities.Count > 0 ? abilities : new List<string> { "UNKNOWN" };
            Debug.WriteLine(is_legendary);
            Legendary = is_legendary == true ? "Yes" : "No";
            Color = color ?? "Unkown";
            Shape = shape ?? "Unkown";
            Description = description?.Replace("\n", " ") ?? "Unkown";
            Height = height.ToString()?? "Unkown";
            Weight = weight.ToString() ?? "Unkown";
            HP = hp.ToString() ?? "Unkown";
            Defense = defense.ToString() ?? "Unkown";
            Attack = attack.ToString() ?? "Unkown";
            Speed = speed.ToString() ?? "Unkown";
        }
    }
    //třída pro načtení dat z databáze
    public class  PokemonData{
        public IQueryable<ShowPokemonDetail> Query { get; private set; }
        private PokemonDbContext context;
        public PokemonData(PokemonDbContext context){ this.context = context; }
        
        public async Task<IQueryable<ShowPokemonDetail>>  Find(string name){
            if (string.IsNullOrEmpty(name)) return null;    
            return await Task.Run(() => {
                return context.Pokemon
                    .Where(p => p.Name == name.ToLower())
                    //.Join(context.PokemonSpecies, combined => combined.ID, PokemonSpecie => PokemonSpecie.ID, (combined, PokemonSpecie) => new { combined, PokemonSpecie })
                    .Select(p => new ShowPokemonDetail(
                        p.Name,
                        p.SpriteFrontDefault,
                        p.PrimaryType,
                        p.SecondaryType,
                        context.Move
                            .Join(context.PokemonMove, m => m.ID, pm => pm.Move, (m, pm) => new { Move = m, PokemonMove = pm })
                            .Where(x => x.PokemonMove.Pokemon == p.ID)
                            .Select(x => x.Move.Name)
                            .ToList(),
                        new List<string> {
                            context.Ability
                                .Join(context.Ability, a => a.ID, pa => pa.ID, (a, pa) => new { Pokemon = a, Ability = pa })
                                .Where(x => x.Ability.ID == p.PrimaryAbility)
                                .Select( x => x.Ability.Name).FirstOrDefault(),
                            context.Ability
                                    .Join(context.Ability, a2 => a2.ID, pa2 => pa2.ID, (a2, pa2) => new { Pokemon = a2, Ability = pa2 })
                                    .Where(x => x.Ability.ID == p.SecondaryAbility)
                                    .Select(x => x.Ability.Name).FirstOrDefault(),
                            context.PokemonSpecies
                                .Join(context.Ability, a3 => a3.ID, pa3 => pa3.ID, (a3, pa3) => new { Pokemon = a3, Ability = pa3 })
                                .Where(x => x.Ability.ID == p.HiddenAbility)
                                .Select(x => x.Ability.Name).FirstOrDefault()
                        },
                        context.PokemonSpecies
                            .Where(ps => ps.ID == p.ID)
                            .Select(ps => ps.IsLegendary).FirstOrDefault(),
                        context.PokemonSpecies
                            .Where(ps => ps.ID == p.ID)
                            .Select(ps => ps.Color).FirstOrDefault(),
                        context.PokemonSpecies
                            .Where(ps => ps.ID == p.ID)
                            .Select(ps => ps.Shape).FirstOrDefault(),
                        context.PokemonSpecies
                            .Where(ps => ps.ID == p.ID)
                            .Select(ps => ps.Description).FirstOrDefault(),
                        p.Height,
                        p.Weight,
                        p.HP,
                        p.Defense,
                        p.Attack,
                        p.Speed
                    ));
            });
        }
    }
    
}
