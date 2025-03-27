# Zřeknutí se odpovědnosti
Tento projekt využívá zdroje třetích stran. Jedná se o studentský projekt vytvořený pro vzdělávací účely. Není spojen, schválen ani nijak spojen s Nintendo, The Pokémon Company nebo PokéAPI.
### Informace o licencování
The Pokémon data in this project is sourced from PokéAPI, which is licensed under the BSD 3-Clause License. The full license can be found in the `LICENSE.txt` file.
Data o Pokémonech v tomto projektu pocházejí z PokéAPI, která jsou licencovány pod licencí BSD 3-Clause. Plné znění licence lze nalézt v souboru LICENSE.txt.
### Autorství spritů
Sprite Pokémonů v tomto projektu jsou získávány v reálném čase z GitHub repozitáře PokéAPI. Tyto sprite jsou chráněným materiálem vlastněným společností Nintendo a jsou zde použity pouze pro vzdělávací účely.
# Přehled
Tento projekt ukazuje použití Entity Framework s PostgreSQL prostřednictvím aplikace pro filtrování Pokémonů. Projekt se skládá ze dvou částí:
- Skript pro zpracování dat, který získává a převádí data z PokéAPI do tabulek PostgreSQL.
- Aplikace pro filtrování, která uživatelům umožňuje vyhledávat a filtrovat Pokémony podle různých atributů.
### Repozitář obsahuje:
- Plně dokončenou aplikaci
- Návod na vytvoření aplikace od začátku
# Co je Entity Framework?
Entity Framework (EF) je Object-Relational Mapper (ORM) pro aplikace v .NET. Zjednodušuje práci s databází tím, že umožňuje vývojářům pracovat s databázovými daty(databázemi) pomocí objektů v jazyce C# místo psaní SQL dotazů ručně. Přesto EF stále umožňuje psát SQL dotazy, takže žádná funkcionalita není ztracena.
### Proč používat Entity Framework?
Bez EF vývojáři obvykle používají ADO.NET, kde musí:
- Ručně psát SQL dotazy
- Explicitně spravovat databázová připojení
- Ručně konvertovat data mezi SQL a C#
### Klíčové vlastnosti EF
ORM funkce: Mapuje databázové tabulky na C# objekty

Podpora LINQ: Dotazy lze psát pomocí LINQ místo SQL

Migrace: Snadná aktualizace databázového schématu při změně modelů

Automatické sledování změn: Sleduje úpravy entit

Nezávislost na databázi: Umí pracovat s různými databázemi jako SQL Server, MySQL, PostgreSQL atd.

# EF vs. EF Core
There are a few versions of Entity Framework. Let’s look at EF 6 and EF Core.
Existuje několik verzí Entity Frameworku. Podívejme se na srovnání EF 6 s EF Core:
|                          | EF 6                     | EF Core                                        |
| ------------------------ | ------------------------ | ---------------------------------------------- |
| Framework                |.NET                      | .NET & .NET Core                               |
| Cross-platform           | Ano                      | Ne                                             |
| Výkon                    | Pomalejší                | Rychlejší                                      |
| Many-to-many             | Vyžad. spojovací tabulky | nativní podpora                                |
| LINQ                     | Méně optimalizovaný      | Optimalizovaný                                 |
| Databázoví poskytovatelé | Převážně SQL servery     | Podpora pro SQL server, PostgreSQL, MySQL etc. |
| Uložené procedury        | Lepší podpora            | Stále se zlepšuje                              |
| Lazy Loading             | Ano                      | Ano                                            |

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

# Začínáme
V tomto tutoriálu budeme používat pro databázi PostgreSQL, PokéAPI jako zdroj dat a Entity Framework 6. Pojďme začít méně důležitými kroky.
## Nastavení PostgreSQL serveru
*(Note: The Linux tutorial for setting up PostgreSQL was generated with the help of an AI model. While every effort was made to ensure accuracy, AI-generated content may occasionally contain errors. We recommend reviewing the [official PostgreSQL documentation](https://www.postgresql.org/docs/) or trusted resources for additional confirmation and guidance.)*
(Poznámka: Návod pro Linux byl vygenerován s pomocí AI. Přestože byla vynaložena maximální snaha o přesnost, kontent vygenerovaný pomocí AI může občas obsahovat chyby. Doporučujeme ověřit informace v [oficiální PostgreSQL dokumentace]. (https://www.postgresql.org/docs/) nebo v jiných důvěryhodných zdrojích.)*
### Pro Windows

##### Stažení PostgreSQL

Stáhněte PostgreSQL z [oficiálních stránek]. (https://www.postgresql.org/download/). Vyberte verzi odpovídající vašemu operačnímu systému a stáhněte instalační balíček.

##### Instalace PostgreSQL

Postupujte podle pokynů instalačního programu. Během instalace si poznamenejte cestu k instalaci (např. C:\Program Files\PostgreSQL\<verze>).

Výchozí přihlašovací údaje jsou:

Uživatelské jméno: postgres

Heslo: postgres (Nebo heslo, které si nastavíte během instalace.)

##### Post-Instalace

PostgreSQL should start automatically as a service after installation. However, you can also start or stop it manually via the command line:
PostgreSQL by se měl po instalaci automaticky spustit jako služba. Pokud ne, můžete jej také spustit nebo zastavit ručně pomocí příkazového řádku:

##### Spuštění služby PostgreSQL
V příkazovém řádku (přejděte do PostgreSQL bin adresáře):

`pg_ctl start -D <your_database_cluster_path>`
##### Zastavení služby PostgreSQL
Pro zastavení PostgreSQL serveru, použijte:

`pg_ctl stop -D <your_database_cluster_path>`
##### Vytvoření nové databáze
Pro vytvoření nové databáze, potřebujete specifikovat název databáze během instalace. Zadejte následující příkaz v PostgreSQL bin adresáři:

`initdb -D <your_database_cluster_path>`
### Pro Ubuntu/Debian
##### Instalace PostgreSQL
Otevřete terminál a spusťte následující příkazy pro instalaci PostgreSQL:

`sudo apt update`

`sudo apt install postgresql postgresql-contrib`
Pomocí tohoto nainstalujete PostgreSQL a některá užitečná rozšíření.
##### Post-Instalace
PostgreSQL by se měl automaticky spustit po instalaci. Pokud jej potřebujete spustit ručně nebo ověřit jeho stav.:
##### Spuštění PostgreSQL služby

`sudo systemctl start postgresql`

##### Zastavení PostgreSQL služby

`sudo systemctl stop postgresql`

##### Kontrola stavu

`sudo systemctl status postgresql`

##### Vytvoření nové databáze

PostgreSQL je po instalaci již inicializován, pokud potřebujete vytvořit novou databázi, můžete to udělat následujícími kroky.:

##### Přepnutí na postgres uživatele

`sudo -i -u postgres`

##### Použití psql nástroje pro přístup do PostgreSQL
`psql`

##### Uvnitř psql terminálu pokud potřebujete si můžete vytvořit novou databázi:

`CREATE DATABASE mydatabase;`

Navraďte název mydatabase s vaším preferovaným názvem.

### Pro CentOS/RHEL/Fedora

##### Instalace PostgreSQL

Pro CentOS or RHEL je instalační proces odlišný. Použijte následující příkazy:

`sudo yum install postgresql-server postgresql-contrib`

On Fedoře, použijte správce balíčků dnf:

`sudo dnf install postgresql-server postgresql-contrib`

##### Post-Instalace

Před prvním spuštěním PostgreSQL je potřeba inicializovat databázi.:

`sudo postgresql-setup initdb`

##### Spuštění PostgreSQL služby

Po instalaci spusťte PostgreSQL službu.:

`sudo systemctl start postgresql`

##### Zastavení PostgreSQL služby

`sudo systemctl stop postgresql`

##### Vytvoření nové databáze

PostgreSQL služba je již inicializovaná, ale pokud potřebujete vytvořit novou databázi, můžete to udělat pomocí následujících příkazů.:
Postupujte stejně jako u Ubuntu/Debian – přepněte na uživatele postgres, spusťte psql a vytvořte novou databázi.
##### Přepnutí na postgres uživatele

`sudo -i -u postgres`

##### Použití psql nástroje pro přístup do PostgreSQL

`psql`

##### Uvnitř psql terminálu si vytvořte novou databázi

`CREATE DATABASE mydatabase;`

Navraďte název mydatabase s vaším preferovaným názvem.

### Nastavení WPF a EF projektu
1. Otevřete Visual Studio (nebo jej nainstalujte s rozšířením pro WPF).
2. Vytvořte nový WPF projekt s názvem **PokedexExplorer**.
3. Pomocí NuGet Package Manager nainstalujte balíčky Npgsql a Entity Framework.
4. Ve Solution Explorer pod PokedexExplorer, vytvořte novou složku **Models** pro tabulky a složku **Data** pro třídy pro práci s daty. Tento krok není povinný, ale pomáhá udržet projekt organizovaný. V nášem tutoriálu jsme tento krok aplikovali.

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

