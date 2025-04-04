using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PokedexExplorer.Data {
    public class PokemonSearch {
        private string? _name, _type1, _type2, _ability, _move, _legendaryStatus, _appearanceColor, _appearanceShape;
        private int? _generation, _appearanceHeightMin, _appearanceHeightMax, _appearanceWeightMin, _appearanceWeightMax;
        private int? _statHPMin, _statHPMax, _statAttackMin, _statAttackMax, _statDefenseMin, _statDefenseMax;
        private int? _statSpecialAttackMin, _statSpecialAttackMax, _statSpecialDefenseMin, _statSpecialDefenseMax, _statSpeedMin, _statSpeedMax;

        public string? Name { get => _name; set { _name = value; UpdateQuery(); } }
        public string? Type1 { get => _type1; set { _type1 = value; UpdateQuery(); } }
        public string? Type2 { get => _type2; set { _type2 = value; UpdateQuery(); } }
        public string? Ability { get => _ability; set { _ability = value; UpdateQuery(); } }
        public string? Move { get => _move; set { _move = value; UpdateQuery(); } }
        public int? Generation { get => _generation; set { _generation = value; UpdateQuery(); } }
        public string? AppearanceColor { get => _appearanceColor; set { _appearanceColor = value; UpdateQuery(); } }
        public string? AppearanceShape { get => _appearanceShape; set { _appearanceShape = value; UpdateQuery(); } }
        public int? AppearanceHeightMin { get => _appearanceHeightMin; set { _appearanceHeightMin = value; UpdateQuery(); } }
        public int? AppearanceHeightMax { get => _appearanceHeightMax; set { _appearanceHeightMax = value; UpdateQuery(); } }
        public int? AppearanceWeightMin { get => _appearanceWeightMin; set { _appearanceWeightMin = value; UpdateQuery(); } }
        public int? AppearanceWeightMax { get => _appearanceWeightMax; set { _appearanceWeightMax = value; UpdateQuery(); } }
        public int? StatHPMin { get => _statHPMin; set { _statHPMin = value; UpdateQuery(); } }
        public int? StatHPMax { get => _statHPMax; set { _statHPMax = value; UpdateQuery(); } }
        public int? StatAttackMin { get => _statAttackMin; set { _statAttackMin = value; UpdateQuery(); } }
        public int? StatAttackMax { get => _statAttackMax; set { _statAttackMax = value; UpdateQuery(); } }
        public int? StatDefenseMin { get => _statDefenseMin; set { _statDefenseMin = value; UpdateQuery(); } }
        public int? StatDefenseMax { get => _statDefenseMax; set { _statDefenseMax = value; UpdateQuery(); } }
        public int? StatSpecialAttackMin { get => _statSpecialAttackMin; set { _statSpecialAttackMin = value; UpdateQuery(); } }
        public int? StatSpecialAttackMax { get => _statSpecialAttackMax; set { _statSpecialAttackMax = value; UpdateQuery(); } }
        public int? StatSpecialDefenseMin { get => _statSpecialDefenseMin; set { _statSpecialDefenseMin = value; UpdateQuery(); } }
        public int? StatSpecialDefenseMax { get => _statSpecialDefenseMax; set { _statSpecialDefenseMax = value; UpdateQuery(); } }
        public int? StatSpeedMin { get => _statSpeedMin; set { _statSpeedMin = value; UpdateQuery(); } }
        public int? StatSpeedMax { get => _statSpeedMax; set { _statSpeedMax = value; UpdateQuery(); } }
        public string? LegendaryStatus { get => _legendaryStatus; set { _legendaryStatus = value; UpdateQuery(); } }

        public IQueryable<PokemonGridData> Query { get; private set; }

        private PokemonDbContext context;
        private MainWindow window;
        public PokemonSearch(PokemonDbContext context, MainWindow window) { this.context = context; this.window = window; }

        public void Init() {
            Task.Run(() => { //spouštění dotazu na samostatném vlákně
                IQueryable<Model.Pokemon> query = context.Pokemon;
                this.Query = query.Select(p => new PokemonGridData(p.Name, p.PrimaryType, p.SecondaryType, p.SpriteFrontDefault));
                Application.Current.Dispatcher.Invoke(() => {
                    this.Query = Query.AsQueryable();
                    this.window.OnQueryUpdated();
                });
            });
        }
        private void UpdateQuery() {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; //optimalizace s kontextem databáze
            Task.Run(() => {
                IQueryable<Model.Pokemon> query = context.Pokemon;

                if (Name != null) { query = query.Where(p => p.Name.ToLower().StartsWith(Name)); }

                if (this.Type1 != null && this.Type1.Length > 0 || this.Type2 != null && this.Type2.Length > 0) {
                    query = query.Where(p =>
                        (this.Type1 != null && (p.PrimaryType == this.Type1 || p.SecondaryType == this.Type1)) ||
                        (this.Type2 != null && (p.PrimaryType == this.Type2 || p.SecondaryType == this.Type2))
                    );
                }

                if (this.Ability != null && this.Ability.Length > 0) {
                    query = query.Where(p => context.Ability.Any(a => (a.ID == p.PrimaryAbility || a.ID == p.SecondaryAbility || a.ID == p.HiddenAbility) && a.Name.ToLower().StartsWith(this.Ability)));
                }

                if (this.Move != null && this.Move.Length > 0) {
                    query = query.Where(p => context.PokemonMove.Any(pm => pm.Pokemon == p.ID && context.Move.Any(m => m.ID == pm.Move && m.Name.ToLower().StartsWith(this.Move))));
                }

                if (this.Generation != null) { query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Generation == this.Generation)); }

                if (this.LegendaryStatus != null && this.LegendaryStatus.Length > 0 && !this.LegendaryStatus.Equals("Any")) {
                    bool isLegendary = this.LegendaryStatus.ToLower().Equals("legendary");
                    bool isMythical = this.LegendaryStatus.ToLower().Equals("mythical");
                    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.IsLegendary == isLegendary && ps.IsMythical == isMythical && p.Species == ps.ID));
                }

                if (this.AppearanceColor != null && this.AppearanceColor.Length > 0) {
                    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Color == this.AppearanceColor && p.Species == ps.ID));
                }
                if (this.AppearanceShape != null && this.AppearanceShape.Length > 0) {
                    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Shape == this.AppearanceShape && p.Species == ps.ID));
                }

                if (this.AppearanceHeightMin != null) { query = query.Where(p => p.Height >= this.AppearanceHeightMin); }
                if (this.AppearanceHeightMax != null) { query = query.Where(p => p.Height <= this.AppearanceHeightMax); }

                if (this.AppearanceWeightMin != null) { query = query.Where(p => p.Weight >= this.AppearanceWeightMin); }
                if (this.AppearanceWeightMax != null) { query = query.Where(p => p.Weight <= this.AppearanceWeightMax); }

                if (this.StatHPMin != null) { query = query.Where(p => p.HP >= this.StatHPMin); }
                if (this.StatHPMax != null) { query = query.Where(p => p.HP <= this.StatHPMax); }

                if (this.StatAttackMin != null) { query = query.Where(p => p.Attack >= this.StatAttackMin); }
                if (this.StatAttackMax != null) { query = query.Where(p => p.Attack <= this.StatAttackMax); }

                if (this.StatDefenseMin != null) { query = query.Where(p => p.Defense >= this.StatDefenseMin); }
                if (this.StatDefenseMax != null) { query = query.Where(p => p.Defense <= this.StatDefenseMax); }

                if (this.StatSpecialAttackMin != null) { query = query.Where(p => p.SpecialAttack >= this.StatSpecialAttackMin); }
                if (this.StatSpecialAttackMax != null) { query = query.Where(p => p.SpecialAttack <= this.StatSpecialAttackMax); }

                if (this.StatSpecialDefenseMin != null) { query = query.Where(p => p.SpecialDefense >= this.StatSpecialDefenseMin); }
                if (this.StatSpecialDefenseMax != null) { query = query.Where(p => p.SpecialDefense <= this.StatSpecialDefenseMax); }

                if (this.StatSpeedMin != null) { query = query.Where(p => p.Speed >= this.StatSpeedMin); }
                if (this.StatSpeedMax != null) { query = query.Where(p => p.Speed <= this.StatSpeedMax); }

                this.Query = query.Select(p => new PokemonGridData(p.Name, p.PrimaryType, p.SecondaryType, p.SpriteFrontDefault));
                
                Application.Current.Dispatcher.Invoke(() => {
                    this.window.OnQueryUpdated();
                });
            });
        }
    }
    public class ImageResources {
        static private readonly Dictionary<string, BitmapImage> _imageCache = new Dictionary<string, BitmapImage>();
        static public BitmapImage GetImage(string url) {
            if (url == null) return null;
            if (!_imageCache.ContainsKey(url)) {
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                _imageCache[url] = bitmap;
            }
            return _imageCache[url];
        }
    }
    public class PokemonGridData{
        public string Name { get; set; }
        public string PrimaryColor { get {
                if (PrimaryType == "NORMAL") return "#A8A77A";
                if (PrimaryType == "FIRE") return "#EE8130";
                if (PrimaryType == "WATER") return "#6390F0";
                if (PrimaryType == "ELECTRIC") return "#F7D02C";
                if (PrimaryType == "GRASS") return "#7AC74C";
                if (PrimaryType == "ICE") return "#96D9D6";
                if (PrimaryType == "FIGHTING") return "#C22E28";
                if (PrimaryType == "POISON") return "#A33EA1";
                if (PrimaryType == "GROUND") return "#E2BF65";
                if (PrimaryType == "FLYING") return "#A98FF3";
                if (PrimaryType == "PSYCHIC") return "#F95587";
                if (PrimaryType == "BUG") return "#A6B91A";
                if (PrimaryType == "ROCK") return "#B6A136";
                if (PrimaryType == "GHOST") return "#735797";
                if (PrimaryType == "DRAGON") return "#6F35FC";
                if (PrimaryType == "DARK") return "#705746";
                if (PrimaryType == "STEEL") return "#B7B7CE";
                if (PrimaryType == "FAIRY") return "#D685AD";
                return "#00FFFFFF"; // Transparent color
            } }
        public string SecondaryColor { get {
                if (SecondaryType == "NORMAL") return "#A8A77A";
                if (SecondaryType == "FIRE") return "#EE8130";
                if (SecondaryType == "WATER") return "#6390F0";
                if (SecondaryType == "ELECTRIC") return "#F7D02C";
                if (SecondaryType == "GRASS") return "#7AC74C";
                if (SecondaryType == "ICE") return "#96D9D6";
                if (SecondaryType == "FIGHTING") return "#C22E28";
                if (SecondaryType == "POISON") return "#A33EA1";
                if (SecondaryType == "GROUND") return "#E2BF65";
                if (SecondaryType == "FLYING") return "#A98FF3";
                if (SecondaryType == "PSYCHIC") return "#F95587";
                if (SecondaryType == "BUG") return "#A6B91A";
                if (SecondaryType == "ROCK") return "#B6A136";
                if (SecondaryType == "GHOST") return "#735797";
                if (SecondaryType == "DRAGON") return "#6F35FC";
                if (PrimaryType == "DARK") return "#705746";
                if (SecondaryType == "STEEL") return "#B7B7CE";
                if (SecondaryType == "FAIRY") return "#D685AD";
                return "#00FFFFFF"; // Transparent color
            } }
        public string PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
        public string? SpriteFrontDefault { get; set; }
        public BitmapImage? SpriteImage { get; set; }
        public PokemonGridData(string name, string primaryType, string? secondaryType, string? spriteFrontDefault) {
            Name = name.ToUpper();
            PrimaryType = primaryType.ToUpper();
            SecondaryType = secondaryType?.ToUpper() ?? null;
            SpriteFrontDefault = spriteFrontDefault;
            SpriteImage = ImageResources.GetImage(spriteFrontDefault);
        }
    }
}
