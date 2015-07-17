using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SSQLib;

namespace ArkData
{
    public partial class ArkDataContainer
    {
        /// <summary>
        /// Loads the profile data for all users from the steam service
        /// </summary>
        /// <param name="apiKey">The Steam API key</param>
        public void LoadSteam(string apiKey)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Players.Count; i++)
                builder.Append(Players[i].SteamId + ",");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri("https://api.steampowered.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(string.Format("ISteamUser/GetPlayerSummaries/v0002/?key={0}&steamids={1}", apiKey, builder.ToString())).Result;
                if (response.IsSuccessStatusCode)
                    using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                    {
                        var profiles = JsonConvert.DeserializeObject<Models.SteamResponse<Models.SteamProfile>>(reader.ReadToEnd()).response.players;

                        for (var i = 0; i < profiles.Count; i++)
                        {
                            var player = Players.Single(p => p.SteamId == profiles[i].steamid);
                            player.SteamName = profiles[i].personaname;
                            player.ProfileUrl = profiles[i].profileurl;
                            player.AvatarUrl = profiles[i].avatar;
                        }
                    }
                else
                    throw new System.Net.WebException("The Steam API request was unsuccessful. Are you using a valid key?");

                response = client.GetAsync(string.Format("ISteamUser/GetPlayerBans/v1/?key={0}&steamids={1}", apiKey, builder.ToString())).Result;
                if (response.IsSuccessStatusCode)
                    using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                    {
                        var bans = JsonConvert.DeserializeObject<Models.SteamPlayerResponse<Models.SteamBan>>(reader.ReadToEnd()).players;
                        for (var i = 0; i < bans.Count; i++)
                        {
                            var player = Players.Single(p => p.SteamId == bans[i].SteamId);
                            player.CommunityBanned = bans[i].CommunityBanned;
                            player.VACBanned = bans[i].VACBanned;
                            player.NumberOfVACBans = bans[i].NumberOfVACBans;
                            player.NumberOfGameBans = bans[i].NumberOfGameBans;
                            player.DaysSinceLastBan = bans[i].DaysSinceLastBan;
                        }
                    }
                else
                    throw new System.Net.WebException("The Steam API request was unsuccessful. Are you using a valid key?");
            }
            SteamLoaded = true;
        }

        /// <summary>
        /// Fetches the player server status. Can only be done after fetching Steam player data.
        /// </summary>
        /// <param name="ipString">The IP of the server.</param>
        /// <param name="port">The port of the server.</param>
        public void LoadOnlinePlayers(string ipString, int port)
        {
            if (SteamLoaded)
            {
                var online = Enumerable.OfType<PlayerInfo>(new SSQL().Players(new IPEndPoint(IPAddress.Parse(ipString), port))).ToList();

                if (online.Count > 0)
                    for (var i = 0; i < online.Count; i++)
                    {
                        var online_player = Players.SingleOrDefault(p => p.SteamName == online[i].Name);
                        if (online_player != null)
                            online_player.Online = true;
                    }
            }
            else
                throw new System.Exception("The Steam user data should be loaded before the server status can be checked.");
        }

        /// <summary>
        /// Instantiates the ArkDataContainer and parses all the user data files
        /// </summary>
        /// <param name="directory">The directory containing the profile and tribe files.</param>
        public static ArkDataContainer Create(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("The ARK data directory couldn't be found.");

            var playerFiles = Directory.GetFiles(directory).Where(p => p.ToLower().Contains(".arkprofile")).ToArray();
            var tribeFiles = Directory.GetFiles(directory).Where(p => p.ToLower().Contains(".arktribe")).ToArray();

            if (playerFiles.Length == 0 && tribeFiles.Length == 0)
                throw new FileLoadException("The directory did not contain any of the parseable files.");

            var container = new ArkDataContainer();

            for (var i = 0; i < playerFiles.Length; i++)
                container.Players.Add(Parser.ParsePlayer(playerFiles[i]));

            for (var i = 0; i < tribeFiles.Length; i++)
                container.Tribes.Add(Parser.ParseTribe(tribeFiles[i]));

            for (var i = 0; i < container.Players.Count; i++)
            {
                var player = container.Players[i];
                player.OwnedTribes = container.Tribes.Where(t => t.OwnerId == player.Id).ToList();
                player.Tribe = container.Tribes.SingleOrDefault(t => t.Id == player.TribeId);
            }

            for (var i = 0; i < container.Tribes.Count; i++)
            {
                var tribe = container.Tribes[i];
                tribe.Owner = container.Players.SingleOrDefault(p => p.Id == tribe.OwnerId);
                tribe.Players = container.Players.Where(p => p.TribeId == tribe.Id).ToList();
            }

            return container;
        }
    }
}
