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
    
    public MainWindow()
    {
        InitializeComponent();
        this.currentColumnCount = 1;
        context = new PokemonDbContext();
    }

    private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        SearchGroup.Width = this.Width;
        SearchGroup.Height = this.Height;
        FilteredDataGroup.Width = this.Width - 280;
        SearchParametersGroup.Height = this.Height;
    }

    
}