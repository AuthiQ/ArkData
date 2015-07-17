using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ArkData.Test
{ 
    [TestClass]
    public class ArkDataAsync
    {
        private static ArkDataContainer container { get; set; }

        private const string Directory = "C:\\Users\\Admin\\Desktop\\ARK\\Tests";
        private const string APIKey = "48756D53FA2E53CCEB292FA347258723";
        private const string IP = "149.210.154.147";
        private const int Port = 27015;

        [TestMethod]
        public async Task ParsePlayersAsync()
        {
            container = await ArkDataContainer.CreateAsync(Directory);

            Assert.IsTrue(container.Players.Count > 0);
            Assert.IsTrue(container.Tribes.Count > 0);
        }

        [TestMethod]
        public async Task LoadSteamAsync()
        {
            await container.LoadSteamAsync(APIKey);

            Assert.IsTrue(container.Players[0].SteamName != null);
            Assert.IsTrue(container.Players[0].SteamName != String.Empty);
        }

        [TestMethod]
        public async Task LoadOnlinePlayersAsync()
        {
            await container.LoadOnlinePlayersAsync(IP, Port);

            Assert.IsTrue(container.Players.Any(p => p.Online));
        }
    }
}
