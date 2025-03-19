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

namespace PokedexExplorer.Data
{
    public class DatabaseInitHandler : INotifyPropertyChanged
    {
        private MainWindow window;
        private PokemonDbContext context;
        private Thread thread;
        private int tableProgress, tableMax, itemProgress, itemMax;
        private Visibility uiVisibility;
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

            int abilityCount = PokeAPIFetcher.GetCount("ability");
            int moveCount = PokeAPIFetcher.GetCount("move");
            int pokemonCount = PokeAPIFetcher.GetCount("pokemon");
            int pokemonSpeciesCount = PokeAPIFetcher.GetCount("pokemon-species");
            int evolutionChainCount = PokeAPIFetcher.GetCount("evolution-chain");

            //Ability
            this.ItemMax = abilityCount;
            this.TableProgress = 0;
            this.ItemProgress = 0;
            for (int i = 0; i < abilityCount; i++)
            {
                Debug.WriteLine(i + " / " + abilityCount);
                Ability ability = PokeAPIFetcher.ParseAbility(PokeAPIFetcher.RetrieveJSON("ability", i + 1));
                if (ability != null) this.context.Ability.Add(ability);
                this.ItemProgress++;
            }

            //Move
            this.ItemMax = moveCount;
            this.TableProgress = 1;
            this.ItemProgress = 0;
            for (int i = 0; i < moveCount; i++)
            {
                Move move = PokeAPIFetcher.ParseMove(PokeAPIFetcher.RetrieveJSON("move", i + 1));
                if (move != null) this.context.Move.Add(move);
                ItemProgress++;
            }

            //PokemonSpecies
            this.ItemMax = pokemonSpeciesCount;
            this.TableProgress = 2;
            this.ItemProgress = 0;
            for (int i = 0; i < pokemonSpeciesCount; i++)
            {
                PokemonSpecies pokemonSpecies = PokeAPIFetcher.ParsePokemonSpecies(PokeAPIFetcher.RetrieveJSON("pokemon-species", i + 1));
                if (pokemonSpecies != null) this.context.PokemonSpecies.Add(pokemonSpecies);
                ItemProgress++;
            }

            //Save changes
            this.context.SaveChanges();

            //Pokemon
            this.ItemMax = pokemonCount;
            this.TableProgress = 3;
            this.ItemProgress = 0;
            int pokemonMoveIndex = 1;
            List<PokemonMove> storedPokemonMoves = new List<PokemonMove>();
            for (int i = 0; i < pokemonCount; i++)
            {
                JObject node = PokeAPIFetcher.RetrieveJSON("pokemon", i + 1);
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

            //EvolutionChain
            this.ItemMax = evolutionChainCount;
            this.TableProgress = 4;
            this.ItemProgress = 0;
            int evolutionChainIndex = 1;
            for (int i = 0; i < evolutionChainCount; i++)
            {
                List<EvolutionChain> evolutionChains = PokeAPIFetcher.ParseEvolutionChain(PokeAPIFetcher.RetrieveJSON("evolution-chain", i + 1));
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

            this.UIVisibility = Visibility.Visible;
            this.IsRunning = false;
        }
    }
}
