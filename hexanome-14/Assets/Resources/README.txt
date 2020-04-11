Welcome to the README file for Info Gamer's PUN 2 Matchmaking add-ons

<File System>
InfoGamerAssets
>PUN2_Matchmaking
>>Demos
>>Prefabs
>>>CustomMatchmaking
>>>DelayStart
>>>QuickStart
>>Scripts
>>>CustomMatchmaking
>>>DelayStart
>>>QuickStart

<Included Files>
Scenes: 
CustomMatchmakingMenuDemo, DelayStartMenuDemo, DelayStartWaitingRoomDemo, MultiplayerSceneDemo, QuickStartMenuDemo
Prefabs:
(CustomMatchmaking) PhotonCustomMatchmaking, PlayerListing, RoomListing
(DelayStart) PhotonDelayStart, PhotonWaitingRoom
(QuickStart) PhotonQuickStart
Scripts:
(CustomMatchmaking) CustomMatchmakingLobbyController, CustomMatchmakingRoomController, RoomButton
(DelayStart) DelayStartLobbyController, DelayStartRoomController, DelayStartWaitingRoomController

<How to use>
To get started you will first need to have the PUN 2 plugin imported and set up in your multiplayer project.
You will then need to import at least one of our PUN 2 matchmaking add-ons. (Quick Start, Delay Start, Custom Matchmaking).
Set up the Build Order.
(QuickStart) QuickStartMenuDemo = 0, MultiplayerSceneDemo = 1
(DelayStart) DelayStartMenuDemo = 0, DelayStartWaitingRoomDemo = 1 MultiplayerSceneDemo = 2
(CustomMatchmaking) CustomMatchmakingMenuDemo = 0, MultiplayerSceneDemo = 1

View the demo scenes to get an idea of how this add-on should behave.

<Features>
Quick Start: This add-on will allow players to connect to a multiplayer room with one click of a button.

Delay Start: This add-on will allow players to load into a waiting room scene until more players connect. Once the max players connect or the timer runs out all connected players will load into the multiplayer scene.

Custom Matchmaking: This add-on will allow players to create their own rooms or join an existing room. Connected players will be waiting in a room menu until the master client clicks the start game button.

<Customize your project>
Depending on which add-on you need all you need to do is drag the correct prefabs into your project scenes and then customize the prefabs visuals.

Quick Start:
Drag the PhotonQuickStart prefab into your main menu scene.
Drag the GameSetup prefab into your multiplayer scene.
Delay Start:
Drag the PhotonDelayStart prefab into your main menu scene.
Drag the PhotonWaitingRoom prefab into your waiting room scene.
Drag the GameSetup prefab into your multiplayer scene.
Custom Matchmaking:
Drag the PhotonCustomMatchmaking prefab into your main menu scene.
Drag the GameSetup prefab into your multiplayer scene.

<Final Notes>
Feel free to change any visual but it is recommended to leave the overall structure of the prefabs alone. If you need additional features You will need to add them yourself.

make sure if you are using your own scenes that you follow the same order as described above with the demo scenes.

These plugins are only meant for matchmaking. Multiplayer game mechanics are not included in this plugin.