using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PokedexExplorer.Data
{
    public class DatabaseInitHandler
    {
        public static bool IsInitialized()
        {
            //TODO
            return false;
        }
        private static void InitializeTables()
        {
            //TODO
        }
        public static void Initialize()
        {
            if (IsInitialized())
            {
                return;
            }

            InitializeTables();
        }
    }
}
