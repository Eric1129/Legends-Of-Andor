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
    private GameObject sphere;
    [SerializeField]
    private SpriteAtlas heroAtlas;


    private void createCamera(Vector3 cameraPosition)
    {
        baseCamera.transform.position = cameraPosition;
        // camera.SetActive(true);

        GameObject gameParent = Instantiate(baseObject, transform.position, transform.rotation);
        GameObject fullBoard = Instantiate(baseObject, transform.position, transform.rotation);
        GameObject masterClassObject = Instantiate(baseObject, transform.position, transform.rotation);
        GameObject sphereObject = Instantiate(baseObject, transform.position, transform.rotation);
        gameParent.name = "gameParent";
        fullBoard.name = "full-Board";
        masterClassObject.name = "Master";
        sphereObject.name = "sphere-object";
        masterClassObject.tag = "Master";

        // otherwise the sphere will show before the map
        sphereObject.SetActive(false);
        // gameParent is a sibling of the camera, NOT child
        gameParent.transform.parent = gameObject.transform;
        // fullBoard is a child of gameParent
        fullBoard.transform.parent = gameParent.transform;
        // the obj with master class is child of full board
        masterClassObject.transform.parent = fullBoard.transform;

        fullBoard.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRenderer = fullBoard.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fullBoardSprite;
        // whatever is drag n dropped as ^ is shown as board img (sprite)

        masterClassObject.AddComponent<masterClass>();
        masterClassObject.AddComponent<BoardPosition>();
        masterClass master = masterClassObject.GetComponent<masterClass>();


        // GameObject heroAtlasObject = Instantiate(baseObject, transform.position, transform.rotation);
        // heroAtlasObject.SetActive(false);
        // heroAtlasObject.AddComponent<SpriteAtlas>();
        // SpriteAtlas atlas = heroAtlasObject.GetComponent<SpriteAtlas>();
        // atlas = heroAtlas;

        master.initMasterClass("MainCamera", sphereObject, heroAtlas, baseObject);
    }


    void Start()
    {
        createCamera(new Vector3(0.0f, 0.0f, -60.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
