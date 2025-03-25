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
    public DatabaseInitHandler Handler {  get; private set; }

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
    }

    private void FetchGroupMouseDown(object sender, MouseButtonEventArgs e)
    {

    }
}