# Zřeknutí se odpovědnosti
Tento projekt využívá zdroje třetích stran. Jedná se o studentský projekt vytvořený pro vzdělávací účely. Není spojen, schválen ani nijak spojen s Nintendo, The Pokémon Company nebo PokéAPI.
### Informace o licencování
The Pokémon data in this project is sourced from PokéAPI, which is licensed under the BSD 3-Clause License. The full license can be found in the `LICENSE.txt` file.
Data o Pokémonech v tomto projektu pocházejí z PokéAPI, která jsou licencovány pod licencí BSD 3-Clause. Plné znění licence lze nalézt v souboru `LICENSE.txt`.
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
- ORM funkce: Mapuje databázové tabulky na C# objekty
- Podpora LINQ: Dotazy lze psát pomocí LINQ místo SQL
- Migrace: Snadná aktualizace databázového schématu při změně modelů
- Automatické sledování změn: Sleduje úpravy entit
- Nezávislost na databázi: Umí pracovat s různými databázemi jako SQL Server, MySQL, PostgreSQL atd.

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
*(Poznámka: Návod pro Linux byl vygenerován s pomocí AI. Přestože byla vynaložena maximální snaha o přesnost, kontent vygenerovaný pomocí AI může občas obsahovat chyby. Doporučujeme ověřit informace v [Official PostgreSQL documentation]. (https://www.postgresql.org/docs/) nebo v jiných důvěryhodných zdrojích pro dodatačn.)*
### Pro Windows

##### Stažení PostgreSQL

Stáhněte PostgreSQL z [oficiálních stránky]. (https://www.postgresql.org/download/). Vyberte verzi odpovídající vašemu operačnímu systému a stáhněte instalační balíček.

##### Instalace PostgreSQL

Postupujte podle pokynů instalačního programu. Během instalace si poznamenejte cestu k instalaci (např. `C:\Program Files\PostgreSQL\<verze>`).

Výchozí přihlašovací údaje jsou:
- Uživatelské jméno: postgres
- Heslo: postgres (Nebo heslo, které si nastavíte během instalace.)

##### Post-Instalace
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
ORM (Object-Relational Mapping) umožňuje dva hlavní přístupy k správě propojení mezi aplikačním kódem a databází: code-first a database-first

### Code-First
Definice: V code-first přístupu je struktura databáze (tabulky, relace apod.) definována přímo v kódu aplikace pomocí tříd a anotací. ORM služba pak automaticky vygeneruje schéma databáze odpovídající tomuto kódu.

Použití: Tento přístup je vhodný pro nové projekty, kde databáze ještě neexistuje, nebo když je prioritou nejprve navrhnutí obchodní logiky aplikace.

Příklad: Definujeme třídu Pokemon v kódu a ORM potom vygeneruje odpovídající Pokemon tabulku v databázi.
### Database-First
Definice: V database-first přístupu začínáme s již existujícím databázovým schématem. ORM poté vygeneruje nezbytný aplikační kód (např. odpovídající třídy), které mapují databázové tabulky na objekty v aplikaci.

Příklad: ORM načte existující `Pokemon` tabulku a vygeneruje odpovídající `Pokemon` třídu pro použití v aplikaci.

Použití: Vhodný pro práci s již existující databází nebo když databázové schéma je předdefinované a nelze výrazně měnit.

### Použitý přístup
Tento projekt ukazuje použití code-first přístupu v ORM, kde je schéma databáze definováno přímo v kódu aplikace. Tento přístup zajišťuje snadnější správu schématu a integraci s obchodní logikou aplikace.

Pokud by již existuje databáze naplněná daty, může být použit database-first přístup. V tomto případě ORM načte již existující databázové schéma a propojí je s aplikaci bez potřeby ručního vytváření tabulek a dat nebo jejich záznamu. Filtrovací aplikace se umí bez problému připojit a interagovat s již existující databází
# Vytvoření subtřídy DbContext
Nejdůležitější částí je připojení k databázi, aby bylo možné provádět dotazy.

### Connection string

**Connection string** je řetězec, který definuje, jak se aplikace má připojit k databázi. Obsahuje informace, pomocí kterých potřebuje aplikace potřebuje vytvořit připojení jako adresa serveru, název databáze, přihlašovací údaje a další parametry:

`Host=<server_address>;Port=<port>;Username=<user>;Password=<password>;Database=<database_name>;`

V našem případě použijeme výchozí nastavení:

`Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;`

### DbContext třída

Tato třída slouží jako propojení k databázi a budeme ji používat, kdykolikov při práci s daty v databázi.

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

# Definování tabulek
*(Poznámka: Tato část vyžaduje použití code-first přístupu. Pokud již databáze existuje, potřebuje pouze spojit datové typy a řádky s odpovídajícími třídami.)*

Jako první krok potřebujem definovat tabulky vytvořením tříd odpovídajících tomu, jak chceme, aby naše tabulka vypadala.
### Anotace

Nejdříve se pojďme seznámit s anotací.

#### [Key]
Tato anotace se používá pro označení primárního klíče.

#### [ForeignKey(“Table”)]
ForeignKey anotace se používá odkazování na jinou tabulku přes její primární klíč. Řetězec nám specifikuje k jaké tabulce to odkazuje.

#### [Required]
Required anotace se používá pro označení non-null hodnoty, že hodnota nesmí být null.

### Tabulky
V tomto tutoriále použijeme následující tabulky `Ability`, `Move`, `Pokemon`, `PokemonSpecies`, `PokemonMove` a `EvolutionSpecies`. Přičemž ke každé přidáme odkazy v `PokemonDbContext` třídě. Také si ukážeme něco o Pokémoních technikách v Pokémon hrách, ale nejsou důležité pro tento tutoriál.

#### Ability
Ability je jednoduchá tabulka, která uchovává data o schopnostech Pokémonů.

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
Tabulka Move obsahuje seznam útoků, které Pokémon může provádět.

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
Tabulka Pokemon obsahuje informace o různých Pokémonech.

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
PokemonSpecies tabulka obsahuje informace o druzích Pokémonů. Jeden druh může obsahovat více Pokémonů. Například Pikachu má různé verze, které se liší atributy a statistikami.
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
EvolutionChain tabulka obsahuje informace o evolučních řetězcích Pokémonů. Pokémon může evolvovat do různých forem, ale vždy jen z jednoho konkrétního Pokémona. Proto je primárním klíčem sloupec `EvolvesTo`.

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
PokemonMove tabulka reprezentuje vztah N:M mezi Pokémonem a útokem(Move), který se může naučit. Obsahuje také další informace o způsobu, jakým se může Pokémon naučit útok. Tato tabulka propojuje tabulky Pokemon a Move

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
### Indexy
Pro účely vyhledávání je užitečné indexovat sloupce, protože to urychlí vyhledávání. Například při hledání podle jména schopnosti (`Ability.Name`) dává smysl přidat indexování, což umožní rychlejší vyhledávání. Můžeme k tomu použít anotaci:

```[Index(nameof(Ability.Name), IsUnique = true, Name = "IndexAbilityName")]```to the class.

#### Ability (Schopnost)
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

#### Move (Útok)
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

#### PokemonSpecies (Druh Pokémona)
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

### Aktualizace třídy PokemonDbContext
Nyní, když máme definované třídy, musíme aktualizovat PokemonDbContext třídu. Je důležité dávat pozor na cizí klíče, protože odpovídající(referencované) tabulky k nim musí být vytvořeny jako první. Z tohoto důvodu budeme tabulky vytvářet v následujícím pořadí.:
1. Ability
2. Move
3. PokemonSpecies
4. Pokemon (odkazuje na PokemonSpecies a Ability)
5. EvolutionChain (odkazuje na Pokemon)
6. PokemonMove (odkazuje na Pokemon a Move)

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

### Vytvoření databáze
Nyní budeme potřebovat vytvořit samotnou databázi na serveru. Dosud jsme pouze modelovali schémata.

#### Migrace
Abychom synchronizovali náš databázový model s PostgreSQL, můžeme použít metodu `DbContext.Database.Migrate();`. Tato metoda aktualizuje tabulky. `Migrate()` si poradí s existujícími tabulkami, ale pokud tabulka existuje v jiné podobě, vyvolá výjimky.
```csharp
public MainWindow()
{
    InitializeComponent();
    context = new PokemonDbContext("skyre", "");

    context.Database.Migrate();
}
```
#### Raw SQL
Můžeme také vygenerovat a spustit SQL příkazy manuálně.:

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
V naší třídě MainWindow, která se inicializuje při spuštění WPF aplikace, přidáme do ní následující kód, který se provede při startu aplikace.
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

# Získávání dat z PokéAPI
*(Poznámka: Tato sekce předpokládá, že používáte code-first přístup zaměřený na kód. Tato sekce není důležitou součástí našeho tutoriálu, takže můžete jednoduše zkopírovat veškerý kód. Tento kód bude běžet pomalu. Také nedoporučujeme spouštět jej příliš často, protože se připojuje k serveru třetí strany. Nechceme způsobit žádné problémy týmu PokéAPI.)*

PokéAPI používá databázi typu NoSQL. Budeme ji muset přeformátovat tak, aby odpovídala naší struktuře tabulek v PostgreSQL. Nyní, když máme definované tabulky, vytvoříme speciální tříu pro získávání a přeformátování dat. Pro výstup budeme používat naše definované třídy.

### Buďte opatrní
Buďte opatrní, protože databáze PostgreSQL vyžaduje, aby primární klíč jedné tabulky existoval předtím, než na něj bude odkazovat jiná tabulka. Kvůli tomu to zjednodušíme a nebudeme používat vlákna.

### Ověření správnosti
V následujících sekcích vysvětíme, jak získat data. Prozatím můžete ke kontrole kódu použít nástroje jako pgAdmin, který je součástí PostgreSQL.

## Struktura třídy
Tuto třídu rozdělíme na dvě části:
- Získávání dat z PokéAPI
- Zpracování a přeformátování dat

### PokeAPIFetcher
Vytvoříme třídu `PokeAPIFetcher` ve složce `Data`, která bude stahovat a zpracovávat data a vracet objekty ve formátu našich definovaných tříd modelu. Tato část kódu není důležitá pro náš tutoriál, takže můžete jednoduše zkopírovat-vložít finální kód. Stejně si ho však vysvětíme.

#### Získávání dat
PokéAPI používá JSON formát s databází typu NoSQL. Tento formát je vhodný pro uchovávání komplexních dat o Pokémonech. My jej však zpracujeme a odstraníme data, která pro nás nejsou důležitá. Také si ukážeme, jak přidávat data do naší databáze.

##### Získání objektu JSON
Použijeme tuto jednoduchou metodu pro získání JSON souboru. Využívá strukturu PokéAPI složek.:
`https://pokeapi.co/api/v2/<table>/[<id>]`

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

##### Získání zdrojové ID
Metoda `GetURLIntValue` jednoduše získá index z URL.

```csharp
static private int GetURLIntValue(string url)
{
    string[] split = url.Split('/');
    return int.Parse(split[split.Length - 1]);
}
```

##### Získání záznamů
Pomocí předchozí metody můžeme začít se zpracováním dat. Nejprve chceme zjistit celkový počet záznamů.

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

#### Zpracování dat
Dále přidáme metody pro analýzu(parsování) JSON dat.

Metoda GetEnglishNode prochází(iteruje) jazykovou strukturou a vrátí anglickou verzi.
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

# Naplnění databáze daty
Teď, když máme data, můžeme začít naplňovat databázi. Použijeme k tomu naši další třídu `DatabaseInitHandler`. Vytvoříme třídu podobnou vláknu pro asynchronní stahování a zpracování.

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

#### Vkládání
Vložení záznamu do tabulky je jednoduché. Potřebujeme jen objekt a tabulku, do které jej vložíme. K tomu použijeme metodu `Run()`.

Vkládání záznamu se provádí pomocí `context.Table.Add(entry);`. Například schopnost můžeme do tabulky Ability vložit pomocí `context.Ability.Add(ability);`. Také využijeme metodu `AddRange(List<T>)`, která umožňuje vložit více hodnot najednou. Tyto změny se však projeví pouze v našem "# prostředí, takže je potřeba to potvrdit pomocí `context.SaveChanges();`.

##### Získání ID záznamů
Seznamy ID pro různé tabulky získáme pomocí metody `PokeAPIFetcher.GetEntries();`.

```csharp
List<int> abilityIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> moveIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> pokemonIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> pokemonSpeciesIndexes = PokeAPIFetcher.GetEntries("ability");
List<int> evolutionChainIndexes = PokeAPIFetcher.GetEntries("ability");
```

##### Ability, Move and PokemonSpecies
Naplnění tabulek Ability, Move a PokemonSpecies je jednoduché. Každý záznam se vytvoří jediným požadavkem na třídu PokeAPIFetcher. Pokud objekt existuje, vložíme ho do databáze. Nakonec změny uložíme, aby budoucí záznamy mohly na tyto záznamy tabulek odkazovat.

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
Pokemon a PokemonMove tabulky jsou vytvořeny z tabulky `pokemon` v PokéAPI. Tyto tabulky naplníme současně, ale záznamy do `PokemonMove` přidáme až po vložení záznamů do `Pokemon`, protože `PokemonMove` odkazuje na `Pokemon`. `PokemonMove` také odkazuje na `Move`, ale všechny záznamy jsou do `Move` již vloženy.

V `PokemonMove` je potřebujeme identifikovat podle odlišných ID, protože obě tabulky nemají stejné množství záznamů. Tohle je to k čemu se používá proměnná `pokemonMoveIndex`.

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

### Přidání UI
//TODO

Do souboru MainWindow.xaml přidáme následující kód, který zobrazí průběh stahování dat z PokéAPI.:
```xaml
<Grid Name="FetchGroup" Width="800" Height="600" Background="#DFFFFFFF" IsEnabled="{Binding Path=Handler.IsRunning, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Visibility="{Binding Path=Handler.UIVisibility, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" MouseDown="FetchGroupMouseDown">
    <StackPanel Orientation="Vertical" Width="720" Height="100" Background="#FFFFFF" Visibility="Visible">
        <TextBlock TextAlignment="Center" FontSize="20">Fetching Pokémon data from PokéAPI</TextBlock>
        <ProgressBar x:Name="TableProgressBar" Minimum="0" Maximum="{Binding Handler.TableMax, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Value="{Binding Handler.TableProgress, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Height="20" Width="700" Margin="0,10,0,0"/>
        <ProgressBar x:Name="ItemProgressBar" Minimum="0" Maximum="{Binding Handler.ItemMax, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Value="{Binding Handler.ItemProgress, RelativeSource={RelativeSource AncestorType=Window}, Mode=OneWay}" Height="20" Width="700" Margin="0,10,0,0"/>
    </StackPanel>
</Grid>
```
Dále aktualizujeme MainWindow.xaml.cs, kde přidáme instanci `DatabaseInitHandler`. Pokud je povolena inicializace tabulek (`INITIALIZE_TABLES`), pokusíme se je vytvořit. Pokud je povolena inicializace dat (`INITIALIZE_DATA`), spustíme inicializační proces.

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
Také aktualizujeme `DatabaseInitHandler` třídu, aby byl veřejný a implementoval `INotifyPropertyChanged`, což umožní aktualizaci uživatelského rozhraní. 

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
A nakonec spustíme aplikaci!

Teď by se všechna data Pokémonů měla stáhnout a být vložena do PostgreSQL databáze.

# Nastavení aplikace Pokédex Explorer
## Nastavení
*Poznámka: Kód můžete zkopírovat a vložit, ale měli byste alespoň vědět, co dělá.*
#### PokemonSearch
Nyní vytvoříme nový soubor třídy: `Data/PokemonSearch`. Tato třída bude obsahovat všechny naše parametry pro hledání a bude se sama aktualizovat.

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
Vytvoříme nové UI. Napíšeme XAML **PŘED** načtením UI.
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
A nakonec vše propojíme s metodami ve třídě MainWindow.
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
##### Aktualizace výsledků hledání
**Změna parametru v UI**: Jakákoli interakce s uživatelským rozhraním (např. změna filtru v rozevíracím seznamu, zadání textu do vyhledávacího pole) spustí aktualizaci objektu MainWindow.Search. Tento objekt obsahuje parametry vyhledávání, které uživatel upravuje prostřednictvím UI.

**Aktualizace vlastností v MainWindow.Search**: Změny v UI se odrážejí ve vlastnostech objektu MainWindow.Search. Tyto vlastnosti jsou navrženy tak, aby zachytily a uložily aktualizované hodnoty poskytnuté uživatelem.

**Volání PokemonSearch.UpdateQuery()**: Jakmile jsou vlastnosti v MainWindow.Search aktualizovány, zavolá se metoda PokemonSearch.UpdateQuery(). Tato metoda je zodpovědná za vytvoření nebo úpravu dotazu na základě aktuálních parametrů hledání. V této fázi je LINQ dotaz aktualizován, aby odrážel nejnovější kritéria poskytnutá uživatelem.

**Provádění MainWindow.OnQueryUpdated()**: Po aktualizaci dotazu se zavolá metoda MainWindow.OnQueryUpdated(). Tato metoda zajistí, že UI odráží změny pro instanci, například obnovením zobrazených výsledků nebo aktualizací dalších UI komponent závislých na výsledku dotazu.

# Vytváření dotazů
*Poznámka: Tato část se bude zabývat obsahem metody `PokemonSearch.UpdateQuery()`.*

Dotazy mají dvě fáze - vytvoření dotazu a jeho spuštění. Dotaz můžeme spustit pomocí metod jako `FirstOrDefault()` nebo `ToList()`.

## LINQ
LINQ (Language Integrated Query) je silná funkce v Entity Frameworku, která usnadňuje práci s daty. Umožňuje snadnější vytváření dotazů.

### Vytváření výchozího "prázdného" dotazu.
Pro získání dat z databáze začneme definováním dotazu, který slouží jako základ pro další vylepšení a provedení. V Entity Frameworku jsou dotazy vytvářeny pomocí LINQ a prováděny až po výslovném provedení. To znamená, že můžeme budovat složité dotazy krok za krokem, aplikovat filtry, řazení nebo projekce podle potřeby. V metodě `PokemonSearch.UpdateQuery()` začíná dotaz výchozím prohlášením, které odkazuje na dataset, se kterým chceme pracovat. Rozebíráme tento první krok.:

```csharp
private void UpdateQuery()
{
    IQueryable<Model.Pokemon> query = context.Pokemon;

    // Filters go here...
    
    this.window.OnQueryUpdated();
}
```
### Select
Po definování základního dotazu ho nyní přizpůsobíme, aby získal pouze sloupce, které potřebujeme pro zobrazení v uživatelském rozhraní. Následující ukázka kódu ukazuje, jak transformujeme výsledky dotazu do strukturovaného formátu vhodného pro UI.

Metoda `Select()` se používá k projekci dat do nové podoby nebo struktury. Místo toho, abychom získali celé objekty Pokémonů, tato projekce získává specifické vlastnosti: `Name`, `PrimaryType`, `SecondaryType` a `SpriteFrontDefault`.

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

To odpovídá naší definici UI:
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
V třídě MainWindow je ještě neobjasněná metoda `ToList()`. Ta spustí aktualní dotaz.
Zde je odpovídající kód.:
```csharp
public void OnQueryUpdated()
{
    Debug.WriteLine("Updated");
    List<PokemonGridData> data = Search.Query.ToList();
    if (data != null) PokemmonDataGrid.ItemsSource = data;
}
```

# Hledání a filtrování
###TODO
Abychom naše dotazy učinili flexibilnějšími a uživatelsky řízenými, zavádíme řadu filtrů, které umožňují dynamické získávání dat na základě specifických kritérií. Tyto filtry zpřesňují výsledky tím, že omezují dataset podle parametrů jako jméno, typ nebo jiné vlastnosti, které nás zajímají. Přidáním filtrů zajistíme, že uživatelé budou moci efektivně vyhledávat a interagovat s daty smysluplnými způsoby.

### Jméno
Pro implementaci jednoduchého filtru jména napíšeme dotaz, který zkontroluje, zda jméno Pokémona začíná zadaným hledaným řetězcem. Hledaný řetězec je uložen ve vlastnosti Name třídy PokemonSearch.

```csharp
query = query.Where(p => p.Name.StartsWith(Name));
```
Metoda Where v LINQ se používá k filtrování datasetu na základě specifikované podmínky. Bere jako argument predikát (funkci, která vrací Booleovskou hodnotu), a pouze prvky, které tuto podmínku splní, jsou zahrnuty do výsledného seznamu.

Tento jeden řádek LINQ kódu se přímo překládá do následujícího SQL dotazu.:
```sql
SELECT * FROM "Pokemon"
WHERE "Name" LIKE 'NamePrefix%';

```
Před použitím filtru také zajistíme, že hledaný řetězec byl poskytnut (tzn. že není null a není prázdný).:
```csharp
if (Name != null)
{
    query = query.Where(p => p.Name.StartsWith(Name));
}
```
### Typ
Dále můžeme přidat filtrování podle typu. Uživatel si může vybrat typ, a dotaz bude kontrolovat možné typování v tabulce.
```csharp
query = query.Where(p => p.PrimaryType == this.Type1 || p.SecondaryType == this.Type1);
```

Tento jeden řádek LINQ kódu se přímo překládá do následujícího SQL dotazu.:
```sql
SELECT * FROM "Pokemon"
WHERE "PrimaryType" = 'Type1' OR "SecondaryType" = 'Type1';

```

Dále přidáme druhý vybraný typ.:
```csharp
query = query.Where(p => p.PrimaryType == this.Type2 || p.SecondaryType == this.Type2);
```

A obalíme to kontrolami bezpečnosti.:
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

### Ability (Schopnosti)
Dále přidáme filtr podle schopností. To umožní uživatelům hledat Pokémony, jejichž schopnosti odpovídají zadaným kritériím.

V databázovém schématu má tabulka Pokemon tři sloupce - `PrimaryAbility`, `SecondaryAbility` a `HiddenAbility` - které odkazují na sloupec ID v tabulce Ability. Tento vztah je definován cizími klíči, které vytvářejí spojení mezi schopnostmi Pokémona a jejich odpovídajícími záznamy v tabulce Ability.
```csharp
query = query.Where(p => context.Ability.Any(a => (a.ID == p.PrimaryAbility || a.ID == p.SecondaryAbility || a.ID == p.HiddenAbility) && a.Name.StartsWith(this.Ability)));
```

Metoda `Any` v LINQ určuje, zda některý prvek v kolekci Ability splňuje zadanou podmínku. V tomto případě kontrolujeme, zda řádek ve tabulce Ability s `Ability.ID` odpovídá jedné z Pokémoních schopností (`PrimaryAbility`, `SecondaryAbility` nebo `HiddenAbility`). Pokud je shoda nalezena, dále kontrolujeme, zda `Ability.Name` začíná zadaným hledaným řetězcem (`this.Ability`).

A tuto řádku obalíme kontrolami bezpečnosti.:
```csharp
if (this.Ability != null && this.Ability.Length > 0)
{
    query = query.Where(p => context.Ability.Any(a => (a.ID == p.PrimaryAbility || a.ID == p.SecondaryAbility || a.ID == p.HiddenAbility) && a.Name.StartsWith(this.Ability)));
}
```

### Move (Pohyby)
Tento kód přidává filtrování na základě pohybů Pokémonů. Kontroluje, zda některý pohyb spojený s Pokémonem odpovídá zadanému vyhledávacímu řetězci uloženému v `this.Move`.

Tabulka PokemonMove funguje jako M:N relace mezi Pokémonem a pohybem.:
```
PokemonMove.Pokemon → Pokemon.ID
PokemonMove.Move → Move.ID
```
Protože chceme vrátit objekt Pokémon, začneme s tabulkou Pokemon, identifikujeme všechny záznamy v tabulce PokemonMove, kde sloupec Pokemon odpovídá ID Pokémona. Poté hledáme záznamy v tabulce Move, kde `Move.ID` odpovídá sloupci Move v tabulce PokemonMove a kontrolujeme, zda `Move.Name` splňuje podmínku vyhledávání. Tímto způsobem zajistíme, že získáme pouze Pokémony spojené s pohyby, které splňují zadaná kritéria.
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

### Vyzkoušete si to sami
Nyní si to můžete vyzkoušet sami.

### Generace
```
Pokemon.Species → PokemmonSpecies.ID
```
Chceme zjistit, zda náš integer `Generation` odpovídá hodnotě `PokemonSpecies.Generation`.
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
Máme parametr vyhledávání `LegendaryStatus`, který může mít hodnoty:
```
Any
Legendary
Mmythical
None
```
Tabulka PokemonSpecies má dva boolean sloupce:

```
IsLegendary
IsMythical
```
Chceme zjistit, zda náš řetězec `LegendaryStatus` odpovídá hodnotám `PokemonSpecies.IsLegendary` a `PokemonSpecies.IsMythical`.
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

### Vzhled: Barva a Tvar
Máme parametry vyhledávání `PokemonSpecies.AppearanceColor` a `PokemonSpecies.AppearanceShape`, které mají odpovídající sloupce v tabulce PokemonSpecies.
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


### Vzhled: Výška a Váha
Máme parametry vyhledávání `AppearanceHeightMin`, `AppearanceHeightMax`, `AppearanceWeightMin` a `AppearanceWeightMax`, které mají odpovídající sloupce v tabulce Pokemon.

Chceme zkontrolovat, zda:
- Parametr není null
- Vstupní hodnoty v tabulce Pokémon odpovídají těmto parametrům:
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

### Staty: HP, Attack, Defense, Special Attack, Special defense and Speed
Nakonec zkontrolujem rozsahy statistik.:
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



