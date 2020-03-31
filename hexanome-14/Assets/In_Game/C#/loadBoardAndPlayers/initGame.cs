using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Photon.Pun;

public class initGame : MonoBehaviour
{
    [SerializeField]
    private GameObject baseCamera;
    [SerializeField]
    private GameObject baseObject;
    [SerializeField]
    private Sprite fullBoardSprite;
    [SerializeField]
    private SpriteAtlas heroAtlas;



    private string createMasterClass()
    {
        GameObject masterClassObject = Instantiate(baseObject, transform.position, transform.rotation);
        masterClassObject.name = "Master";
        masterClassObject.tag = "Master";

        masterClassObject.AddComponent<masterClass>();
        masterClass m = masterClassObject.GetComponent<masterClass>();
        m.enabled = true;
        masterClassObject.AddComponent<BoardPosition>();

        return masterClassObject.tag;
    }


    /* think solution is to create a function in turnmanager which returns
     * init position based on photon players passing in their player #
     * player # obtained by iterating over current room, if player.value = this
     * then player# = player.Key
     *
     *
     */
    private void createPlayers()
    {
        // loop over the players in room and call setTag()
        // foreach(string playerTag in initialPlayerOrder())
        string[] tags = initialPlayerOrder();
        int i = 0;
        // foreach(KeyValuePair<int,Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        foreach(Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            // GameObject playerObject = Instantiate(baseObject, transform.position, transform.rotation);
            GameObject playerObject = (GameObject) Resources.Load("Player");
            // playerObject.AddComponent<Andor.Player>();
            playerObject.tag = setHeroType(tags[i]);

            // Andor.Player player = playerObject.GetComponent<Andor.Player>();
            // player.Value.tag = setHeroType(tags[i]);
            i++;
        }
    }


    private void setCameraPosition(Vector3 cameraPosition)
    {
        baseCamera.transform.position = cameraPosition;
    }


    private void createBoard(string masterTag)
    {

        GameObject fullBoard = Instantiate(baseObject, transform.position, transform.rotation);
        fullBoard.name = "full-Board";

        GameObject masterClassObject = GameObject.FindWithTag(masterTag);
        // masterClass is a child of fullBoard so that the sprites locations
        // are relative to an image of the entire board.
        masterClassObject.transform.parent = fullBoard.transform;

        fullBoard.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = fullBoard.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = fullBoardSprite;
        // whatever is drag n dropped as ^ is shown as board img (but import it as a sprite)
    }

    private string[] initialPlayerOrder()
    {
        return new string[]{
            "Player-Male-Wizard",
            "Player-Male-Dwarf",
            "Player-Male-Archer",
            "Player-Male-Warrior",
        };
    }

    private string[] getInitialPositions()
    {
        return new string[]{
            "0",
            "1",
            "2",
            "3",
        };
    }

    void Start()
    {
        setCameraPosition(new Vector3(0f, 0f, -65f));
        // GameObject tagHandler = Instantiate(baseObject, transform.position, Quaternion.identity);
        // tagHandler.AddComponent<CreateTagList>();
        string masterTag = createMasterClass();
        createBoard(masterTag);

        createPlayers();

        GameObject masterClassObject = GameObject.FindWithTag(masterTag);
        masterClass master = masterClassObject.GetComponent<masterClass>();
        master.initMasterClass("MainCamera", heroAtlas, baseObject, initialPlayerOrder(), getInitialPositions());
    }

    private string setHeroType(string myTag)
    {
        // must initialize before adding strings
        int ct = 0;
        string[] partsOfTag = myTag.Split('-');
        if (partsOfTag.Length != 3)
        {
            Debug.Log("Found a bad tag in Player.setHeroType: " + myTag);
        }

        return partsOfTag[1] + "-" + partsOfTag[2];
        //foreach(string partOfTag in myTag.Split('-'))
        //{
        //    // skip first val: "Player"
        //    if (ct++ == 0) continue;

        //    heroType += partOfTag;
        //}
    }
}
