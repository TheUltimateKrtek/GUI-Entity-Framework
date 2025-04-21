using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using PokedexExplorer.Data;

namespace PokedexExplorer;
public partial class MainWindow : Window {
    static public readonly bool INITIALIZE_TABLES = false;
    static public readonly bool INITIALIZE_DATA = false;

    private readonly PokemonDbContext context;
    public DatabaseInitHandler Handler { get; private set; }
    public PokemonSearch Search { get; private set; }
    public PokemonData pokemonDetail { get; private set; }
    
    public MainWindow() {
        InitializeComponent();
        context = new PokemonDbContext("skyre", "");
        //Add the init handler
        Handler = new DatabaseInitHandler(this, this.context);
        if (INITIALIZE_TABLES) {
            try {
                context.Database.ExecuteSqlRaw(context.Database.GenerateCreateScript()); Debug.WriteLine("Created tables!");
            }
            catch (Exception e){ Debug.WriteLine(e.Message); }
        }
        if (INITIALIZE_DATA) { //Run the init handler
            Handler.Start();
        }
        this.Search = new PokemonSearch(this.context, this);
        this.Search.Init();
    }
    public void OnQueryUpdated(){
        Debug.WriteLine("Updated");
        List<PokemonGridData> data = Search.Query.AsQueryable().ToList();
        if (data != null) PokemmonDataGrid.ItemsSource = data;
    }
    private void FetchGroupMouseDown(object sender, MouseButtonEventArgs e){ }
    private void SearchedNameTextChanged(object sender, TextChangedEventArgs e) { 
        this.Search.Name = ((TextBox)sender).Text.ToLower();
    }
    private void Pokemon_MouseEnter(object sender, MouseEventArgs e){
        Border border = (Border)sender;
        border.Background = new SolidColorBrush(Colors.LightGreen);
        border.BorderThickness = new Thickness(1);
    }
    private void Pokemon_MouseLeave(object sender, MouseEventArgs e){
        Border border = (Border)sender;
        border.Background = null;
        border.BorderThickness = new Thickness(1);
    }
    private async void Pokemon_MouseLeftButtonClick(object sender, MouseButtonEventArgs e)
    {
        Border border = (Border)sender;
        PokemonGridData pokemon = (PokemonGridData)border.DataContext;
        Debug.WriteLine("Pokemon " + pokemon.Name + " clicked");

        pokemonDetail = new PokemonData(context);
        var pokemon_query = await pokemonDetail.Find(pokemon.Name);
        ShowPokemonDetail pokemon_details = pokemon_query?.FirstOrDefault();

        if (pokemon_details != null){
            PokemonNameTextBlock.Text = pokemon_details.Name;
            PokemonNameTextBlock.Foreground = (Brush)new BrushConverter().ConvertFromString(pokemon_details.PrimaryColor);

            PokemonImage.Source = pokemon_details.SpriteImage;
            PokemonTypeTextBlock.Text = pokemon_details.PrimaryType;
            PokemonSecondaryTypeTextBlock.Text = pokemon_details.SecondaryType;
            PokemonMoveTextBlock.Text = pokemon_details.Move;

            PokemonHeightTextBlock.Text = pokemon_details.Height;
            PokemonWeightTextBlock.Text = pokemon_details.Weight;
            PokemonAbilitiesTextBlock.Text = pokemon_details.Abilities;
            PokemonHPTextBlock.Text = pokemon_details.HP;
            PokemonDefenseTextBlock.Text = pokemon_details.Defense;
            PokemonAttackTextBlock.Text = pokemon_details.Attack;
            PokemonSpeedTextBlock.Text = pokemon_details.Speed;
            PokemonLegendaryStatusTextBlock.Text = pokemon_details.Legendary;
            PokemonColorTextBlock.Text = pokemon_details.Color;
            PokemonShapeTextBlock.Text = pokemon_details.Shape;

            PokemonDescriptionTextBlock.Text = pokemon_details.Description;
            
            PokemonDetailsGrid.Visibility = Visibility.Visible;
        }
    }
    private void CloseDetailsButton_Click(object sender, RoutedEventArgs e){ PokemonDetailsGrid.Visibility = Visibility.Collapsed; }
    private void SearchedType1Changed(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.Type1 = null;
        else this.Search.Type1 = str.ToLower();
    }
    private void SearchedType2Changed(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.Type2 = null;
        else this.Search.Type2 = str.ToLower();
    }
    private void SearchedGenerationChanged(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Tag.ToString();
        if (str.Equals("Any")) this.Search.Generation = null;
        else this.Search.Generation = int.Parse(str.ToLower());
    }
    private void SearchedMoveTextChanged(object sender, TextChangedEventArgs e){
        this.Search.Move = ((TextBox)sender).Text.ToLower();
    }
    private void SearchedAbilityTextChanged(object sender, TextChangedEventArgs e){
        this.Search.Ability = ((TextBox)sender).Text.ToLower();
    }
    private void SearchedLegendaryStatusSelectionChanged(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.LegendaryStatus = null;
        else this.Search.LegendaryStatus = str.ToLower();
    }
    private void SearchedAppearanceColorSelectionChanged(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.AppearanceColor = null;
        else this.Search.AppearanceColor = str.ToLower();
    }
    private void SearchedAppearanceShapeSelectionChanged(object sender, SelectionChangedEventArgs e){
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.AppearanceShape = null;
        else this.Search.AppearanceShape = str.ToLower().Replace(" ", "-");
    }
    private void SearchedAppearanceHeightMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceHeightMin = null;
                return;
            }
            else this.Search.AppearanceHeightMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex) {
            ((TextBox)sender).BorderBrush = Brushes.Red;
            throw new Exception("", ex);
        }
    }
    private void SearchedAppearanceHeightMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceHeightMax = null;
                return;
            }
            else this.Search.AppearanceHeightMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedAppearanceWeightMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceWeightMin = null;
                return;
            }
            else this.Search.AppearanceWeightMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedAppearanceWeightMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceWeightMax = null;
                return;
            }
            else this.Search.AppearanceWeightMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red;}
    }
    private void SearchedStatHPMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0){
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatHPMin = null;
                return;
            }
            else this.Search.StatHPMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatHPMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatHPMax = null;
                return;
            }
            else this.Search.StatHPMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatAttackMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatAttackMin = null;
                return;
            }
            else this.Search.StatAttackMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatAttackMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatAttackMax = null;
                return;
            }
            else this.Search.StatAttackMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatDefenseMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatDefenseMin = null;
                return;
            }
            else this.Search.StatDefenseMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatDefenseMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatDefenseMax = null;
                return;
            }
            else this.Search.StatDefenseMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpecialAttackMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialAttackMin = null;
                return;
            }
            else this.Search.StatSpecialAttackMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpecialAttackMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialAttackMax = null;
                return;
            }
            else this.Search.StatSpecialAttackMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpecialDefenseMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0){
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialDefenseMin = null;
                return;
            }
            else this.Search.StatSpecialDefenseMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpecialDefenseMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialDefenseMax = null;
                return;
            }
            else this.Search.StatSpecialDefenseMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpeedMinChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpeedMin = null;
                return;
            }
            else this.Search.StatSpeedMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
    private void SearchedStatSpeedMaxChanged(object sender, TextChangedEventArgs e) {
        try {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpeedMin = null;
                return;
            }
            else this.Search.StatSpeedMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex){ ((TextBox)sender).BorderBrush = Brushes.Red; }
    }
}