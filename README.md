![alt text][logo]
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
<h3>Cross-platform compatibility</h3>

The sources of all the dependencies are available and no unsupported features are used in the development of ArkData it can be compiled with <a href="http://www.mono-project.com/docs/about-mono/languages/csharp/">Mono</a>. You might have to change a line or two to compensate for environmental differences. The core mechanics should work under Linux and Mac OS X, although this is untested.

<h3>Third-party Sources</h3>

- <a href="https://github.com/JamesNK/Newtonsoft.Json">NewtonSoft.Json</a>
- <a href="https://code.google.com/p/ssqlib/">SSQ Lib</a>


[logo]: http://web.mxcontent.network/arkdata/ArkData.png "ArkData Logo"
