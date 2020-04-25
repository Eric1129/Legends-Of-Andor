using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SendChat :Action
{
    private string[] players;
    private Type type;
    string message;
    string nickname;

    public SendChat(string newmessage, string playerID, string playerNickName)
    {
        type = Type.SendChat;
        players = new string[] { playerID };
        message = newmessage;
        nickname = playerNickName;
    }

    public Type getType()
    {
        return type;
    }

    public string[] playersInvolved()
    {
        return players;
    }


    public bool isLegal(GameState gs)
    {
        return true;
    }
    public void execute(GameState gs)
    {
        // Chat.sendMessageToPlayers(message, nickname);
        string messageToBeSent = nickname + ": ";
        messageToBeSent += message + " \n";
        //string all_messages = chatText.text;
        //all_messages = String.Concat(all_messages, messageToBeSent);
        //chatText.text = all_messages;
        string all_messages = GameController.instance.chatText.text;
        all_messages = String.Concat(all_messages, messageToBeSent);
        GameController.instance.updateChatText(all_messages);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
