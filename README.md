# Legends-Of-Andor

<img src="https://raw.githubusercontent.com/Eric1129/Legends-Of-Andor/Re-format/AndorAssets/Andor_HP_Art_07_01.jpg" width="400" height="300">

## Overview
This is an one-year group project from COMP 361 Software Engineering Project course.

It creates a PC version of a board game [Legends Of Andor](http://legendsofandor.com). 

## Table of contents
1. [Background](#background)
2. [Installation](#installation)
3. [Contributors](#contributors)
4. [Important Features](#important-features)

## Background
This is a multiplayer online game and follows all original game rules.

Game rules can be found here: 

- [Game Manual](AndorAssets/Andor_Manual.pdf)
- [Quick Start Guide](AndorAssets/Andor_QuickStart.pdf).

The whole development follows waterfall model. It's been through 8 phases:

- [User Interface Sketch](Milestones/M1_UI_Sketch.pdf)
- [Requirement Elicitation with Use Cases](Milestones/M2_Use_Cases.pdf)
- Requirements Specification Models 
  - [Environment Model](Milestones/M3_Environment_Model.pdf)
  - [Operation Model](Milestones/M3_Operation_Model.pdf)
  - [Concept Model](Milestones/M3_Concept_Model.pdf)
  - [Protocol Model](Milestones/M3_Protocol_Model.jucm)
- Pre-Demo
- [Design Models](Milestones/M5_Design_Models.zip)
- Demo
- Acceptance Test
- Maintainace

## Installation
This software builds on Unity version 2019.2.11f1.

The online function is based on Photon PUN. You may need a wizard id to play this game. 

## Contributors
- [@Eric1129](https://github.com/Eric1129) 
- [@anahita-m](https://github.com/anahita-m)
- [@MaxMB15](https://github.com/MaxMB15)
- [@ailishm](https://github.com/ailishm)
- [@Brendonk13](https://github.com/Brendonk13)
- [@CWright44](https://github.com/CWright44)
- [@IanXTs](https://github.com/IanXTs)

## Important Features

### Menu

![Menu](https://github.com/Eric1129/Legends-Of-Andor/raw/master/AndorAssets/MenuScene.png)

### Create Game Room

You need to name your room, choose room type as private or public, and choose difficulty. 

Once click play room, the room is created and will display in the lobby for other players to join.

![Create Room](./AndorAssets/CreateRoomScene.png)

### Game Lobby

![GameLobby](AndorAssets/GameLobby.png)

### In Room

You'll need 2 to 4 people to start a new game. You have to wait until other player join the room. Every player need to choose a hero, different hero has different ability.

You may start if all other players are ready.

![InRoom](https://github.com/Eric1129/Legends-Of-Andor/raw/master/AndorAssets/InRoom.png)

### In Game

You can move, fight, chat and trade 

Legend task shows all tasks players have to finish. 

Up-right corner shows all hero stats. 

![InGame](https://github.com/Eric1129/Legends-Of-Andor/raw/master/AndorAssets/TimeMark.png)

#### Time Mark

On the top of the game map there are sunrise box and time track. 

Each player has its own color, every time player performes a time-consumed action, players time mark will move accordingly.

#### move

You can only move your hero when it's your turn, and only one move per turn. Each region will consume one hour.
As long as you have enough hours, you can continue move when it's your turn again. 

#### fight

You can fight to a monster if your hero stand on same region as the monster's. 

If more than one heroes stand on same region. They can fight collabratively.

Player can choose fight alone or fight 

![InviteFight](AndorAssets/InviteFight.png)
![FightInvitation](AndorAssets/FightInvitation.png)
![InFight](AndorAssets/InFight.png)
![Rewards](AndorAssets/Rewards.png)

#### chat

There is a pop-up chat window on the side which you can chat with other users freely no matter it's your turn or not. 

#### trade

![Trade](AndorAssets/Trade.png)

#### buy

When player moves to a region where there is a merchant legend on it. There will be a merchant button on the side which allows player buy stuff.
![Merchant](AndorAssets/Merchant.png)
