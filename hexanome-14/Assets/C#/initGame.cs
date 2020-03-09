using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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


    private void createPlayers()
    {
        foreach(string playerTag in initialPlayerOrder())
        {
            GameObject playerObject = Instantiate(baseObject, transform.position, transform.rotation);
            playerObject.AddComponent<Player>();
            playerObject.tag = playerTag;

            Player player = playerObject.GetComponent<Player>();
            player.setTag(playerTag);
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
}
