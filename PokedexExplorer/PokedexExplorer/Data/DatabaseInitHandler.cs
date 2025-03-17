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
            tableMax = 5;

            int abilityCount = PokeAPIFetcher.GetCount("ability");
            int moveCount = PokeAPIFetcher.GetCount("move");
            int pokemonCount = PokeAPIFetcher.GetCount("pokemon");
            int pokemonSpeciesCount = PokeAPIFetcher.GetCount("pokemon-species");
            int evolutionChainCount = PokeAPIFetcher.GetCount("evolution-chain");

            this.itemMax = abilityCount;
            this.tableProgress = 0;
            this.itemProgress = 0;
            for (int i = 0; i < abilityCount; i++)
            {
                Ability ability = PokeAPIFetcher.ParseAbility(PokeAPIFetcher.RetrieveJSON("ability", i));
                if (ability != null) this.context.Ability.Add(ability);
                this.itemProgress++;
            }

            this.itemMax = moveCount;
            this.tableProgress = 1;
            this.itemProgress = 0;
            for (int i = 0; i < moveCount; i++)
            {
                Move move = PokeAPIFetcher.ParseMove(PokeAPIFetcher.RetrieveJSON("move", i));
                if (move != null) this.context.Move.Add(move);
                itemProgress++;
            }

            this.itemMax = pokemonSpeciesCount;
            this.tableProgress = 2;
            this.itemProgress = 0;
            for (int i = 0; i < pokemonSpeciesCount; i++)
            {
                PokemonSpecies pokemonSpecies = PokeAPIFetcher.ParsePokemonSpecies(PokeAPIFetcher.RetrieveJSON("pokemon-species", i));
                if (pokemonSpecies != null) this.context.PokemonSpecies.Add(pokemonSpecies);
                itemProgress++;
            }

            this.itemMax = pokemonCount;
            this.tableProgress = 3;
            this.itemProgress = 0;
            int pokemonMoveIndex = 0;
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
                                this.context.PokemonMove.Add(pokemonMove);
                                pokemonMoveIndex++;
                            }
                        }
                    }
                }
                itemProgress++;
            }

            this.itemMax = evolutionChainCount;
            this.tableProgress = 4;
            this.itemProgress = 0;
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
                itemProgress++;
            }

            this.context.SaveChanges();
        }

    }
}
