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
        [Required]
        public int Generation { get; set; }
        public string? Ailment { get; set; }
        public int? AilmentChance { get; set; }
        public int? CritRate { get; set; }
        public int? Drain { get; set; }
        public int? FlinchChance { get; set; }
        public int? Healing { get; set; }
        public int? MaxHits { get; set; }
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
        public int? econdaryAbility { get; set; }
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
        [Required]
        public string EggGroups { get; set; }
        [Required]
        public string Varieties { get; set; }
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
            public string? Trade_Species { get; set; }
            public int? RelativePhysicalStats { get; set; }
            public string? Item { get; set; }
            public string? HeldItem { get; set; }
            [ForeignKey("Move")]
            public string? KnownMove { get; set; }
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
        public int Id { get; set; }
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
    [Index(nameof(Pokemon.Name), IsUnique = true, Name = "IndexPokemonName")]
    [Index(nameof(Pokemon.Height), IsUnique = true, Name = "IndexPokemonHeight")]
    [Index(nameof(Pokemon.Weight), IsUnique = true, Name = "IndexPokemonWeight")]
    [Index(nameof(Pokemon.HP), IsUnique = true, Name = "IndexPokemonHp")]
    [Index(nameof(Pokemon.Attack), IsUnique = true, Name = "IndexPokemonAttack")]
    [Index(nameof(Pokemon.Defense), IsUnique = true, Name = "IndexPokemonDefense")]
    [Index(nameof(Pokemon.SpecialAttack), IsUnique = true, Name = "IndexPokemonSpecialAttack")]
    [Index(nameof(Pokemon.SpecialDefense), IsUnique = true, Name = "IndexPokemonSpecialDefense")]
    [Index(nameof(Pokemon.Speed), IsUnique = true, Name = "IndexPokemonSpeed")]
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
    [Index(nameof(PokemonSpecies.Generation), IsUnique = true, Name = "IndexPokemonSpeciesGeneration")]
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
    [Index(nameof(PokemonMove.Pokemon), IsUnique = true, Name = "IndexPokemonMovePokemon")]
    [Index(nameof(PokemonMove.Move), IsUnique = true, Name = "IndexPokemonMoveMove")]
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

We also need to create the tables in the actual database.
#TODO: Insert code here
