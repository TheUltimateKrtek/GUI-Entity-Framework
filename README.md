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
Create a new WPF project.
Install the Npgsql and Entity Framework libraries using the NuGet Package Manager.
In the Solution Explorer, create a new folder Models. We will write our tables here. This step is not required, but it helps keep the project clean. Our tutorial will assume this step was taken.

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
using System.Data.Entity;
using Npgsql;

public class PokemonDbContext : DbContext
{
    public PokemonDbContext() : base("name=Host=localhost;Port=5432;Username=postgres;Password=postgre;Database=Pokemon;
") {
        
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
#TODO: Insert code here
#### Move
The Move table contains a list of moves that a Pokémon can perform.
#TODO: Insert code here
#### Pokemon
The Pokémon table contains information about the various Pokémon.
#TODO: Insert code here
#### PokemonSpecies
The PokemonSpecies table contains information about the Pokémon species. Note, that a species may contain multiple pokémon. An obvious example is Pikachu with its various versions, each having different attributes and stats.
#TODO: Insert code here
#### EvolutionChain
The EvolutionChain table includes information about a Pokémon’s evolution chain. Pokémon can evolve into various Pokémon, but a Pokémon can only evolve from one other Pokémon. Because of this, the primary key will be the EvolvesTo column.
#TODO: Insert code here
#### PokemonMove
This table represents our many-to-many relation between a Pokémon and a move it can learn. It will also contain additional information about the way a Pokémon can learn a move. This table connects the Pokemon and the Move tables.
#TODO: Insert code here
### Indexes
For the purpose of searching, indexing columns will be beneficial. It will speed up search.

### Updating the PokemonDbContext class
Now, that we have our classes, we have to update the PokemonDbContext class. Be careful, as foreign keys require the referenced table to be created first. Because of this, we will be creating these tables in the following order:
- Ability
- Move
- PokemonSpecies
- Pokemon (references PokemonSpecies and Ability)
- EvolutionChain (references Pokemon)
- PokmeonMove (references Pokemon and Move)

We also need to create the tables in the actual database.
#TODO: Insert code here
