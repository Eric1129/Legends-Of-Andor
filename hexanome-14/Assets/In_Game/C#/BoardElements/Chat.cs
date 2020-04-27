using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class chat : MonoBehaviourPun
{
    public ScrollRect scrollRect;
    public Button button;
    public InputField input;
    PhotonView PV;
    //public string messages;
    static Photon.Realtime.RaiseEventOptions sendToAllOptions = new Photon.Realtime.RaiseEventOptions()
    {
        CachingOption = Photon.Realtime.EventCaching.DoNotCache,
        Receivers = Photon.Realtime.ReceiverGroup.All
    };

    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        button = GetComponent<Button>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buttonIsClicked()
    {
        Debug.Log("chat button clicked");
        string message = input.text;
        Debug.Log("got the input");
        object[] data = { message, photonView.ViewID, PhotonNetwork.LocalPlayer.NickName };
        Debug.Log("sent the data");
        PhotonNetwork.RaiseEvent((byte)53, data, sendToAllOptions, SendOptions.SendReliable);
    }

    public void sendMessageToPlayers(string Message, string playerNickname)
    {
        string messageToBeSent = playerNickname + ": ";
        messageToBeSent += Message + " \n";
        //messages += messageToBeSent;
        Debug.Log("reached boo");
        GameObject text = GameObject.Find("/Canvas/GameContainer/Chat/Scroll View/Viewport/Text");
        Debug.Log("reached boo 2");
        string all_messages = text.GetComponent<Text>().text;
        all_messages = String.Concat(all_messages, messageToBeSent);
        text.GetComponent<Text>().text = all_messages;
        //text.GetComponent<Text>().text = messageToBeSent;
    }

    //  =============== NETWORK SYNCRONIZATION SECTION ===============
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData eventData)
    {
        byte evCode = eventData.Code;

        //0: placing a firefighter
        if (evCode == (byte)53)
        {
            object[] receivedData = eventData.CustomData as object[];
            string message = (string)receivedData[0];
            int viewID = (int)receivedData[1];
            string PlayerNickname = (string)receivedData[2];

            if (viewID == base.photonView.ViewID)
            {
                sendMessageToPlayers(message, PlayerNickname);
            }
        }
    }
}