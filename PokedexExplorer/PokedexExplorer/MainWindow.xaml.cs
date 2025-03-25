using System.CodeDom;
using System.Diagnostics;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PokedexExplorer.Data;
using PokedexExplorer.Model;

namespace PokedexExplorer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static public readonly bool INITIALIZE_TABLES = false;
    static public readonly bool INITIALIZE_DATA = false;

    private readonly PokemonDbContext context;
    public DatabaseInitHandler Handler { get; private set; }
    public PokemonSearch Search { get; private set; }

    public MainWindow()
    {
        InitializeComponent();
        context = new PokemonDbContext("skyre", "");

        //Add the init handler
        Handler = new DatabaseInitHandler(this, this.context);

        if (INITIALIZE_TABLES)
        {
            try
            {
                context.Database.ExecuteSqlRaw(context.Database.GenerateCreateScript());
                Debug.WriteLine("Created tables!");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        if (INITIALIZE_DATA)
        {
            //Run the init handler
            Handler.Start();
        }

        this.Search = new PokemonSearch(this.context, this);
        this.Search.Init();
    }

    public void OnQueryUpdated()
    {
        Debug.WriteLine("Updated");
        List<PokemonGridData> data = Search.Query.ToList();
        if (data != null) PokemmonDataGrid.ItemsSource = data;
    }


    private void FetchGroupMouseDown(object sender, MouseButtonEventArgs e)
    {

    }

    private void SearchedNameTextChanged(object sender, TextChangedEventArgs e)
    {
        this.Search.Name = ((TextBox)sender).Text;
    }
    private void SearchedType1Changed(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.Type1 = null;
        else this.Search.Type1 = str.ToLower();
    }
    private void SearchedType2Changed(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.Type2 = null;
        else this.Search.Type2 = str.ToLower();
    }
    private void SearchedGenerationChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.Generation = null;
        else this.Search.Generation = int.Parse(str.ToLower());
    }
    private void SearchedMoveTextChanged(object sender, TextChangedEventArgs e)
    {
        this.Search.Move = ((TextBox)sender).Text;

    }
    private void SearchedAbilityTextChanged(object sender, TextChangedEventArgs e)
    {
        this.Search.Ability = ((TextBox)sender).Text;

    }
    private void SearchedLegendaryStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.LegendaryStatus = null;
        else this.Search.LegendaryStatus = str.ToLower();

    }
    private void SearchedAppearanceColorSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.AppearanceColor = null;
        else this.Search.AppearanceColor = str.ToLower();

    }
    private void SearchedAppearanceShapeSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
        string str = selectedItem.Content.ToString();
        if (str.Equals("Any")) this.Search.AppearanceShape = null;
        else this.Search.AppearanceShape = str.ToLower();

    }
    private void SearchedAppearanceHeightMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.AppearanceHeightMin = null;
            else this.Search.AppearanceHeightMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedAppearanceHeightMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.AppearanceHeightMax = null;
            else this.Search.AppearanceHeightMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedAppearanceWeightMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.AppearanceWeightMin = null;
            else this.Search.AppearanceWeightMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedAppearanceWeightMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.AppearanceWeightMax = null;
            else this.Search.AppearanceWeightMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatHPMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatHPMin = null;
            else this.Search.StatHPMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatHPMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatHPMax = null;
            else this.Search.StatHPMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatAttackMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatAttackMin = null;
            else this.Search.StatAttackMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatAttackMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatAttackMax = null;
            else this.Search.StatAttackMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatDefenseMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatDefenseMin = null;
            else this.Search.StatDefenseMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }

    }
    private void SearchedStatDefenseMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatDefenseMax = null;
            else this.Search.StatDefenseMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }

    }
    private void SearchedStatSpecialAttackMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpecialAttackMin = null;
            else this.Search.StatSpecialAttackMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatSpecialAttackMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpecialAttackMax = null;
            else this.Search.StatSpecialAttackMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatSpecialDefenseMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpecialDefenseMin = null;
            else this.Search.StatSpecialDefenseMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatSpecialDefenseMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpecialDefenseMax = null;
            else this.Search.StatSpecialDefenseMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatSpeedMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpeedMin = null;
            else this.Search.StatSpeedMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
    private void SearchedStatSpeedMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ComboBoxItem selectedItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            string str = selectedItem.Content.ToString();
            if (str != null && str.Length == 0) {((TextBox)sender).BorderBrush = Brushes.Black; return;}
            if (str.Equals("Any")) this.Search.StatSpeedMax = null;
            else this.Search.StatSpeedMax = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
}