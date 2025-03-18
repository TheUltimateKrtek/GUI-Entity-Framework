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
### The repository contains three versions of the app:
- The fully completed app
- The try-it-yourself version, including only the filtering part of the app and UI
- A bare-minimum version, containing only the UI and methods associated with it

# What is Entity Framework?
Entity Framework (EF) is an Object-Relational Mapper (ORM) for .NET applications. It simplifies database interactions by allowing developers to work with databases using C# objects instead of writing SQL queries manually. EF still allows to write SQL queries, so no functionality is lost.
### Why should you use Entity Framework?
Without EF, developers typically use ADO.NET, where they have to:
Write SQL queries manually
Manage database connections explicitly
Handle data conversions between SQL and C#
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
Open Visual Studio (or install it with the WPF extension).
Create a new WPF project. We will name it **PokedexExplorer**.
Install the Npgsql and Entity Framework libraries using the NuGet Package Manager.
In the Solution Explorer, under PokedexExplorer, create a new folder **Models**. We will write our tables here. Next, create a folder **Data**. We will put all our data-handling classes there. This step is not required, but it helps keep the project clean. Our tutorial will assume this step was taken.

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

        public Ability(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
```
#### Move
The Move table contains a list of moves that a Pokémon can perform.
```csharp
namespace PokedexExplorer.Model
{
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

        public Move(int iD, string name, int pp, int priority, string target, string type)
        {
            ID = iD;
            Name = name;
            PP = pp;
            Priority = priority;
            Target = target;
            Type = type;
        }
    }
}
```
#### Pokemon
The Pokémon table contains information about the various Pokémon.
```csharp
namespace PokedexExplorer.Model
{
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
        [Required]
        public string SpriteFrontDefault { get; set; }
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

        public Pokemon(int iD, int baseExperience, int height, int weight, int order, int species, int hP, int hPEffort, int attack, int attackEffort, int defense, int defenseEffort, int specialAttack, int specialAttackEffort, int specialDefense, int specialDefenseEffort, int speed, int speedEffort, string spriteFrontDefault, string name, string primaryType)
        {
            ID = iD;
            BaseExperience = baseExperience;
            Height = height;
            Weight = weight;
            Order = order;
            Species = species;
            HP = hP;
            HPEffort = hPEffort;
            Attack = attack;
            AttackEffort = attackEffort;
            Defense = defense;
            DefenseEffort = defenseEffort;
            SpecialAttack = specialAttack;
            SpecialAttackEffort = specialAttackEffort;
            SpecialDefense = specialDefense;
            SpecialDefenseEffort = specialDefenseEffort;
            Speed = speed;
            SpeedEffort = speedEffort;
            SpriteFrontDefault = spriteFrontDefault;
            Name = name;
            PrimaryType = primaryType;
        }
    }
}
```
#### PokemonSpecies
The PokemonSpecies table contains information about the Pokémon species. Note, that a species may contain multiple pokémon. An obvious example is Pikachu with its various versions, each having different attributes and stats.
```csharp
namespace PokedexExplorer.Model
{
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

        public PokemonSpecies(int iD, int baseHappiness, int captureRate, int genderRate, int order, int generation, int nationalPokedexNumber, bool isBaby, bool isLegendary, bool isMythical, string color, string growthRate, string habitat, string shape, string genera, string name)
        {
            ID = iD;
            BaseHappiness = baseHappiness;
            CaptureRate = captureRate;
            GenderRate = genderRate;
            Order = order;
            Generation = generation;
            NationalPokedexNumber = nationalPokedexNumber;
            IsBaby = isBaby;
            IsLegendary = isLegendary;
            IsMythical = isMythical;
            Color = color;
            GrowthRate = growthRate;
            Habitat = habitat;
            Shape = shape;
            Genera = genera;
            Name = name;
        }
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
        
        public EvolutionChain(int id, int evolvesFrom, int evolvesTo)
        {
            this.ID = id;
            this.EvolvesFrom = evolvesFrom;
            this.EvolvesTo = evolvesTo;
        }
    }
}
```
#### PokemonMove
This table represents our many-to-many relation between a Pokémon and a move it can learn. It will also contain additional information about the way a Pokémon can learn a move. This table connects the Pokemon and the Move tables.
```csharp
namespace PokedexExplorer.Model
{
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

        public PokemonMove(int iD, int pokemon, int move)
        {
            ID = iD;
            Pokemon = pokemon;
            Move = move;
        }
    }
}
```
### Indexes
For the purpose of searching, indexing columns will be beneficial. It will speed up search. For example, if we were to search by ability name, it would make sense to use indexing for faster searching. We can add an annotation ```[Index(nameof(Ability.Name), IsUnique = true, Name = "IndexAbilityName")]```to the class.

#### Ability
```csharp
namespace PokedexExplorer.Model
{
    [Index(nameof(Ability.Name), IsUnique = true, Name = "IndexAbilityName")]
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
    [Index(nameof(PokemonSpecies.Name), IsUnique = true, Name = "IndexPokemonName")]
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
To synchronize our database model with Postgre, we will use the method `DbContext.Database.Migrate();`. This will update our tables. The `Migrate()` method handles existing tables, however it will throw exceptions if the existing table is different.
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
        context.Database.Migrate();
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
static public JsonNode? RetrieveJSON(string name, int? id = 0)
{
    string url = "https://pokeapi.co/api/v2/" + name + "/";
    if (id != null) url += id + "/";

    using (HttpClient client = new HttpClient())
    {
        try
        {
            HttpResponseMessage response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;

            return JsonObject.Parse(jsonResponse);
        }
        catch
        {
            return null;
        }
    }
}
```

##### Retrieving entry count
Using the method from before, we can start with processing data. First, we want to find the total number of entries.

```csharp
static public int GetCount(string name)
{
    JsonNode json = RetrieveJSON(name);
    if (json == null) return -1;
    return json["count"].GetValue<int>();
}
```

#### Processing data
Next, we will add methods to parse JSON data

##### Helper Methods
Method `GetURLIntValue` will simply retrieve an index from a url.
```csharp
static private int GetURLIntValue(string url)
{
    string[] split = url.Split('/');
    return int.Parse(split[split.Length - 1]);
}
```
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
static public Ability ParseAbility(JsonNode node)
{
    if (node == null) return null;
    try
    {
        int id = node["id"].GetValue<int>();
        int generation = node["generation"].GetValue<int>();

        JsonNode effectNode = GetEnglishNode(node["effect_entries"].GetValue<JsonNode>());
        string effect = null;
        string shortEffect = null;
        if (effectNode != null)
        {
            effect = effectNode["effect"].GetValue<string>();
            effect = effectNode["short_effect"].GetValue<string>();
        }

        JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
        string description = null;
        if (descriptionNode != null)
        {
            description = descriptionNode["flavor_text"].GetValue<string>();
        }

        JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
        string name = node["name"].GetValue<string>();
        if (nameNode != null)
        {
            name = nameNode["name"].GetValue<string>();
        }

        Ability ability = new Ability(id, name);
        ability.Generation = generation;
        ability.Effect = effect;
        ability.ShortEffect = shortEffect;
        ability.Description = description;
        return ability;
    }
    catch
    {
        return null;
    }
}
```

##### Move
```csharp
static public Move ParseMove(JsonNode node)
{
    if (node == null) return null;

    int? accuracy = node["accuracy"].GetValue<int?>();
    string? damageClass = node["damage_class"]["name"].GetValue<string?>();
    int? effectChance = node["effectChance"].GetValue<int?>();
    int? generation = GetURLIntValue(node["generation"]["url"].GetValue<string?>());
    int id = node["id"].GetValue<int>();

    string? ailment = node["meta"] == null ? null : (node["meta"]["ailment"] == null ? null : node["meta"]["ailment"]["name"].GetValue<string>());
    int? ailmentChance = node["meta"] == null ? null : node["meta"]["ailment_chance"].GetValue<int?>();
    int? critRate = node["meta"] == null ? null : node["meta"]["crit_rate"].GetValue<int?>();
    int? drain = node["meta"] == null ? null : node["meta"]["drain"].GetValue<int?>();
    int? flinchChance = node["meta"] == null ? null : node["meta"]["flinch_chance"].GetValue<int?>();
    int? healing = node["meta"] == null ? null : node["meta"]["healing"].GetValue<int?>();
    int? maxHits = node["meta"] == null ? null : node["meta"]["max_hits"].GetValue<int?>();
    int? maxTurns = node["meta"] == null ? null : node["meta"]["max_turns"].GetValue<int?>();
    int? minHits = node["meta"] == null ? null : node["meta"]["min_hits"].GetValue<int?>();
    int? minTurns = node["meta"] == null ? null : node["meta"]["min_turns"].GetValue<int?>();
    int? statChance = node["meta"] == null ? null : node["meta"]["stat_chance"].GetValue<int?>();

    JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
    string name = node["name"].GetValue<string>();
    if (nameNode != null)
    {
        name = nameNode["name"].GetValue<string>();
    }

    int? power = node["power"].GetValue<int?>();
    int pp = node["pp"].GetValue<int>();
    int priority = node["priority"].GetValue<int>();
    string target = node["target"].GetValue<string>();
    string type = node["type"].GetValue<string>();

    JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
    string description = null;
    if (descriptionNode != null)
    {
        description = descriptionNode["flavor_text"].GetValue<string>();
    }

    Move move = new Move(id, name, pp, priority, target, type);
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
static public PokemonSpecies ParsePokemonSpecies(JsonNode node)
{
    int baseHappiness = node["base_happiness"].GetValue<int>();
    int captureRate = node["capture_rate"].GetValue<int>();
    int genderRate = node["gender_rate"].GetValue<int>();
    int? hatchCounter = node["hatch_counter"].GetValue<int?>();
    int id = node["id"].GetValue<int>();
    int order = node["order"].GetValue<int>();
    bool isBaby = node["is_baby"].GetValue<bool>();
    bool isLegendary = node["is_legendary"].GetValue<bool>();
    bool isMythical = node["is_mythical"].GetValue<bool>();
    string color = node["color"].GetValue<string>();
    string growthRate = node["growth_rate"].GetValue<string>();
    string habitat = node["habitat"].GetValue<string>();
    string shape = node["shape"].GetValue<string>();
    int generation = node["generation"].GetValue<int>();

    JsonNode generaNode = GetEnglishNode(node["genera"].GetValue<JsonNode>());
    string genera = "";
    if (generaNode != null)
    {
        genera = generaNode["genus"].GetValue<string>();
    }

    JsonNode nationalPokedexNumberNode = GetEnglishNode(node["national_pokedex_number"].GetValue<JsonNode>());
    int nationalPokedexNumber = -1;
    if (generaNode != null)
    {
        nationalPokedexNumber = nationalPokedexNumberNode["entry_number"].GetValue<int>();
    }

    JsonNode nameNode = GetEnglishNode(node["names"].GetValue<JsonNode>());
    string name = node["name"].GetValue<string>();
    if (nameNode != null)
    {
        name = nameNode["name"].GetValue<string>();
    }

    JsonNode descriptionNode = GetEnglishNode(node["flavor_text_entries"].GetValue<JsonNode>());
    string? description = null;
    if (descriptionNode != null)
    {
        description = descriptionNode["flavor_text"].GetValue<string>();
    }

    PokemonSpecies species = new PokemonSpecies(id, baseHappiness, captureRate, genderRate, order, generation, nationalPokedexNumber, isBaby, isLegendary, isMythical, color, growthRate, habitat, shape, genera, name);
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

##### Get entry counts
```csharp
int abilityCount = PokeAPIFetcher.GetCount("ability");
int moveCount = PokeAPIFetcher.GetCount("move");
int pokemonCount = PokeAPIFetcher.GetCount("pokemon");
int pokemonSpeciesCount = PokeAPIFetcher.GetCount("pokemon-species");
int evolutionChainCount = PokeAPIFetcher.GetCount("evolution-chain");
```

##### Ability, Move and PokemonSpecies
Populating the Ability, Move and PokemonSpecies tables is simple. Every entry is created by a single request to the PokeAPIFetcher class. We will simply request the object, and if it exists, we will simply insesrt it. Finally, we will save the changes, so that we can be confident future entries can reference these entries.
```csharp
//Ability
for (int i = 0; i < abilityCount; i++)
{
    Ability ability = PokeAPIFetcher.ParseAbility(PokeAPIFetcher.RetrieveJSON("ability", i));
    if (ability != null) this.context.Ability.Add(ability);
}

//Move
for (int i = 0; i < moveCount; i++)
{
    Move move = PokeAPIFetcher.ParseMove(PokeAPIFetcher.RetrieveJSON("move", i));
    if (move != null) this.context.Move.Add(move);
}

//PokemonSpecies
for (int i = 0; i < pokemonSpeciesCount; i++)
{
    PokemonSpecies pokemonSpecies = PokeAPIFetcher.ParsePokemonSpecies(PokeAPIFetcher.RetrieveJSON("pokemon-species", i));
    if (pokemonSpecies != null) this.context.PokemonSpecies.Add(pokemonSpecies);
}

//Save changes
this.context.SaveChanges();
```

##### Pokemon and PokemonMove
Pokemon and PokemonMove tables are both created from the `pokemon` PokéAPI table. We will create at the same time, however we will add `PokemonMove` entries after `Pokemon` entries, because `Pokemon` references `Pokemon`. `PokemonMove` also references `Move`, but all `Move` entries have already been inserted.

As for `PokemonMove`, we need to identify them by different IDs, because the two tables don't have the same number of entries. That's what the `pokemonMoveIndex` variable is for.

```csharp
//Pokemon
this.ItemMax = pokemonCount;
this.TableProgress = 3;
this.ItemProgress = 0;
int pokemonMoveIndex = 0;
List<PokemonMove> storedPokemonMoves = new List<PokemonMove>();
for (int i = 0; i < pokemonCount; i++)
{
    JsonNode node = PokeAPIFetcher.RetrieveJSON("pokemon", i);
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

```csharp
//EvolutionChain
this.ItemMax = evolutionChainCount;
this.TableProgress = 4;
this.ItemProgress = 0;
int evolutionChainIndex = 0;
for (int i = 0; i < evolutionChainCount; i++)
{
    List<EvolutionChain> evolutionChains = PokeAPIFetcher.ParseEvolutionChain(PokeAPIFetcher.RetrieveJSON("evolution-chain", i));
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

### Adding data
We well use our PokeAPI fetcher class to retrieve, process and insert data into our database. We will do this by retrieving the number of entries and tgen looping through every index. We will add entries one by one and then save all changes.
```csharp
//TODO: Add InitData to DatabaseInitHandler
```
