using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PokedexExplorer.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokedexExplorer.Data
{
    public class PokemonDbContext : DbContext
    {
        private string name, password;
        public PokemonDbContext(string name, string password) : base()
        {
            this.name = name;
            this.password = password;
        }
        public DbSet<Ability> Ability { get; set; }
        public DbSet<Move> Move { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonSpecies> PokemonSpecies { get; set; }
        public DbSet<EvolutionChain> EvolutionChain { get; set; }
        public DbSet<PokemonMove> PokemonMove { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=" + this.name + ";Password=" + password + ";Database=postgres;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ability>().ToTable("Ability");
            modelBuilder.Entity<Move>().ToTable("Move");
            modelBuilder.Entity<Pokemon>().ToTable("Pokemon");
            modelBuilder.Entity<PokemonSpecies>().ToTable("PokemonSpecies");
            modelBuilder.Entity<EvolutionChain>().ToTable("EvolutionChain");
            modelBuilder.Entity<PokemonMove>().ToTable("PokemonMove");
        }
    }
}
