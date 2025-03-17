using PokedexExplorer.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            tableMax = 6;
            int abilityCount = PokeAPIFetcher.GetCount("ability");
            int moveCount = PokeAPIFetcher.GetCount("move");
            int pokemonCount = PokeAPIFetcher.GetCount("pokemon");
            int pokemonSpeciesCount = PokeAPIFetcher.GetCount("pokemon-species");
            int evolutionChainCount = PokeAPIFetcher.GetCount("evolution-chain");

            for (int i = 0; i < abilityCount; i++)
            {
                this.context.Ability.Add();
            }
        }

    }
}
