using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public enum Type
{
    Move,
    PassTurn,
    EndTurn,
    InitiateTrade,
    RespondTrade,
    BuyFromMerchant,
    MovePrinceThoald,
    EmptyWell,
    BuyBrew,
    SendChat,
    UseTelescope,
    InviteFighter,
    RespondFight,
    UseWineskin,
    StartFight,
    EndFight,
    UseHelm,
    UseWitchBrew,
    UseBow,
    UseShield,
    FightTurn,
    ExitFight,
    JoinNextRound,
    CreatureTurn,
    WinBattle,
    DistributeReward,
    GetMyReward

}

public interface Action
{
    Type getType();
    string[] playersInvolved();

    bool isLegal(GameState gs);
    void execute(GameState gs);

}


