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
        string str = selectedItem.Tag.ToString();
        if (str.Equals("Any")) this.Search.Generation = null;
        else this.Search.Generation = int.Parse(str.ToLower());
    }
    private void SearchedMoveTextChanged(object sender, TextChangedEventArgs e)
    {
        this.Search.Move = ((TextBox)sender).Text.ToLower();

    }
    private void SearchedAbilityTextChanged(object sender, TextChangedEventArgs e)
    {
        this.Search.Ability = ((TextBox)sender).Text.ToLower();

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
        else this.Search.AppearanceShape = str.ToLower().Replace(" ", "-");

    }
    private void SearchedAppearanceHeightMinChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceHeightMin = null;
                return;
            }
            else this.Search.AppearanceHeightMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
            throw new Exception("", ex);
        }
    }
    private void SearchedAppearanceHeightMaxChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceHeightMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceWeightMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0) {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.AppearanceWeightMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatHPMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatHPMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatAttackMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatAttackMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatDefenseMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatDefenseMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialAttackMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialAttackMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialDefenseMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpecialDefenseMax = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpeedMin = null;
                return;
            }
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
            string str = ((TextBox)sender).Text.ToString();
            if (str != null && str.Length == 0)
            {
                ((TextBox)sender).BorderBrush = Brushes.Black;
                this.Search.StatSpeedMin = null;
                return;
            }
            else this.Search.StatSpeedMin = int.Parse(str.ToLower());

            ((TextBox)sender).BorderBrush = Brushes.Black;
        }
        catch (Exception ex)
        {
            ((TextBox)sender).BorderBrush = Brushes.Red;
        }
    }
}