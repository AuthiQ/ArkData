using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    public partial class ArkDataContainer
    {
        /// <summary>
        /// A list of all players registered on the server.
        /// </summary>
        public List<Player> Players { get; set; }
        /// <summary>
        /// A list of all tribes registered on the server.
        /// </summary>
        public List<Tribe> Tribes { get; set; }
        /// <summary>
        /// Indicates whether the steam user data has been loaded.
        /// </summary>
        private bool SteamLoaded { get; set; }

        /// <summary>
        /// Constructs the ArkDataContainer.
        /// </summary>
        public ArkDataContainer()
        {
            Players = new List<Player>();
            Tribes = new List<Tribe>();
            SteamLoaded = false;
        }
    }
}
