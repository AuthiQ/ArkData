# ArkData - ARK Dedicated Server data reader



The core mechanics are based on <a href="http://steamcommunity.com/app/346110/discussions/0/594821545173979380/">Player & Tribe Viewer</a> made by <a href="http://steamcommunity.com/id/cssjunky">[xU] .$pIrit</a>
we wanted to make the mechanics more easily implementable into other components.

<h3>How to use it</h3>

ArkData is very easy to use:

```C#
var container = ArkData.ArkDataContainer.Create("Saved data directory");

txtName.text = container.Players[0].CharacterName;
txtLevel.text = container.Players[0].Level;
```

If you want to use the extended information such as the steam profile data you'll have to load the steam information for the players.
To use the Steam functionalities you need to have an access key to use with the Steam API. You can create one <a href="http://steamcommunity.com/dev/apikey">here</a>.
Steam data can be loaded like this:

```C#
container.LoadSteam("API Key");

txtSteamName.text = container.Players[0].SteamName;
txtProfileURL.text = container.Players[0].ProfileUrl;
```

Last but not least we have the functionality to check who of the players is online. To use this function you need to have loaded the users Steam data. This is necessary to bind the online users to their server profiles.

```C#
container.LoadOnlinePlayers("127.0.0.1", 27015);

txtOnline.text = container.Players[0].Online;
```

<h3>Using the async pattern</h3>

All the mechanics work precisely the same but have been added into an async pattern.

```C#
var container = await ArkData.ArkDataContainer.CreateAsync("Saved data directory");

txtName.text = container.Players[0].CharacterName;
txtLevel.text = container.Players[0].Level;

await container.LoadSteamAsync("API Key");

txtSteamName.text = container.Players[0].SteamName;
txtProfileURL.text = container.Players[0].ProfileUrl;

await container.LoadOnlinePlayersAsync("127.0.0.1", 27015);

txtOnline.text = container.Players[0].Online;
```
