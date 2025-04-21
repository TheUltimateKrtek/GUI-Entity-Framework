using System.Windows.Media.Imaging;

namespace PokedexExplorer.Data{
    public class ShowPokemonDetail{
        //definování potřebných vlastností
        public string Name { get; set; }
        public string? SpriteFrontDefault { get; set; }
        public string? PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
        public string? Move { get; set; }
        public string? Abilities { get; set; }
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
        public ShowPokemonDetail(string name, string? spriteFrontDefault, string type, string secondaryType, string move, string abilitities, bool is_legendary, string color, string shape, string description,
                                int height, int weight, int hp, int defense, int attack, int speed){
            Name = name?.ToUpper() ?? "UNKOWN";
            SpriteFrontDefault = spriteFrontDefault; SpriteImage = GetImage(spriteFrontDefault);
            PrimaryType = type.ToUpper();
            SecondaryType = secondaryType?.ToUpper() ?? "UNKOWN";
            Move = move ?? "UNKOWN";
            Abilities = abilitities ?? "UNKOWN";
            Legendary = is_legendary == true ? "Yes" : "No";
            Color = color ?? "Unkown";
            Shape = shape ?? "Unkown";
            Description = description ?? "Unkown";
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
        public PokemonData(PokemonDbContext context) { this.context = context; }
        public async Task<IQueryable<ShowPokemonDetail>> Find(string name){
            if (string.IsNullOrEmpty(name)) return null;    
            return await Task.Run(() =>{
                return context.Pokemon
                    .Where(p => p.Name == name.ToLower())
                    .Join(context.Ability, pokemon => pokemon.ID, ability => ability.ID, (pokemon, ability) => new { pokemon, ability } )
                    .Join(context.Move, combined => combined.pokemon.ID, move => move.ID, (combined, move) => new { combined, move } )
                    .Join(context.PokemonSpecies, combined => combined.combined.pokemon.ID, PokemonSpecie => PokemonSpecie.ID, (combined, PokemonSpecie) => new { combined, PokemonSpecie })
                    .GroupBy(combined => combined.combined.combined.pokemon.ID)
                    .Select(p => new ShowPokemonDetail(
                        p.First().combined.combined.pokemon.Name, 
                        p.First().combined.combined.pokemon.SpriteFrontDefault,
                        p.First().combined.combined.pokemon.PrimaryType,
                        p.First().combined.combined.pokemon.SecondaryType,
                        p.First().combined.move.Name,
                        p.First().combined.combined.ability.Name,
                        p.First().PokemonSpecie.IsLegendary,
                        p.First().PokemonSpecie.Color,
                        p.First().PokemonSpecie.Shape,
                        p.First().combined.combined.ability.Description,
                        p.First().combined.combined.pokemon.Height,
                        p.First().combined.combined.pokemon.Weight,
                        p.First().combined.combined.pokemon.HP,
                        p.First().combined.combined.pokemon.Defense,
                        p.First().combined.combined.pokemon.Attack,
                        p.First().combined.combined.pokemon.Speed
                    ));
            });
        }
    }
    
}
