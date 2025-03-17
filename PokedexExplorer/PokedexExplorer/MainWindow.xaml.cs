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
    int currentColumnCount;
    PokemonDbContext context;

    DatabaseInitHandler databaseInitHandler;

    public MainWindow()
    {
        InitializeComponent();
        this.currentColumnCount = 1;
        context = new PokemonDbContext("skyre", "");
        context.Database.Migrate();

        //Add the init handler
        databaseInitHandler = new DatabaseInitHandler(this, this.context);
        //Run the init handler
        databaseInitHandler.Start();
    }

    public void NotifyInitProgressChanged()
    {
        //ItemProgressBar.Maximum = databaseInitHandler.ItemMax;
        //ItemProgressBar.Value = databaseInitHandler.ItemProgress;
        //TableProgressBar.Maximum = databaseInitHandler.TableMax;
        //TableProgressBar.Value = databaseInitHandler.TableProgress;
    }
}