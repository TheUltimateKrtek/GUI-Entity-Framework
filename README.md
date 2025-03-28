# Disclaimer
This project uses third-party sources. It is a student project created for educational purposes. It is not affiliated with, endorsed by, or associated with Nintendo, The Pokémon Company, or PokéAPI.
### Licensing Information
The Pokémon data in this project is sourced from PokéAPI, which is licensed under the BSD 3-Clause License. The full license can be found in the `LICENSE.txt` file.
### Sprites Attribution
Pokémon sprites in this project are retrieved at runtime from PokéAPI GitHub repository. These sprites are copyrighted material owned by Nintendo and are used here for educational purposes only.
# Overview
This project demonstrates the use of Entity Framework with PostgreSQL through a Pokémon filtering application. The project consists of two components:
- A data processing script that retrieves and converts data from PokéAPI into PostgreSQL tables.
- A filtering app that allows users to search and filter Pokémon by various attributes.
### The repository contains:
- The fully completed app
- A tutorial to make the app from scratch

# What is Entity Framework?
Entity Framework (EF) is an Object-Relational Mapper (ORM) for .NET applications. It simplifies database interactions by allowing developers to work with databases using C# objects instead of writing SQL queries manually. EF still allows to write SQL queries, so no functionality is lost.
### Why should you use Entity Framework?
Without EF, developers typically use ADO.NET, where they have to:
- Write SQL queries manually
- Manage database connections explicitly
- Handle data conversions between SQL and C#
### Key Features of EF
ORM Capabilities: Maps database tables to C# objects
LINQ Support: Queries can be written using LINQ instead of SQL
Migrations: Easily update the database schema as models change
Automatic Change Tracking: Keeps track of modifications to entities
Database Independence: Works with various databases like SQL Server, MySQL, PostgreSQL, etc.
# EF vs. EF Core
There are a few versions of Entity Framework. Let’s look at EF 6 and EF Core.
|                    | EF 6                 | EF Core                                     |
| ------------------ | -------------------- | ------------------------------------------- |
| Framework          | .NET                 | .NET & .NET Core                            |
| Cross-platform     | Yes                  | No                                          |
| Performancce       | Slower               | Faster                                      |
| Many-to-many       | Requires join tables | Native                                      |
| LINQ               | Less optimized       | Optimized                                   |
| Database providers | Mostly SQL servers   | Supports SQL server, PostgreSQL, MySQL etc. |
| Stored procedures  | Better support       | Still improving                             |
| Lazy Loading       | Yes                  | Yes                                         |

### Example code:
#### EF 6:
```csharp
using System.Data.Entity;
public class AppDbContext : DbContext
    public AppDbContext() : base("name=MyConnectionString") { }
    public DbSet<Product> Products { get; set; }
}
```

#### EF Core:
```csharp
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("Server=.;Database=MyDb;Trusted_Connection=True;");
    }
    public DbSet<Product> Products { get; set; }
}
```

# Getting started
For this tutorial, we’ll be using PostgreSQL for the database, PokéAPI as the data source, and Entity Framework 6. Let’s begin with the less important steps.
## Setting up a PostgreSQL server
*(Note: The Linux tutorial for setting up PostgreSQL was generated with the help of an AI model. While every effort was made to ensure accuracy, AI-generated content may occasionally contain errors. We recommend reviewing the [official PostgreSQL documentation](https://www.postgresql.org/docs/) or trusted resources for additional confirmation and guidance.)*
### For Windows

##### Download PostgreSQL

Download PostgreSQL from [official sources](https://www.postgresql.org/download/). Choose the appropriate version for your Windows operating system and download the installer.

##### Install PostgreSQL

Follow the installation steps provided by the installer. During the installation process, make sure to note the installation path (e.g., `C:\Program Files\PostgreSQL\<version>`).

The default login credentials are:

Username: postgres

Password: postgres (or the password you choose during the installation)

##### Post-Installation

PostgreSQL should start automatically as a service after installation. However, you can also start or stop it manually via the command line:

##### Start the PostgreSQL service

In the Command Prompt (navigate to the PostgreSQL bin directory):

`pg_ctl start -D <your_database_cluster_path>`
##### Stop the PostgreSQL service
To stop the PostgreSQL server, use:

`pg_ctl stop -D <your_database_cluster_path>`
##### Creating a New Database
To create a new database, you need to specify the name of the database during initialization. Run the following command from the PostgreSQL bin directory:

`initdb -D <your_database_cluster_path>`
### For Ubuntu/Debian
##### Install PostgreSQL
Open a terminal and run the following commands to install PostgreSQL:

`sudo apt update`

`sudo apt install postgresql postgresql-contrib`
This will install PostgreSQL and some useful extensions.
##### Post-Installation
PostgreSQL should start automatically after installation. If you need to manually start it or check its status:
##### Start PostgreSQL service

`sudo systemctl start postgresql`

##### Stop PostgreSQL service

`sudo systemctl stop postgresql`

##### Check status

`sudo systemctl status postgresql`

##### Create a New Database

PostgreSQL is already initialized, but if you need to create a new database, you can do so with the following steps:

##### Switch to the postgres user

`sudo -i -u postgres`

##### Use the psql tool to access PostgreSQL

`psql`

##### Once inside the psql shell, create your new database:

`CREATE DATABASE mydatabase;`

Replace mydatabase with your preferred name for the database.

### For CentOS/RHEL/Fedora

##### Install PostgreSQL

For CentOS or RHEL, the installation process differs slightly. Run the following commands:

`sudo yum install postgresql-server postgresql-contrib`

On Fedora, use the dnf package manager:

`sudo dnf install postgresql-server postgresql-contrib`

##### Post-Installation

Before starting PostgreSQL for the first time, initialize the database:

`sudo postgresql-setup initdb`

##### Start PostgreSQL

After initialization, start the PostgreSQL service:

`sudo systemctl start postgresql`

##### Stop PostgreSQL service

`sudo systemctl stop postgresql`

##### Create a New Database

PostgreSQL is already initialized, but if you need to create a new database, you can do so with the following steps:

##### Switch to the postgres user

`sudo -i -u postgres`

##### Use the psql tool to access PostgreSQL

`psql`

##### Once inside the psql shell, create your new database:

`CREATE DATABASE mydatabase;`

Replace mydatabase with your preferred name for the database.


### Setting up a WPF and EF project
1. Open Visual Studio (or install it with the WPF extension).
2. Create a new WPF project. We will name it **PokedexExplorer**.
3. Install the Npgsql and Entity Framework libraries using the NuGet Package Manager.
4. In the Solution Explorer, under PokedexExplorer, create a new folder **Models**. We will write our tables here. Next, create a folder **Data**. We will put all our data-handling classes there. This step is not required, but it helps keep the project clean. Our tutorial will assume this step was taken.

# Code-First vs. Database-First Approach
Object-Relational Mappers (ORMs) provide two common approaches for managing the relationship between your application code and the database: code-first and database-first.
### Code-First Approach
Definition: In the code-first approach, you define the database structure (tables, relationships, etc.) in your application code using classes and annotations. The ORM tool generates the database schema based on this code.

Use Case: This is ideal for new projects where the database doesn't exist yet, or when the focus is on designing the application's business logic first.

Example: Define a Pokemon class in code, and the ORM generates a corresponding Pokemon table in the database.
### Database-First Approach
Definition: In the database-first approach, you start with an existing database schema. The ORM generates the necessary application code (e.g., classes) to map the database tables to objects in the application.

Use Case: This is suitable when working with a legacy database or when the database schema is predefined and cannot be modified significantly.

Example: The ORM reads an existing `Pokemon` table and generates a corresponding `Pokemon` class for use in the application.

### For this project…

This project demonstrates the use of the code-first approach in Object-Relational Mapping (ORM). In this approach, the database schema is defined programmatically in the application code, allowing for easier schema management and integration with the application's business logic. 

If, however, an existing database is available and already populated with data, the database-first approach can be used. In this case, the database schema is imported into the application, and no tables or data need to be created or populated from scratch. The filtering app can seamlessly connect to and interact with the existing database.

# Creating a DbContext subclass
This is the most crucial part. You need to connect to a database to start queries.
### Connection string
**A connection string** is a string used to specify how to connect to a database. It contains various pieces of information that the application needs to establish a connection, including the server's address, the database name, authentication details, and additional options:

`Host=<server_address>;Port=<port>;Username=<user>;Password=<password>;Database=<database_name>;`

We’ll be using the default options:

`Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;`

### DbContext class

This class is used as a connection to the database. We will be referencing it a lot, whenever we try to interact with the database.

```csharp
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;");
        }
    }
}
```

# Defining tables
*(Note: This section assumes you are using the code-first approach. If a database exists, you need only to match its data types and the columns within the classes.)*

First of all, we need to define our tables. This is done by creating a class that matches what we want the table to look like.
### Annotations

First, let’s familiarize ourselves with some annotations.

#### [Key]
The Key annotation is used for defining a primary key.

#### [ForeignKey(“Table”)]
The ForeignKey annotation is used to reference a table by its primary key. The string specifies which table is referenced.

#### [Required]
The Required annotation is used to specify non-null values.

### Tables
For this tutorial, we want to use the following tables. We will also add the references to them in the PokemonDbContext class, but that will be explained in a later section. We will also provide articles about the Pokémon mechanics in the Pokémon games, but they are not necessary to understand for this tutorial. 
#### Ability
The Ability table is a simple table that holds data about the Pokémon abilities.
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Ability.Name), IsUnique = false, Name = "IndexAbilityName")]
    [Index(nameof(Ability.Generation), IsUnique = false, Name = "IndexAbilityGeneration")]
    public class Ability
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Effect { get; set; }
        public string? ShortEffect { get; set; }
        public string? Description { get; set; }
        public int? Generation { get; set; }
    }
}
```
#### Move
The Move table contains a list of moves that a Pokémon can perform.
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Move.Name), IsUnique = false, Name = "IndexMoveName")]
    public class Move
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int? Accuracy { get; set; }
        public string? DamageClass { get; set; }
        public int? EfectChance { get; set; }
        public int? Generation { get; set; }
        public string? Ailment { get; set; }
        public int? AilmentChance { get; set; }
        public int? CritRate { get; set; }
        public int? Drain { get; set; }
        public int? FlinchChance { get; set; }
        public int? Healing { get; set; }
        public int? MaxHits { get; set; }
        public int? MaxTurns { get; set; }
        public int? MinHits { get; set; }
        public int? MinTurns { get; set; }
        public int? StatChance { get; set; }
        public int? Power { get; set; }
        [Required]
        public int PP { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public string Target { get; set; }
        [Required]
        public string Type { get; set; }
        public string? Description { get; set; }
    }
}
```
#### Pokemon
The Pokémon table contains information about the various Pokémon.
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Pokemon.ID), IsUnique = true, Name = "IndexPokemonID")]
    [Index(nameof(Pokemon.Name), IsUnique = false, Name = "IndexPokemonName")]
    [Index(nameof(Pokemon.Height), IsUnique = false, Name = "IndexPokemonHeight")]
    [Index(nameof(Pokemon.Weight), IsUnique = false, Name = "IndexPokemonWeight")]
    [Index(nameof(Pokemon.HP), IsUnique = false, Name = "IndexPokemonHp")]
    [Index(nameof(Pokemon.Attack), IsUnique = false, Name = "IndexPokemonAttack")]
    [Index(nameof(Pokemon.Defense), IsUnique = false, Name = "IndexPokemonDefense")]
    [Index(nameof(Pokemon.SpecialAttack), IsUnique = false, Name = "IndexPokemonSpecialAttack")]
    [Index(nameof(Pokemon.SpecialDefense), IsUnique = false, Name = "IndexPokemonSpecialDefense")]
    [Index(nameof(Pokemon.Speed), IsUnique = false, Name = "IndexPokemonSpeed")]
    public class Pokemon
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public int BaseExperience { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public int Order { get; set; }
        [ForeignKey("Ability")]
        public int? PrimaryAbility { get; set; }
        [ForeignKey("Ability")]
        public int? SecondaryAbility { get; set; }
        [ForeignKey("Ability")]
        public int? HiddenAbility { get; set; }
        [ForeignKey("PokemonSpecies")]
        [Required]
        public int Species { get; set; }
        [Required]
        public int HP { get; set; }
        [Required]
        public int HPEffort { get; set; }
        [Required]
        public int Attack { get; set; }
        [Required]
        public int AttackEffort { get; set; }
        [Required]
        public int Defense { get; set; }
        [Required]
        public int DefenseEffort { get; set; }
        [Required]
        public int SpecialAttack { get; set; }
        [Required]
        public int SpecialAttackEffort { get; set; }
        [Required]
        public int SpecialDefense { get; set; }
        [Required]
        public int SpecialDefenseEffort { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int SpeedEffort { get; set; }
        public string? SpriteFrontDefault { get; set; }
        public string? SpriteFrontFemale { get; set; }
        public string? SpriteFrontShinyFemale { get; set; }
        public string? SpriteFrontShiny { get; set; }
        public string? SpriteBackDefault { get; set; }
        public string? SpriteBackFemale { get; set; }
        public string? SpriteBackShinyFemale { get; set; }
        public string? SpriteBackShiny { get; set; }
        public string? Cry { get; set; }
        public string? CryLegacy { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
    }
}

```
#### PokemonSpecies
The PokemonSpecies table contains information about the Pokémon species. Note, that a species may contain multiple pokémon. An obvious example is Pikachu with its various versions, each having different attributes and stats.
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(PokemonSpecies.Name), IsUnique = false, Name = "IndexPokemonName")]
    [Index(nameof(PokemonSpecies.Generation), IsUnique = false, Name = "IndexPokemonSpeciesGeneration")]
    public class PokemonSpecies
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public int BaseHappiness { get; set; }
        [Required]
        public int CaptureRate { get; set; }
        [Required]
        public int GenderRate { get; set; }
        public int? HatchCounter { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int Generation { get; set; }
        [Required]
        public int? NationalPokedexNumber { get; set; }
        [Required]
        public bool IsBaby { get; set; }
        [Required]
        public bool IsLegendary { get; set; }
        [Required]
        public bool IsMythical { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string GrowthRate { get; set; }
        [Required]
        public string Habitat { get; set; }
        [Required]
        public string Shape { get; set; }
        [Required]
        public string Genera { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
```
#### EvolutionChain
The EvolutionChain table includes information about a Pokémon’s evolution chain. Pokémon can evolve into various Pokémon, but a Pokémon can only evolve from one other Pokémon. Because of this, the primary key will be the EvolvesTo column.
```csharp
namespace PokedexExplorer.Model
{
    public class EvolutionChain
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [ForeignKey("Pokemon")]
        [Required]
        public int EvolvesFrom { get; set; }
        [ForeignKey("Pokemon")]
        [Required]
        public int EvolvesTo { get; set; }
        public int? Gender { get; set; }
        public int? MinBeauty { get; set; }
        public int? MinHappiness { get; set; }
        public int? MinLevel { get; set; }
        [ForeignKey("Pokemon")]
        public int? TradeSpecies { get; set; }
        public int? RelativePhysicalStats { get; set; }
        public string? Item { get; set; }
        public string? HeldItem { get; set; }
        [ForeignKey("Move")]
        public int? KnownMove { get; set; }
        public string? KnownMoveType { get; set; }
        public string? Trigger { get; set; }
        [ForeignKey("Pokemon")]
        public int? PartySpecies { get; set; }
        public string? PartyType { get; set; }
        public string? TimeOfDay { get; set; }
        public bool? NeedsOverworldRain { get; set; }
        public bool? TurnUpsideDown { get; set; }
    }
}
```
#### PokemonMove
This table represents our many-to-many relation between a Pokémon and a move it can learn. It will also contain additional information about the way a Pokémon can learn a move. This table connects the Pokemon and the Move tables.
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(PokemonMove.Pokemon), IsUnique = false, Name = "IndexPokemonMovePokemon")]
    [Index(nameof(PokemonMove.Move), IsUnique = false, Name = "IndexPokemonMoveMove")]
    public class PokemonMove
    {
        [Key]
        [Required]
        public int ID { get; set; }
        [Required]
        public int Pokemon { get; set; }
        [Required]
        public int Move { get; set; }
        public int? LevelLearnedAt { get; set; }
        public string? LearnMethod { get; set; }
    }
}
```
### Indexes
For the purpose of searching, indexing columns will be beneficial. It will speed up search. For example, if we were to search by ability name, it would make sense to use indexing for faster searching. We can add an annotation ```[Index(nameof(Ability.Name), IsUnique = true, Name = "IndexAbilityName")]```to the class.

#### Ability
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Ability.Name), IsUnique = false, Name = "IndexAbilityName")]
    [Index(nameof(Ability.Generation), IsUnique = false, Name = "IndexAbilityGeneration")]
    public class Ability
    {
        //Code...
    }
}
```

#### Move
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Move.Name), IsUnique = true, Name = "IndexMoveName")]
    public class Move
    {
        //Code...
    }
}
```

#### Pokemon
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Pokemon.ID), IsUnique = true, Name = "IndexPokemonID")]
    [Index(nameof(Pokemon.Name), IsUnique = false, Name = "IndexPokemonName")]
    [Index(nameof(Pokemon.Height), IsUnique = false, Name = "IndexPokemonHeight")]
    [Index(nameof(Pokemon.Weight), IsUnique = false, Name = "IndexPokemonWeight")]
    [Index(nameof(Pokemon.HP), IsUnique = false, Name = "IndexPokemonHp")]
    [Index(nameof(Pokemon.Attack), IsUnique = false, Name = "IndexPokemonAttack")]
    [Index(nameof(Pokemon.Defense), IsUnique = false, Name = "IndexPokemonDefense")]
    [Index(nameof(Pokemon.SpecialAttack), IsUnique = false, Name = "IndexPokemonSpecialAttack")]
    [Index(nameof(Pokemon.SpecialDefense), IsUnique = false, Name = "IndexPokemonSpecialDefense")]
    [Index(nameof(Pokemon.Speed), IsUnique = false, Name = "IndexPokemonSpeed")]
    public class Pokemon
    {
        //Code...
    }
}
```

#### PokemonSpecies
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(PokemonSpecies.Name), IsUnique = false, Name = "IndexPokemonName")]
    [Index(nameof(PokemonSpecies.Generation), IsUnique = false, Name = "IndexPokemonSpeciesGeneration")]
    public class PokemonSpecies
    {
        //Code...
    }
}

```

#### PokemonMove
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(PokemonMove.Pokemon), IsUnique = false, Name = "IndexPokemonMovePokemon")]
    [Index(nameof(PokemonMove.Move), IsUnique = false, Name = "IndexPokemonMoveMove")]
    public class PokemonMove
    {
        //Code...
    }
}
```

### Updating the PokemonDbContext class
Now, that we have our classes, we have to update the PokemonDbContext class. Be careful, as foreign keys require the referenced table to be created first. Because of this, we will be creating these tables in the following order:
- Ability
- Move
- PokemonSpecies
- Pokemon (references PokemonSpecies and Ability)
- EvolutionChain (references Pokemon)
- PokmeonMove (references Pokemon and Move)

```
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PokedexExplorer.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PokemonDbContext : DbContext
{
    public PokemonDbContext() : base()
    {

    }
    public DbSet<Ability> Ability { get; set; }
    public DbSet<Move> Move { get; set; }
    public DbSet<Pokemon> Pokemon { get; set; }
    public DbSet<PokemonSpecies> PokemonSpecies { get; set; }
    public DbSet<PokemonMove> PokemonMove { get; set; }
    public DbSet<EvolutionChain> EvolutionChain { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;");
    }
}
```

### Creating the database
Now, we will need to create the actual database on the server. So far, we have only modeled the schemas.
#### Migrate
To synchronize our database model with Postgre, we can use the method `DbContext.Database.Migrate();`. This would update our tables. The `Migrate()` method handles existing tables, however it will throw exceptions if the existing table is different.
```csharp
public MainWindow()
{
    InitializeComponent();
    context = new PokemonDbContext("skyre", "");

    context.Database.Migrate();
}
```
#### Raw SQL
We can also generate and execute SQL commands. This is done like so:
```csharp
public MainWindow()
{
    InitializeComponent();
    context = new PokemonDbContext("skyre", "");

    try
    {
        context.Database.ExecuteSqlRaw(context.Database.GenerateCreateScript());
    }
    catch { }
}
```
#### MainWindow
In our MainWindow class, created at WPF initialization, we will add the following code. This code runs at startup.
```csharp
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
    PokemonDbContext context;
    
    public MainWindow()
    {
        InitializeComponent();
        context = new PokemonDbContext();
        try
        {
            context.Database.ExecuteSqlRaw(context.Database.GenerateCreateScript());
        }
        catch { }
    }
}
```

# Retrieving data from PokéAPI
*(Note: This section assumes you’re using the code-first approach. This section is not an important part in our tutorial, so you can just copy-paste all the code. This code will be slow to run. We also advise not to run this too often, as the code contained in this section connects to a third-party server. We are not trying to cause the PokéAPI team any problems.)*
PokéAPI uses a NoSQL-type database. We will need to reformat it to match our PostgreSQL table structure. Now that we have our tables defined, we will create a dedicated class for retrieving data and reformatting it. We will use our classes for output.
### Be careful
Be careful, as the PostgreSQL database requires a primary key of one table to exist before another table can reference it. Because of this, we will keep it simple and not use threading.
### Checking if you’re correct
We will explain how to retrieve data in later sections. For now, to see your code in action, you can use tools like pgAdmin, which comes bundled with PostgreSQL.
## Class structure
We will split this class into two parts:
- Retrieving data from PokéAPI
- Processing and reformatting data

### PokeAPIFetcher
We will create a class `PokeAPIFetcher` in the `Data` folder, which will fetch and process data and return objects in the form of our defined classes model. This part of the code is not important for our tutorial, so you can just copy-paste the final code. We will explain it anyways.

#### Retrieving data
PokéAPI uses a JSON format with a NoSQL-type database. This format is good for storing the complex data Pokémon has. However, we will process it and drop data unimportant to us. We also want to show how to add data to our database.

##### Retrieving a JSON object
We will use this simple method to retrieve a JSON file. This uses PokéAPI's folder structure: `https://pokeapi.co/api/v2/<table>/[<id>]`

```csharp
static public JObject RetrieveJSON(string name, int? id = null)
{
    string url = "https://pokeapi.co/api/v2/" + name + "/";
    if (id != null) url += id + "/";
    else url += "?limit = 100000";

    using (HttpClient client = new HttpClient())
    {
        try
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            Console.WriteLine($"Status Code: {response.StatusCode}");
            response.EnsureSuccessStatusCode();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            return JObject.Parse(jsonResponse);
        }
        catch (HttpRequestException e)
        {
            Debug.WriteLine("HTTP Error: " + name + (id == null ? "" : " " + id));
            return null;
        }
    }
}
```

##### Get source ID
Method `GetURLIntValue` will simply retrieve an index from a url.
```csharp
static private int GetURLIntValue(string url)
{
    string[] split = url.Split('/');
    return int.Parse(split[split.Length - 1]);
}
```

##### Retrieving entries
Using the method from before, we can start with processing data. First, we want to find the total number of entries.

```csharp
static public List<int> GetEntries(string name)
{
    JObject json = RetrieveJSON(name);
    if (json == null) return [];
    List<int> entries = [];
    foreach (JToken t in json["results"])
    {
        entries.Add((int)GetURLIntValue(t["url"].ToString()));
    }
    return entries;
}
```

#### Processing data
Next, we will add methods to parse JSON data

Method GetEnglishNode will iterate through a language structure and return an english version.
```csharp
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
```

##### Ability
```csharp
static public Ability ParseAbility(JObject node)
{
    if (node == null) return null;
    string at = "";
    try
    {
        int id = node["id"]?.ToObject<int>() ?? - 1;
        int generation = GetURLIntValue(node["generation"]["url"].ToString()) ?? 0;
        
        at = "effect_entries: " + node["effect_entries"];
        JObject effectNode = GetEnglishNode(node["effect_entries"]?.ToObject<JArray>() ?? null);
        string effect = "No effect description.";
        string shortEffect = "No short effect description.";
        if (effectNode != null)
        {
            effect = effectNode["effect"]?.ToString() ?? "No effect description.";
            shortEffect = effectNode["short_effect"]?.ToString() ?? "No short effect description.";
        }

        at = "flavor_text_entries: " + node["flavor_text_entries"];
        JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
        string description = "No description.";
        if (descriptionNode != null)
        {
            description = descriptionNode["flavor_text"].ToObject<string>();
        }

        at = "names: " + node["names"];
        JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
        string name = node["name"]?.ToObject<string>() ?? "<unknown>";
        if (nameNode != null)
        {
            name = nameNode["name"].ToObject<string>();
        }

        Ability ability = new Ability();
        ability.ID = id;
        ability.Name = name;
        ability.Generation = generation;
        ability.Effect = effect;
        ability.ShortEffect = shortEffect;
        ability.Description = description;
        return ability;
    }
    catch (Exception e)
    {
        throw new Exception(at, e);
        return null;
    }
}
```

##### Move
```csharp
static public Move ParseMove(JObject node)
{
    if (node == null) return null;

    int? accuracy = node["accuracy"]?.ToObject<int?>() ?? null;
    string? damageClass = node["damage_class"]["name"].ToObject<string?>();
    int? effectChance = node["effectChance"]?.ToObject<int?>() ?? null;
    int? generation = GetURLIntValue(node["generation"]["url"]?.ToObject<string?>() ?? null);
    int id = node["id"]?.ToObject<int>() ?? -1;

    string? ailment = node["meta"] == null ? null : (node["meta"]["ailment"] == null ? null : node["meta"]["ailment"]["name"].ToObject<string>());
    int? ailmentChance = node["meta"] == null ? null : node["meta"]["ailment_chance"].ToObject<int?>();
    int? critRate = node["meta"] == null ? null : node["meta"]["crit_rate"].ToObject<int?>();
    int? drain = node["meta"] == null ? null : node["meta"]["drain"].ToObject<int?>();
    int? flinchChance = node["meta"] == null ? null : node["meta"]["flinch_chance"].ToObject<int?>();
    int? healing = node["meta"] == null ? null : node["meta"]["healing"].ToObject<int?>();
    int? maxHits = node["meta"] == null ? null : node["meta"]["max_hits"].ToObject<int?>();
    int? maxTurns = node["meta"] == null ? null : node["meta"]["max_turns"].ToObject<int?>();
    int? minHits = node["meta"] == null ? null : node["meta"]["min_hits"].ToObject<int?>();
    int? minTurns = node["meta"] == null ? null : node["meta"]["min_turns"].ToObject<int?>();
    int? statChance = node["meta"] == null ? null : node["meta"]["stat_chance"].ToObject<int?>();

    JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
    string name = node["name"]?.ToObject<string>() ?? "<unknown>";
    if (nameNode != null)
    {
        name = nameNode["name"]?.ToObject<string>() ?? "<unknown>";
    }

    int? power = node["power"]?.ToObject<int?>() ?? null;
    int pp = node["pp"]?.ToObject<int>() ?? -1;
    int priority = node["priority"]?.ToObject<int>() ?? -1;
    string target = node["target"]?["name"]?.ToObject<string>() ?? null;
    string type = node["type"]?["name"]?.ToObject<string>() ?? "normal";

    JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
    string description = null;
    if (descriptionNode != null)
    {
        description = descriptionNode["flavor_text"]?.ToObject<string>() ?? null;
    }

    Move move = new Move();
    move.ID = id;
    move.Name = name;
    move.PP = pp;
    move.Priority = priority;
    move.Target = target;
    move.Type = type;
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
```

##### PokemonSpecies
```csharp
static public PokemonSpecies ParsePokemonSpecies(JObject node)
{
    if (node == null) return null;

    int baseHappiness = node["base_happiness"]?.ToObject<int?>() ?? -1;
    int captureRate = node["capture_rate"]?.ToObject<int?>() ?? -1;
    int genderRate = node["gender_rate"]?.ToObject<int?>() ?? -1;
    int? hatchCounter = node["hatch_counter"]?.ToObject<int?>() ?? null;
    int id = node["id"]?.ToObject<int?>() ?? -1;
    int order = node["order"]?.ToObject<int?>() ?? -1;
    bool isBaby = node["is_baby"].ToObject<bool>();
    bool isLegendary = node["is_legendary"].ToObject<bool>();
    bool isMythical = node["is_mythical"].ToObject<bool>();
    string color = null;
    if (node["color"] != null && node["color"] is JObject)
        color = node["color"]?["name"]?.ToObject<string>() ?? null;
    string growthRate = null;
    if (node["growth_rate"] != null && node["growth_rate"] is JObject)
        growthRate = node["growth_rate"]?["name"]?.ToObject<string>() ?? null;
    string habitat = "none";
    if (node["habitat"] != null && node["habitat"] is JObject)
        habitat = node["habitat"]?["name"]?.ToObject<string>() ?? "none";
    string shape = null;
    if (node["shape"] != null && node["shape"] is JObject)
        shape = node["shape"]?["name"]?.ToObject<string>() ?? null;
    int generation = -1;
    if (node["trade_spgenerationecies"] != null && node["generation"] is JObject)
        generation = (int)GetURLIntValue(node["generation"]?["url"]?.ToObject<string>() ?? null);

    JObject generaNode = GetEnglishNode(node["genera"]?.ToObject<JArray>() ?? null);
    string genera = "";
    if (generaNode != null)
    {
        genera = generaNode["genus"].ToObject<string>();
    }
    genera = genera.Replace(" Pokémon", "");

    int nationalPokedexNumber = -1;
    foreach (JToken t in node["pokedex_numbers"])
    {
        if (t["pokedex"]["name"].Equals("national"))
        {
            nationalPokedexNumber = (int)GetURLIntValue(t["url"].ToString());
        }
    }

    JObject nameNode = GetEnglishNode(node["names"]?.ToObject<JArray>() ?? null);
    string name = node["name"].ToObject<string>();
    if (nameNode != null)
    {
        name = nameNode["name"].ToObject<string>();
    }
    name = name.Replace("♀", "(female)").Replace("♂", "(male)");

    JObject descriptionNode = GetEnglishNode(node["flavor_text_entries"]?.ToObject<JArray>() ?? null);
    string? description = null;
    if (descriptionNode != null)
    {
        description = descriptionNode["flavor_text"].ToObject<string>();
    }
    description = description.Replace("\u2212", "-");

    PokemonSpecies species = new PokemonSpecies();
    species.ID = id;
    species.Name = name;
    species.BaseHappiness = baseHappiness;
    species.CaptureRate = captureRate;
    species.GenderRate = genderRate;
    species.Order = order;
    species.Generation = generation;
    species.NationalPokedexNumber = nationalPokedexNumber;
    species.IsBaby = isBaby;
    species.IsLegendary = isLegendary;
    species.IsMythical = isMythical;
    species.Color = color;
    species.GrowthRate = growthRate;
    species.Habitat = habitat;
    species.Shape = shape;
    species.Genera = genera;
    species.Description = description;
    species.HatchCounter = hatchCounter;
    return species;
}
```

##### Pokemon
```csharp
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
    int? primaryAbility = abilities[0];
    int? secondaryAbility = abilities[1];
    int? hiddenAbility = abilities[2];

    int baseExperience = node["base_experience"].GetValue<int>();
    int height = node["height"].GetValue<int>();
    int weight = node["weight"].GetValue<int>();
    int id = node["id"].GetValue<int>();
    int order = node["order"].GetValue<int>();
    string name = node["name"].GetValue<string>();

    string spriteFrontDefault = node["sprite_front_default"].GetValue<string>();
    string? spriteFrontFemale = node["sprite_front_female"].GetValue<string?>();
    string? spriteFrontShiny = node["sprite_front_shiny"].GetValue<string?>();
    string? spriteFrontShinyFemale = node["sprite_front_shiny_female"].GetValue<string?>();
    string? spriteBackDefault = node["sprite_back_default"].GetValue<string?>();
    string? spriteBackFemale = node["sprite_back_female"].GetValue<string?>();
    string? spriteBackShiny = node["sprite_back_shiny"].GetValue<string?>();
    string? spriteBackShinyFemale = node["sprite_back_shiny_female"].GetValue<string?>();

    int species = node["species"].GetValue<int>();

    string? cry = node["cry"].GetValue<string?>();
    string? cryLegacy = node["cry"].GetValue<string?>();

    int hp = node["stats"][0]["base_stat"].GetValue<int>();
    int hpEffort = node["stats"][0]["effort"].GetValue<int>();
    int attack = node["stats"][1]["base_stat"].GetValue<int>();
    int attackEffort = node["stats"][1]["effort"].GetValue<int>();
    int defense = node["stats"][2]["base_stat"].GetValue<int>();
    int defenseEffort = node["stats"][2]["effort"].GetValue<int>();
    int specialAttack = node["stats"][3]["base_stat"].GetValue<int>();
    int specialAttackEffort = node["stats"][3]["effort"].GetValue<int>();
    int specialDefense = node["stats"][4]["base_stat"].GetValue<int>();
    int specialDefenseEffort = node["stats"][4]["effort"].GetValue<int>();
    int speed = node["stats"][5]["base_stat"].GetValue<int>();
    int speedEffort = node["stats"][5]["effort"].GetValue<int>();

    string primaryType = node["types"][0].GetValue<string>();
    string? secondaryType = node["types"].AsArray().Count == 1 ? null : node["types"][1].GetValue<string?>();

    Pokemon pokemon = new Pokemon(id, baseExperience, height, weight, order, species, hp, hpEffort, attack, attackEffort,
        defense, defenseEffort, specialAttack, specialAttackEffort, specialDefense, specialDefenseEffort, speed, speedEffort, spriteFrontDefault, name, primaryType);
    pokemon.PrimaryAbility = primaryAbility;
    pokemon.SecondaryAbility = secondaryAbility;
    pokemon.HiddenAbility = hiddenAbility;
    pokemon.SpriteFrontFemale = spriteFrontFemale;
    pokemon.SpriteFrontShiny = spriteFrontShiny;
    pokemon.SpriteFrontShinyFemale = spriteFrontShinyFemale;
    pokemon.SpriteBackDefault = spriteBackDefault;
    pokemon.SpriteBackFemale = spriteBackFemale;
    pokemon.SpriteBackShiny = spriteBackShiny;
    pokemon.SpriteBackShinyFemale = spriteBackShinyFemale;
    pokemon.Cry = cry;
    pokemon.CryLegacy = cryLegacy;
    pokemon.SecondaryType = secondaryType;
    return pokemon;
}
```

##### EvolutionChain
```csharp
static public List<EvolutionChain> ParseEvolutionChain(JsonNode node, List<EvolutionChain> list = null)
{
    if (list == null)
    {
        list = new List<EvolutionChain>();
        ParseEvolutionChain(node["chain"], list);
        return list;
    }

    foreach (JsonNode evolution in node["evolves_to"].AsArray())
    {
        foreach (JsonNode details in node["evolution_details"].AsArray())
        {
            int from = (int)GetURLIntValue(node["species"]["url"].GetValue<string>());
            int to = (int)GetURLIntValue(evolution["species"]["url"].GetValue<string>());

            int id = -1;

            EvolutionChain chain = new EvolutionChain(id, from, to);

            chain.Gender = details["gender"].GetValue<int?>();
            chain.MinBeauty = details["min_beauty"].GetValue<int?>();
            chain.MinHappiness = details["min_happiness"].GetValue<int?>();
            chain.MinLevel = details["min_level"].GetValue<int?>();
            chain.TradeSpecies = GetURLIntValue(details["trade_species"].GetValue<string?>());
            chain.RelativePhysicalStats = details["relative_physical_stats"].GetValue<int?>();
            chain.Item = details["item"].GetValue<string?>();
            chain.HeldItem = details["helpItem"].GetValue<string?>();
            chain.KnownMove = GetURLIntValue(details["known_move"].GetValue<string?>());
            chain.KnownMoveType = details["known_move_type"].GetValue<string?>();
            chain.Trigger = details["trigger"].GetValue<string?>();
            chain.PartySpecies = GetURLIntValue(details["party_species"].GetValue<string?>());
            chain.PartyType = details["party_type"].GetValue<string?>();
            chain.TimeOfDay = details["time_of_day"].GetValue<string?>();
            chain.NeedsOverworldRain = details["needs_overworld_rain"].GetValue<bool?>();
            chain.TurnUpsideDown = details["turn_upside_down"].GetValue<bool?>();

            list.Add(chain);
        }

        ParseEvolutionChain(node, list);
    }
    
    return list;
}
```

##### PokemonMove
```csharp
static public List<PokemonMove> ParsePokemonMove(JsonNode pokemonJson)
{
    List<PokemonMove> list = new List<PokemonMove>();

    int pokemon = pokemonJson["id"].GetValue<int>();
    foreach (JsonNode m in pokemonJson["moves"].AsArray())
    {
        int index = m["version_group_details"].AsArray().Count() - 1;
        int move = (int)GetURLIntValue(m["move"]["url"].GetValue<string>());
        int? levelLearnedAt = m["version_group_details"][index]["level_learned_at"].GetValue<int>();
        string? learnMethod = m["version_group_details"][index]["learn_method"]["name"].GetValue<string>();

        PokemonMove pm = new PokemonMove(index, pokemon, move);
        pm.LearnMethod = learnMethod;
        pm.LevelLearnedAt = levelLearnedAt;

        list.Add(pm);
    }
    return list;
}
```

# Populating the database
Now that we have our data, we can start populating the database. We will use our next class, `DatabaseInitHandler`. We will ceate a thread-like class for asynchronous download and proccessing.
```csharp
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokedexExplorer.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace PokedexExplorer.Data
{
    class DatabaseInitHandler
    {
        private PokemonDbContext context;
        private Thread thread;
        private int tableProgress, tableMax, itemProgress, itemMax;

        public DatabaseInitHandler(PokemonDbContext context) {
            thread = new Thread(Run);
            this.context = context;
        }

        public void Start()
        {
            thread.Start();
        }

        private void Run()
        {
            //Put data fetching code here...
        }
    }
}

```

#### Insert
Inserting an entry to our table is straight-forward. All we need is an object and a table to insert it to. We will fill in the `Run()` method.

Inserting an entry is done by `context.Table.Add(entry);`. As an example, we can insert an ability in the Ability table with `context.Ability.Add(ability);`. We will also make use of the `AddRange(List<T>)` method, which adds multiple values at once. These changes only happen in our "# environment, so we will need to apply them using `context.SaveChanges();`.

##### Get entry ids
```csharp
List<int> abilityIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> moveIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> pokemonIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> pokemonSpeciesIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> evolutionChainIndexes = PokeAPIFetcher.GetEntries("ability");
```

##### Ability, Move and PokemonSpecies
Populating the Ability, Move and PokemonSpecies tables is simple. Every entry is created by a single request to the PokeAPIFetcher class. We will simply request the object, and if it exists, we will simply insesrt it. Finally, we will save the changes, so that we can be confident future entries can reference these entries.
```csharp
//Ability
foreach (int id in abilityIndexes)
{
    Ability ability = PokeAPIFetcher.ParseAbility(PokeAPIFetcher.RetrieveJSON("ability", id));
    if (ability != null) this.context.Ability.Add(ability);
    this.ItemProgress++;
}

//Move
foreach (int id in moveIndexes)
{
    Move move = PokeAPIFetcher.ParseMove(PokeAPIFetcher.RetrieveJSON("move", id));
    if (move != null) this.context.Move.Add(move);
    ItemProgress++;
}

//PokemonSpecies
foreach (int id in pokemonSpeciesIndexes)
{
    PokemonSpecies pokemonSpecies = PokeAPIFetcher.ParsePokemonSpecies(PokeAPIFetcher.RetrieveJSON("pokemon-species", id));
    if (pokemonSpecies != null) this.context.PokemonSpecies.Add(pokemonSpecies);
    ItemProgress++;
}

//Save changes
this.context.SaveChanges();
```

##### Pokemon and PokemonMove
Pokemon and PokemonMove tables are both created from the `pokemon` PokéAPI table. We will create at the same time, however we will add `PokemonMove` entries after `Pokemon` entries, because `Pokemon` references `Pokemon`. `PokemonMove` also references `Move`, but all `Move` entries have already been inserted.

As for `PokemonMove`, we need to identify them by different IDs, because the two tables don't have the same number of entries. That's what the `pokemonMoveIndex` variable is for.

```csharp
//Pokemon
int pokemonMoveIndex = 1;
List<PokemonMove> storedPokemonMoves = new List<PokemonMove>();
foreach (int id in pokemonIndexes)
{
    JObject node = PokeAPIFetcher.RetrieveJSON("pokemon", id);
    Pokemon pokemon= PokeAPIFetcher.ParsePokemon(node);
    List<PokemonMove> pokemonMoves = PokeAPIFetcher.ParsePokemonMove(node);
    if (pokemon != null)
    {
        this.context.Pokemon.Add(pokemon);
        if (pokemonMoves != null)
        {
            foreach (PokemonMove pokemonMove in pokemonMoves)
            {
                if (pokemonMove != null)
                {
                    pokemonMove.ID = pokemonMoveIndex;
                    storedPokemonMoves.Add(pokemonMove);
                    pokemonMoveIndex++;
                }
            }
        }
    }
    ItemProgress++;
}
//Save changes to prepare for inserting PokemonMove entries
this.context.SaveChanges();

//PokemonMove
this.context.PokemonMove.AddRange(storedPokemonMoves);
this.context.SaveChanges();
```

##### EvolutionChain
//TODO: Explain
```csharp
//EvolutionChain
int evolutionChainIndex = 1;
foreach (int id in evolutionChainIndexes)
{
    List<EvolutionChain> evolutionChains = PokeAPIFetcher.ParseEvolutionChain(PokeAPIFetcher.RetrieveJSON("evolution-chain", id));
    if (evolutionChains != null)
    {
        foreach (EvolutionChain chain in evolutionChains)
        {
            if (chain != null)
            {
                chain.ID = evolutionChainIndex;
                this.context.EvolutionChain.Add(chain);
                evolutionChainIndex++;
            }
        }
    }
    ItemProgress++;
}

//Save changes
this.context.SaveChanges();
```

### Adding UI
//TODO
Inside MainWindow.xaml, we wil add the following code. This will require a property Handler to be in the MainWindow class.
```xaml
<Grid Name="FetchGroup" Width="800" Height="600" Background="#DFFFFFFF" IsEnabled="{Binding Path=Handler.IsRunning, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Visibility="{Binding Path=Handler.UIVisibility, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" MouseDown="FetchGroupMouseDown">
    <StackPanel Orientation="Vertical" Width="720" Height="100" Background="#FFFFFF" Visibility="Visible">
        <TextBlock TextAlignment="Center" FontSize="20">Fetching Pokémon data from PokéAPI</TextBlock>
        <ProgressBar x:Name="TableProgressBar" Minimum="0" Maximum="{Binding Handler.TableMax, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Value="{Binding Handler.TableProgress, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Height="20" Width="700" Margin="0,10,0,0"/>
        <ProgressBar x:Name="ItemProgressBar" Minimum="0" Maximum="{Binding Handler.ItemMax, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Value="{Binding Handler.ItemProgress, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Height="20" Width="700" Margin="0,10,0,0"/>
    </StackPanel>
</Grid>
```
We will next update MainWindow.xaml.cs
```csharp
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
```
We will also update our DatabaseInitHandler class. We will change this class to public and extend it with INotifyPropertyChanged, whihch will allow to update the UI.

```csharp
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using PokedexExplorer.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace PokedexExplorer.Data
{
    public class DatabaseInitHandler : INotifyPropertyChanged
    {
        private MainWindow window;
        private PokemonDbContext context;
        private Thread thread;
        private int tableProgress, tableMax, itemProgress, itemMax;
        private Visibility uiVisibility = Visibility.Hidden;
        private bool isRunning;
        public int TableProgress
        {
            get => tableProgress;
            private set
            {
                this.tableProgress = value;
                OnPropertyChanged(nameof(TableProgress));
            }
        }
        public int TableMax
        {
            get => tableMax;
            private set
            {
                this.tableMax = value;
                OnPropertyChanged(nameof(TableMax));
            }
        }
        public int ItemProgress
        {
            get => itemProgress;
            private set
            {
                this.itemProgress = value;
                OnPropertyChanged(nameof(ItemProgress));
            }
        }
        public int ItemMax
        {
            get => itemMax;
            private set
            {
                this.itemMax = value;
                OnPropertyChanged(nameof(ItemMax));
            }
        }
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                this.isRunning = value;
                OnPropertyChanged(nameof(IsRunning));
            }
        }
        
        public Visibility UIVisibility
        {
            get => uiVisibility;
            private set
            {
                if (uiVisibility != value)
                {
                    uiVisibility = value;
                    OnPropertyChanged(nameof(UIVisibility));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DatabaseInitHandler(MainWindow window, PokemonDbContext context) {
            this.window = window;
            this.context = context;
            thread = new Thread(Run);
        }

        public void Start()
        {
            if (thread.IsAlive) return;
            thread.Start();
        }

        public void Run()
        {
            this.UIVisibility = Visibility.Visible;
            this.IsRunning = true;

            TableMax = 5;

            List<int> abilityIndexes = PokeAPIFetcher.GetEntries("ability");
            List<int> moveIndexes = PokeAPIFetcher.GetEntries("move");
            List<int> pokemonIndexes = PokeAPIFetcher.GetEntries("pokemon");
            List<int> pokemonSpeciesIndexes = PokeAPIFetcher.GetEntries("pokemon-species");
            List<int> evolutionChainIndexes = PokeAPIFetcher.GetEntries("evolution-chain");

            //Ability
            this.ItemMax = abilityIndexes.Count;
            this.TableProgress = 0;
            this.ItemProgress = 0;
            foreach (int id in abilityIndexes)
            {
                Ability ability = PokeAPIFetcher.ParseAbility(PokeAPIFetcher.RetrieveJSON("ability", id));
                if (ability != null) this.context.Ability.Add(ability);
                this.ItemProgress++;
                Debug.WriteLine("Added ability " + ability.ID + "(" + id + ")");
                this.context.SaveChanges();
            }

            //Save changes
            this.context.SaveChanges();

            //Move
            this.ItemMax = moveIndexes.Count;
            this.TableProgress = 1;
            this.ItemProgress = 0;
            foreach (int id in moveIndexes)
            {
                Move move = PokeAPIFetcher.ParseMove(PokeAPIFetcher.RetrieveJSON("move", id));
                if (move != null) this.context.Move.Add(move);
                ItemProgress++;
                Debug.WriteLine("Added move " + move.ID + "(" + id + ")");
                this.context.SaveChanges();
            }

            //Save changes
            this.context.SaveChanges();

            //PokemonSpecies
            this.ItemMax = pokemonSpeciesIndexes.Count;
            this.TableProgress = 2;
            this.ItemProgress = 0;
            foreach (int id in pokemonSpeciesIndexes)
            {
                PokemonSpecies pokemonSpecies = PokeAPIFetcher.ParsePokemonSpecies(PokeAPIFetcher.RetrieveJSON("pokemon-species", id));
                if (pokemonSpecies != null) this.context.PokemonSpecies.Add(pokemonSpecies);
                ItemProgress++;
                Debug.WriteLine("Added pokemonSpecies " + pokemonSpecies.ID + "(" + id + ")");
                this.context.SaveChanges();
            }

            //Save changes
            this.context.SaveChanges();

            //Pokemon
            this.ItemMax = pokemonIndexes.Count;
            this.TableProgress = 3;
            this.ItemProgress = 0;
            int pokemonMoveIndex = 1;
            List<PokemonMove> storedPokemonMoves = new List<PokemonMove>();
            foreach (int id in pokemonIndexes)
            {
                JObject node = PokeAPIFetcher.RetrieveJSON("pokemon", id);
                Pokemon pokemon= PokeAPIFetcher.ParsePokemon(node);
                List<PokemonMove> pokemonMoves = PokeAPIFetcher.ParsePokemonMove(node);
                if (pokemon != null)
                {
                    this.context.Pokemon.Add(pokemon);
                    if (pokemonMoves != null)
                    {
                        foreach (PokemonMove pokemonMove in pokemonMoves)
                        {
                            if (pokemonMove != null)
                            {
                                pokemonMove.ID = pokemonMoveIndex;
                                storedPokemonMoves.Add(pokemonMove);
                                pokemonMoveIndex++;
                            }
                        }
                    }
                }
                ItemProgress++;
                Debug.WriteLine("Added pokemon " + pokemon.ID + "(" + id + ")");
                this.context.SaveChanges();
            }
            //Save changes to prepare for inserting PokemonMove entries
            this.context.SaveChanges();
            
            //PokemonMove
            this.context.PokemonMove.AddRange(storedPokemonMoves);
            this.context.SaveChanges();

            //EvolutionChain
            this.ItemMax = evolutionChainIndexes.Count;
            this.TableProgress = 4;
            this.ItemProgress = 0;
            int evolutionChainIndex = 1;
            foreach (int id in evolutionChainIndexes)
            {
                List<EvolutionChain> evolutionChains = PokeAPIFetcher.ParseEvolutionChain(PokeAPIFetcher.RetrieveJSON("evolution-chain", id));
                if (evolutionChains != null)
                {
                    foreach (EvolutionChain chain in evolutionChains)
                    {
                        if (chain != null)
                        {
                            chain.ID = evolutionChainIndex;
                            this.context.EvolutionChain.Add(chain);
                            evolutionChainIndex++;
                            Debug.WriteLine("Added evolutionChains " + chain.ID + "(" + id + ")");
                            this.context.SaveChanges();
                        }
                    }
                }
                ItemProgress++;
            }

            //Save changes
            this.context.SaveChanges();

            this.UIVisibility = Visibility.Hidden;
            this.IsRunning = false;
        }
    }
}
```
And, let's run!

Right now, all Pokémon data should download and be inserted to the Postgre database.

# Setting up the Pokédex Explorer app
## 
## Setup
*Note: You can copy-paste the code, but you should at least know what it does.*
#### PokemonSearch
Now, we will create a new class file: `Data/PokemonSearch`. This class will hold all our search parameters and update itself.
##### 
```csharp
namespace PokedexExplorer.Data
{
    public class PokemonSearch
    {
        private string? _name, _type1, _type2, _ability, _move, _legendaryStatus, _appearanceColor, _appearanceShape;
        private int? _generation, _appearanceHeightMin, _appearanceHeightMax, _appearanceWeightMin, _appearanceWeightMax;
        private int? _statHPMin, _statHPMax, _statAttackMin, _statAttackMax, _statDefenseMin, _statDefenseMax;
        private int? _statSpecialAttackMin, _statSpecialAttackMax, _statSpecialDefenseMin, _statSpecialDefenseMax, _statSpeedMin, _statSpeedMax;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                UpdateQuery();
            }
        }
        public string? Type1
        {
            get => _type1;
            set
            {
                _type1 = value;
                UpdateQuery();
            }
        }
        public string? Type2
        {
            get => _type2;
            set
            {
                _type2 = value;
                UpdateQuery();
            }
        }
        public string? Ability
        {
            get => _ability;
            set
            {
                _ability = value;
                UpdateQuery();
            }
        }
        public string? Move
        {
            get => _move;
            set
            {
                _move = value;
                UpdateQuery();
            }
        }
        public int? Generation
        {
            get => _generation;
            set
            {
                _generation = value;
                UpdateQuery();
            }
        }
        public string? AppearanceColor
        {
            get => _appearanceColor;
            set
            {
                _appearanceColor = value;
                UpdateQuery();
            }
        }
        public string? AppearanceShape
        {
            get => _appearanceShape;
            set
            {
                _appearanceShape = value;
                UpdateQuery();
            }
        }
        public int? AppearanceHeightMin
        {
            get => _appearanceHeightMin;
            set
            {
                _appearanceHeightMin = value;
                UpdateQuery();
            }
        }
        public int? AppearanceHeightMax
        {
            get => _appearanceHeightMax;
            set
            {
                _appearanceHeightMax = value;
                UpdateQuery();
            }
        }
        public int? AppearanceWeightMin
        {
            get => _appearanceWeightMin;
            set
            {
                _appearanceWeightMin = value;
                UpdateQuery();
            }
        }
        public int? AppearanceWeightMax
        {
            get => _appearanceWeightMax;
            set
            {
                _appearanceWeightMax = value;
                UpdateQuery();
            }
        }
        public int? StatHPMin
        {
            get => _statHPMin;
            set
            {
                _statHPMin = value;
                UpdateQuery();
            }
        }
        public int? StatHPMax
        {
            get => _statHPMax;
            set
            {
                _statHPMax = value;
                UpdateQuery();
            }
        }
        public int? StatAttackMin
        {
            get => _statAttackMin;
            set
            {
                _statAttackMin = value;
                UpdateQuery();
            }
        }
        public int? StatAttackMax
        {
            get => _statAttackMax;
            set
            {
                _statAttackMax = value;
                UpdateQuery();
            }
        }
        public int? StatDefenseMin
        {
            get => _statDefenseMin;
            set
            {
                _statDefenseMin = value;
                UpdateQuery();
            }
        }
        public int? StatDefenseMax
        {
            get => _statDefenseMax;
            set
            {
                _statDefenseMax = value;
                UpdateQuery();
            }
        }
        public int? StatSpecialAttackMin
        {
            get => _statSpecialAttackMin;
            set
            {
                _statSpecialAttackMin = value;
                UpdateQuery();
            }
        }
        public int? StatSpecialAttackMax
        {
            get => _statSpecialAttackMax;
            set
            {
                _statSpecialAttackMax = value;
                UpdateQuery();
            }
        }
        public int? StatSpecialDefenseMin
        {
            get => _statSpecialDefenseMin;
            set
            {
                _statSpecialDefenseMin = value;
                UpdateQuery();
            }
        }
        public int? StatSpecialDefenseMax
        {
            get => _statSpecialDefenseMax;
            set
            {
                _statSpecialDefenseMax = value;
                UpdateQuery();
            }
        }
        public int? StatSpeedMin
        {
            get => _statSpeedMin;
            set
            {
                _statSpeedMin = value;
                UpdateQuery();
            }
        }
        public int? StatSpeedMax
        {
            get => _statSpeedMax;
            set
            {
                _statSpeedMax = value;
                UpdateQuery();
            }
        }
        public string? LegendaryStatus
        {
            get => _legendaryStatus;
            set
            {
                _legendaryStatus = value;
                UpdateQuery();
            }
        }

        public IQueryable<PokemonGridData> Query { get; private set; }

        private PokemonDbContext context;
        private MainWindow window;
        public PokemonSearch(PokemonDbContext context, MainWindow window)
        {
            this.context = context;
            this.window = window;
        }

        public void Init()
        {
            IQueryable<Model.Pokemon> query = context.Pokemon;

            this.Query = query.Select(p => new PokemonGridData(
                p.Name,
                p.PrimaryType,
                p.SecondaryType,
                p.SpriteFrontDefault
            ));

            this.window.OnQueryUpdated();
        }
        private void UpdateQuery()
        {
            // TODO

            this.window.OnQueryUpdated();
        }
    }
    public class ImageResources
    {
        static private readonly Dictionary<string, BitmapImage> _imageCache = new Dictionary<string, BitmapImage>();

        static public BitmapImage GetImage(string url)
        {
            if (url == null) return null;
            if (!_imageCache.ContainsKey(url))
            {
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                _imageCache[url] = bitmap;
            }
            return _imageCache[url];
        }
    }
    public class PokemonGridData
    {
        public string Name { get; set; }
        public string PrimaryColor
        {
            get
            {
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
            }
        }
        public string SecondaryColor
        {
            get
            {
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
            }
        }
        public string PrimaryType { get; set; }
        public string? SecondaryType { get; set; }
        public string? SpriteFrontDefault { get; set; }
        public BitmapImage? SpriteImage { get; set; }

        public PokemonGridData(string name, string primaryType, string? secondaryType, string? spriteFrontDefault)
        {
            Name = name.ToUpper();
            PrimaryType = primaryType.ToUpper();
            SecondaryType = secondaryType?.ToUpper() ?? null;
            SpriteFrontDefault = spriteFrontDefault;
            SpriteImage = ImageResources.GetImage(spriteFrontDefault);
        }
    }
}
```
##### UI
We will create new UI. We will write the XAML **BEFORE** the loading UI.
```xaml
<StackPanel Name="PokedexGroup" Orientation="Horizontal" Width="800" Height="600">
    <ScrollViewer Name="FilteredDataGroup" Width="520">
        <ItemsControl Name="PokemmonDataGrid" ItemsSource="{Binding Data.PokemonGridData}">
            <ItemsControl.ItemsPanel>
                <ItemsPanelTemplate>
                    <WrapPanel Orientation="Horizontal" ItemWidth="125" ItemHeight="150"/>
                </ItemsPanelTemplate>
            </ItemsControl.ItemsPanel>
            <ItemsControl.ItemTemplate>
                <DataTemplate>
                    <Border BorderBrush="Black" BorderThickness="1" Margin="5">
                        <StackPanel Width="100" Height="150">
                            <Image Source="{Binding SpriteImage}" Width="100" Height="100" HorizontalAlignment="Center"/>
                            <TextBlock Text="{Binding Name}" FontWeight="Bold" FontSize="10" HorizontalAlignment="Center" TextAlignment="Center"/>
                            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                                <TextBlock Text="{Binding PrimaryType}" FontWeight="Bold" FontSize="10" Width="50" HorizontalAlignment="Center" TextAlignment="Center" Background="{Binding PrimaryColor}"/>
                                <TextBlock Text="{Binding SecondaryType}" FontWeight="Bold" FontSize="10" Width="50" HorizontalAlignment="Center" TextAlignment="Center" Background="{Binding SecondaryColor}"/>
                            </StackPanel>
                        </StackPanel>
                    </Border>
                </DataTemplate>
            </ItemsControl.ItemTemplate>
        </ItemsControl>
    </ScrollViewer>
    <ScrollViewer Name="SearchParametersGroup" Width="280">
        <StackPanel Orientation="Horizontal">
            <StackPanel Margin="0,0,0,0">
                <StackPanel Name="NameStack" Orientation="Horizontal">
                    <Label Content="NAME:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <TextBox x:Name="NameTextBox" TextWrapping="Wrap" Text="" Margin="0,0,0,0" FontSize="18" HorizontalAlignment="Left" Width="150" Height="25" TextChanged="SearchedNameTextChanged"/>
                </StackPanel>
                <StackPanel Name="TypeStack" Orientation="Horizontal">
                    <Label Content="TYPE:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <ComboBox Name="Type1ComboBox" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" Width="70" SelectionChanged="SearchedType1Changed">
                        <ComboBoxItem>Any</ComboBoxItem>
                        <ComboBoxItem>Bug</ComboBoxItem>
                        <ComboBoxItem>Dark</ComboBoxItem>
                        <ComboBoxItem>Dragon</ComboBoxItem>
                        <ComboBoxItem>Electric</ComboBoxItem>
                        <ComboBoxItem>Fairy</ComboBoxItem>
                        <ComboBoxItem>Fighting</ComboBoxItem>
                        <ComboBoxItem>Fire</ComboBoxItem>
                        <ComboBoxItem>Flying</ComboBoxItem>
                        <ComboBoxItem>Ghost</ComboBoxItem>
                        <ComboBoxItem>Grass</ComboBoxItem>
                        <ComboBoxItem>Ground</ComboBoxItem>
                        <ComboBoxItem>Ice</ComboBoxItem>
                        <ComboBoxItem>Normal</ComboBoxItem>
                        <ComboBoxItem>Poison</ComboBoxItem>
                        <ComboBoxItem>Psychic</ComboBoxItem>
                        <ComboBoxItem>Rock</ComboBoxItem>
                        <ComboBoxItem>Steel</ComboBoxItem>
                        <ComboBoxItem>Water</ComboBoxItem>
                    </ComboBox>
                    <ComboBox Name="Type2ComboBox" Margin="10,0,0,0" Width="70" HorizontalAlignment="Left" VerticalAlignment="Center" SelectionChanged="SearchedType2Changed">
                        <ComboBoxItem>Any</ComboBoxItem>
                        <ComboBoxItem>Bug</ComboBoxItem>
                        <ComboBoxItem>Dark</ComboBoxItem>
                        <ComboBoxItem>Dragon</ComboBoxItem>
                        <ComboBoxItem>Electric</ComboBoxItem>
                        <ComboBoxItem>Fairy</ComboBoxItem>
                        <ComboBoxItem>Fighting</ComboBoxItem>
                        <ComboBoxItem>Fire</ComboBoxItem>
                        <ComboBoxItem>Flying</ComboBoxItem>
                        <ComboBoxItem>Ghost</ComboBoxItem>
                        <ComboBoxItem>Grass</ComboBoxItem>
                        <ComboBoxItem>Ground</ComboBoxItem>
                        <ComboBoxItem>Ice</ComboBoxItem>
                        <ComboBoxItem>Normal</ComboBoxItem>
                        <ComboBoxItem>Poison</ComboBoxItem>
                        <ComboBoxItem>Psychic</ComboBoxItem>
                        <ComboBoxItem>Rock</ComboBoxItem>
                        <ComboBoxItem>Steel</ComboBoxItem>
                        <ComboBoxItem>Water</ComboBoxItem>
                    </ComboBox>
                </StackPanel>
                <StackPanel Name="GenerationStack" Orientation="Horizontal">
                    <Label Content="GENERATION:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <ComboBox Name="GenertionComboBox" Margin="0,0,0,0" VerticalAlignment="Center" Width="150" SelectionChanged="SearchedGenerationChanged">
                        <ComboBoxItem>Any</ComboBoxItem>
                        <ComboBoxItem Tag="1">Generation 1 (Kanto)</ComboBoxItem>
                        <ComboBoxItem Tag="2">Generation 2 (Johto)</ComboBoxItem>
                        <ComboBoxItem Tag="3">Generation 3 (Hoenn)</ComboBoxItem>
                        <ComboBoxItem Tag="4">Generation 4 (Sinoh)</ComboBoxItem>
                        <ComboBoxItem Tag="5">Generation 5 (Unova)</ComboBoxItem>
                        <ComboBoxItem Tag="6">Generation 6 (Kalos)</ComboBoxItem>
                        <ComboBoxItem Tag="7">Generation 7 (Alola)</ComboBoxItem>
                        <ComboBoxItem Tag="8">Generation 8 (Galar)</ComboBoxItem>
                        <ComboBoxItem Tag="9">Generation 9 (Paldea)</ComboBoxItem>
                    </ComboBox>
                </StackPanel>
                <StackPanel Name="MoveStack" Orientation="Horizontal">
                    <Label Content="MOVE:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <TextBox x:Name="KnowsMoveTextBox" TextWrapping="Wrap" Text="" Margin="0,0,0,0" FontSize="18" HorizontalAlignment="Left" Width="150" Height="25" TextChanged="SearchedMoveTextChanged"/>
                </StackPanel>
                <StackPanel Name="AbilityStack" Orientation="Horizontal">
                    <Label Content="ABILITY:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <TextBox x:Name="AbilityTextBox" TextWrapping="Wrap" Text="" Margin="0,0,0,0" FontSize="18" HorizontalAlignment="Left" Width="150" Height="25" TextChanged="SearchedAbilityTextChanged"/>
                </StackPanel>
                <StackPanel Name="LegendaryStack" Orientation="Horizontal">
                    <Label Content="LEGENDARY:" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="100"/>
                    <ComboBox Name="LegendaryComboBox" Margin="0,0,0,0" VerticalAlignment="Center" Width="150" SelectionChanged="SearchedLegendaryStatusSelectionChanged">
                        <ComboBoxItem>Any</ComboBoxItem>
                        <ComboBoxItem>Legendary</ComboBoxItem>
                        <ComboBoxItem>Mythical</ComboBoxItem>
                        <ComboBoxItem>None</ComboBoxItem>
                    </ComboBox>
                </StackPanel>
                <Expander Name="AppearanceExpander" Header="APPEARANCE" ExpandDirection="Down" IsExpanded="True" FontWeight="Bold" FontSize="14">
                    <StackPanel Name="AppearanceStack">
                        <StackPanel Name="AppearanceColorStack" Orientation="Horizontal">
                            <Label Content="COLOR:" HorizontalAlignment="Left" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <ComboBox Name="AppearanceColorComboBox" Margin="0,0,0,0" VerticalAlignment="Center" Width="150" SelectionChanged="SearchedAppearanceColorSelectionChanged">
                                <ComboBoxItem Tag="Any">Any</ComboBoxItem>
                                <ComboBoxItem Tag="white">White</ComboBoxItem>
                                <ComboBoxItem Tag="gray">Gray</ComboBoxItem>
                                <ComboBoxItem Tag="black">Black</ComboBoxItem>
                                <ComboBoxItem Tag="red">Red</ComboBoxItem>
                                <ComboBoxItem Tag="green">Green</ComboBoxItem>
                                <ComboBoxItem Tag="blue">Blue</ComboBoxItem>
                                <ComboBoxItem Tag="yellow">Yellow</ComboBoxItem>
                                <ComboBoxItem Tag="pink">Pink</ComboBoxItem>
                                <ComboBoxItem Tag="brown">Brown</ComboBoxItem>
                                <ComboBoxItem Tag="purple">Purple</ComboBoxItem>
                            </ComboBox>
                        </StackPanel>
                        <StackPanel Name="AppearanceShapeStack" Orientation="Horizontal">
                            <Label Content="SHAPE:" HorizontalAlignment="Left" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <ComboBox Name="AppearanceShapeComboBox" Margin="0,0,0,0" VerticalAlignment="Center" Width="150" SelectionChanged="SearchedAppearanceShapeSelectionChanged">
                                <ComboBoxItem Tag="Any">Any</ComboBoxItem>
                                <ComboBoxItem Tag="armor">Armor</ComboBoxItem>
                                <ComboBoxItem Tag="arms">Arms</ComboBoxItem>
                                <ComboBoxItem Tag="ball">Ball</ComboBoxItem>
                                <ComboBoxItem Tag="blob">Blob</ComboBoxItem>
                                <ComboBoxItem Tag="bug wings">Bug Wings</ComboBoxItem>
                                <ComboBoxItem Tag="fish">Fish</ComboBoxItem>
                                <ComboBoxItem Tag="heads">Heads</ComboBoxItem>
                                <ComboBoxItem Tag="humanoid">Humanoid</ComboBoxItem>
                                <ComboBoxItem Tag="legs">Legs</ComboBoxItem>
                                <ComboBoxItem Tag="quadruped">Quadruped</ComboBoxItem>
                                <ComboBoxItem Tag="squiggle">Squiggle</ComboBoxItem>
                                <ComboBoxItem Tag="upright">Upright</ComboBoxItem>
                                <ComboBoxItem Tag="tentacles">Tentacles</ComboBoxItem>
                                <ComboBoxItem Tag="wings">Wings</ComboBoxItem>
                            </ComboBox>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="HEIGHT:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedAppearanceHeightMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedAppearanceHeightMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="WEIGHT:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedAppearanceWeightMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedAppearanceWeightMaxChanged"/>
                        </StackPanel>
                    </StackPanel>
                </Expander>
                <Expander Name="StatsExpander" Header="STATS" ExpandDirection="Down" IsExpanded="True" FontWeight="Bold" FontSize="14">
                    <StackPanel Name="StatsStack">
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="HP:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatHPMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatHPMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="ATTACK:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatAttackMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatAttackMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="DEFENSE:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatDefenseMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatDefenseMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="SP. ATT.:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpecialAttackMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpecialAttackMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="SP. DEF.:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpecialDefenseMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpecialDefenseMaxChanged"/>
                        </StackPanel>
                        <StackPanel Orientation="Horizontal" Margin="0,0,0,0">
                            <Label Content="SPEED:" Margin="20,0,0,0" VerticalAlignment="Center" FontWeight="Bold" FontSize="14" Width="80"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpeedMinChanged"/>
                            <Label Content="-" Margin="0,0,0,0" VerticalAlignment="Center" HorizontalContentAlignment="Center" VerticalContentAlignment="Center" FontSize="14" Width="20" Height="25"/>
                            <TextBox Margin="0,0,0,0" VerticalAlignment="Center" Width="65" Height="25" TextChanged="SearchedStatSpeedMaxChanged"/>
                        </StackPanel>
                    </StackPanel>
                </Expander>
            </StackPanel>
        </StackPanel>
    </ScrollViewer>
</StackPanel>
```
##### MainWindow.xaml.cs
And finally, we will tie it all together with methods in the MainWindow class. 
```csharp
public void OnQueryUpdated()
{
    Debug.WriteLine("Updated");
    List<PokemonGridData> data = Search.Query.ToList();
    if (data != null) PokemmonDataGrid.ItemsSource = data;
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
}(object sender, TextChangedEventArgs e)
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
```
##### Updating the Search Results
**Parameter Change in the UI**: Any interaction with the user interface (e.g., changing a filter in a dropdown, entering text in a search box) triggers an update in the MainWindow.Search object. This object holds the search parameters that the user adjusts through the UI.

**Property Update in MainWindow.Search**: The changes in the UI are reflected in the properties of MainWindow.Search. These properties are designed to capture and store the updated values provided by the user.

**Calling PokemonSearch.UpdateQuery()**: Once the properties in MainWindow.Search are updated, the method PokemonSearch.UpdateQuery() is invoked. This method is responsible for creating or modifying the query based on the current search parameters. At this stage, the LINQ query is updated to reflect the latest criteria provided by the user.

**Executing MainWindow.OnQueryUpdated()**: After updating the query, the method MainWindow.OnQueryUpdated() is called. This method ensures that the UI reflects the changes by, for instance, refreshing the displayed results or updating other UI components that depend on the query's outcome.

# Creating Queries
*Note: This section will be talking about the contents of `PokemonSearch.UpdateQuery()` method.*

There are two phases to queries - creating the query and running the query. We can run the query with methods like `FirstOrDefault()` or `ToList()`.
## LINQ
LINQ (Language Integrated Query) is a powerful feature in Entity Framework that simplifies working with data. It allows easier query creation.
### Creating a default "empty" query.
To retrieve data from the database, we start by defining a query that serves as the foundation for further refinement and execution. In Entity Framework, queries are constructed using LINQ and are deferred until explicitly executed. This means we can build complex queries step-by-step, applying filters, ordering, or projections as needed. In the `PokemonSearch.UpdateQuery()` method, the query begins with a baseline declaration that points to the dataset we want to work with. Let’s break down this first step:
```csharp
private void UpdateQuery()
{
    IQueryable<Model.Pokemon> query = context.Pokemon;

    // Filters go here...
    
    this.window.OnQueryUpdated();
}
```
### Select
After defining the base query, we now tailor it to retrieve only the columns we need for display in the user interface. The following code snippet demonstrates how we transform the query results into a structured format suitable for the UI.

The `Select()` method is used to project data into a new shape or structure. Instead of fetching entire Pokemon objects, this projection retrieves specific properties: Name, PrimaryType, SecondaryType, and SpriteFrontDefault.

```csharp
private void UpdateQuery()
{
    IQueryable<Model.Pokemon> query = context.Pokemon;

    // Filters go here...

    this.Query = query.Select(p => new PokemonGridData(
        p.Name,
        p.PrimaryType,
        p.SecondaryType,
        p.SpriteFrontDefault
    ));

    this.window.OnQueryUpdated();
}
```

This corresponds to our UI definition:
```xaml
<ItemsControl Name="PokemmonDataGrid" ItemsSource="{Binding Data.PokemonGridData}">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <WrapPanel Orientation="Horizontal" ItemWidth="125" ItemHeight="150"/>
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <Border BorderBrush="Black" BorderThickness="1" Margin="5">
                <StackPanel Width="100" Height="150">
                    <Image Source="{Binding SpriteImage}" Width="100" Height="100" HorizontalAlignment="Center"/>
                    <TextBlock Text="{Binding Name}" FontWeight="Bold" FontSize="10" HorizontalAlignment="Center" TextAlignment="Center"/>
                    <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                        <TextBlock Text="{Binding PrimaryType}" FontWeight="Bold" FontSize="10" Width="50" HorizontalAlignment="Center" TextAlignment="Center" Background="{Binding PrimaryColor}"/>
                        <TextBlock Text="{Binding SecondaryType}" FontWeight="Bold" FontSize="10" Width="50" HorizontalAlignment="Center" TextAlignment="Center" Background="{Binding SecondaryColor}"/>
                    </StackPanel>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```
### ToList
In the MainWindow class, there is a yet-unexplained method `ToList()`. This will run the actual query.
Here is the corresponding code:
```csharp
public void OnQueryUpdated()
{
    Debug.WriteLine("Updated");
    List<PokemonGridData> data = Search.Query.ToList();
    if (data != null) PokemmonDataGrid.ItemsSource = data;
}
```

# Searching and Filtering
###TODO
To make our queries more versatile and user-driven, we introduce a series of filters that allow for dynamic data retrieval based on specific criteria. These filters refine the results by narrowing down the dataset according to parameters like name, type, or any other attributes of interest. By adding filters, we ensure that users can efficiently search and interact with the data in meaningful ways.
### Name
To implement a straightforward name filter, we write a query that checks if a Pokémon's name starts with the given search string. The search string is stored in the Name property of the PokemonSearch class.
```csharp
query = query.Where(p => p.Name.StartsWith(Name));
```

The Where method in LINQ is used to filter a dataset based on a specified condition. It takes a predicate (a function that returns a Boolean value) as an argument, and only elements that satisfy this condition are included in the resulting sequence.

This single line of LINQ code translates directly into the following SQL query:
```sql
SELECT * FROM "Pokemon"
WHERE "Name" LIKE 'NamePrefix%';

```
Before applying the filter, we also ensure that the search string has been provided (i.e., it is not null and not empty):
```csharp
if (Name != null)
{
    query = query.Where(p => p.Name.StartsWith(Name));
}
```
### Type
Next, we can add type filtering. The user can select a typing, and the query will check against either possible typing in the table.
```csharp
query = query.Where(p => p.PrimaryType == this.Type1 || p.SecondaryType == this.Type1);
```

This single line of LINQ code translates directly into the following SQL query:
```sql
SELECT * FROM "Pokemon"
WHERE "PrimaryType" = 'Type1' OR "SecondaryType" = 'Type1';

```

Next, we can add the second selected typing:
```csharp
query = query.Where(p => p.PrimaryType == this.Type2 || p.SecondaryType == this.Type2);
```

And wrap it with safety checks:
```csharp
if (this.Type1 != null && this.Type1.Length > 0)
{
    query = query.Where(p => p.PrimaryType == this.Type1 || p.SecondaryType == this.Type1);
}
else if (this.Type2 != null && this.Type2.Length > 0)
{
    query = query.Where(p => p.PrimaryType == this.Type2 || p.SecondaryType == this.Type2);
}
```

### Ability
Next, we incorporate filtering based on abilities. This allows users to search for Pokémon whose abilities match the specified criteria.

In the database schema, the Pokemon table has three columns—`PrimaryAbility`, `SecondaryAbility`, and `HiddenAbility`—that reference the ID column in the Ability table. This relationship is defined by foreign keys, which establish a connection between the Pokémon's abilities and their corresponding entries in the Ability table.
```csharp
query = query.Where(p => context.Ability.Any(a => (a.ID == p.PrimaryAbility || a.ID == p.SecondaryAbility || a.ID == p.HiddenAbility) && a.Name.StartsWith(this.Ability)));
```
The `Any` method in LINQ determines if any element in the Ability collection satisfies the given condition. In this case, we check whether an Ability row with `Ability.ID` matches one of the Pokémon's abilities (`PrimaryAbility`, `SecondaryAbility`, or `HiddenAbility`). If a match is found, we further check if the `Ability.Name` starts with the provided search string (`this.Ability`).

And, we will wrap this line with safety checks:
```csharp
if (this.Ability != null && this.Ability.Length > 0)
{
    query = query.Where(p => context.Ability.Any(a => (a.ID == p.PrimaryAbility || a.ID == p.SecondaryAbility || a.ID == p.HiddenAbility) && a.Name.StartsWith(this.Ability)));
}
```

### Move
This of code adds filtering based on Pokémon moves. It checks whether any move associated with a Pokémon matches the specified search string stored in this.Move.

The PokemonMove table acts as a M:N relation between Pokemon and Move.
```
PokemonMove.Pokemon → Pokemon.ID
PokemonMove.Move → Move.ID
```
Since we want to return a Pokémon object, we begin with the Pokemon table, identifying all entries in the PokemonMove table where the Pokemon column matches the Pokémon's ID. From there, we look for entries in the Move table where the Move.ID matches the Move column in the PokemonMove table and check if the Move.Name satisfies the search condition. This ensures we retrieve only Pokémon associated with moves that meet the specified criteria.
```csharp
query = query.Where(
    p => context.PokemonMove.Any(
        pm => pm.Pokemon == p.ID &&
        context.Move.Any(
            m => m.ID == pm.Move && m.Name.StartsWith(this.Move)
        )
    )
);
```

### Try it yourself
Now, you can these this one yourself.
### Generation
```
Pokemon.Species → PokemmonSpecies.ID
```
We want to check if our `Generation` integer maatches the value `PokemonSpecies.Generation`.
<details>
    <summary>Code</code> (<i>click to reveal answer</i>)</summary>
    
```csharp
if (this.Generation != null)
{
    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Generation == this.Generation));
}
```
</details>
### Legendary Status
We are given a search parameter LegendaryStatus, that can have the values:
```
Any
Legendary
Mmythical
None
```
The PokemonSpecies table has two boolean columns:

```
IsLegendary
IsMythical
```
We want to check if our `LegendaryStatus` string maatches the values `PokemmonSpecies.IsLegendary` and `PokemmonSpecies.IsMythical`.
<details>
    <summary>Code</code> (<i>click to reveal answer</i>)</summary>
    
```csharp
if (this.LegendaryStatus != null && this.LegendaryStatus.Length > 0 && !this.LegendaryStatus.Equals("Any"))
{
    bool isLegendary = this.LegendaryStatus.ToLower().Equals("Legendary");
    bool isMythical= this.LegendaryStatus.ToLower().Equals("Mythical");

    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.IsLegendary == isLegendary && ps.IsMythical == isMythical));
}
```
</details>

### Apearance: Color and Shape
We are given a search parameters `PokemonSpecies.AppearanceColor` and `PokemonSpecies.AppearanceShape`, that have corresponding columns `PokemonSpecies.AppearanceColor` and `PokemonSpecies.AppearanceShape`.
<details>
    <summary>Code</code> (<i>click to reveal answer</i>)</summary>
    
```csharp
if (this.AppearanceColor != null && this.AppearanceColor.Length > 0)
{
    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Color == this.AppearanceColor));
}

if (this.AppearanceShape != null && this.AppearanceShape.Length > 0)
{
    query = query.Where(p => context.PokemonSpecies.Any(ps => ps.Shape == this.AppearanceShape));
}
```
</details>


### Apearance: Height and Weight
We are given a search parameters `AppearanceHeightMin`, `AppearanceHeightMax`, `AppearanceWeightMin` and `AppearanceWeightMax`, that have corresponding columns `Pokemon.AppearanceHeight` and `Pokemon.AppearanceWeight`.
We want to check if:
- A parameter is not null
- The entry values in the Pokemon table are constrained by these parameters
```
AppearanceHeightMin < AppearanceHeightMax <= Pokemon.AppearanceHeight
AppearanceWeightMin < AppearanceWeightMax <= Pokemon.AppearanceWeight
```
<details>
    <summary>Code</code> (<i>click to reveal answer</i>)</summary>
    
```csharp
if (this.AppearanceHeightMin != null)
{
    query = query.Where(p => p.Height >= this.AppearanceHeightMin);
}
if (this.AppearanceHeightMax != null)
{
    query = query.Where(p => p.Height <= this.AppearanceHeightMax);
}

if (this.AppearanceWeightMin != null)
{
    query = query.Where(p => p.Weight >= this.AppearanceWeightMin);
}
if (this.AppearanceWeightMax != null)
{
    query = query.Where(p => p.Weight <= this.AppearanceWeightMax);
}
```
</details>

### Stats: HP, Attack, Defense, Special Attack, Special defense and Speed
Finally, we want to check for ranges of stats.
```
StatHPMin < Pokemon.HP <= StatHPMax
StatAttackMin < Pokemon.Attack <= StatAttackMax
StatDefenseMin < Pokemon.Defense <= StatDefenseMax
StatSpecialAttackMin < Pokemon.SpecialAttack <= StatSpecialAttackMax
StatSpecialDefenseMin < Pokemon.SpecialDefense <= StatSpecialDefenseMax
StatSpeedMin < Pokemon.Speed <= StatSpeedMax
```
<details>
    <summary>Code</code> (<i>click to reveal answer</i>)</summary>
    
```csharp
if (this.StatHPMin != null)
{
    query = query.Where(p => p.HP >= this.StatHPMin);
}
if (this.StatHPMax != null)
{
    query = query.Where(p => p.HP >= this.StatHPMax);
}
if (this.StatAttackMin != null)
{
    query = query.Where(p => p.Attack >= this.StatAttackMin);
}
if (this.StatAttackMax != null)
{
    query = query.Where(p => p.Attack >= this.StatAttackMax);
}
if (this.StatDefenseMin != null)
{
    query = query.Where(p => p.Defense >= this.StatDefenseMin);
}
if (this.StatDefenseMax != null)
{
    query = query.Where(p => p.Defense >= this.StatDefenseMax);
}
if (this.StatSpecialAttackMin != null)
{
    query = query.Where(p => p.SpecialAttack >= this.StatSpecialAttackMin);
}
if (this.StatSpecialAttackMax != null)
{
    query = query.Where(p => p.SpecialAttack >= this.StatSpecialAttackMax);
}
if (this.StatSpecialDefenseMin != null)
{
    query = query.Where(p => p.SpecialDefense >= this.StatSpecialDefenseMin);
}
if (this.StatSpecialDefenseMax != null)
{
    query = query.Where(p => p.SpecialDefense >= this.StatSpecialDefenseMax);
}
if (this.StatSpeedMin != null)
{
    query = query.Where(p => p.Speed >= this.StatSpeedMin);
}
if (this.StatSpeedMax != null)
{
    query = query.Where(p => p.Speed >= this.StatSpeedMax);
}
```
</details>

