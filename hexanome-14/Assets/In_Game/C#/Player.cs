using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;


namespace Andor
{
public class Player : MonoBehaviourPun, IPunObservable
{

    [HideInInspector]
    public GameObject sphere;

    private Vector3 realPosition;

    private string userName;

    // ie Player-Male-Dwarf
    // also encodes the corresponding hero type
    // and Sphere-Male-Dwarf. sphere object is attached to this script.
    private string myTag;
    private string heroType;

    private Hero myHero;
    // Will need to use this to verify things like: 
    // showTradeRequest() { if player.lookingAt != Battle then showTradeRequest() }
    // private Screen lookingAt;

    public string NickName { get; internal set; }

    void Start()
    {
        if (photonView.IsMine)
        {
            sphere = PhotonNetwork.Instantiate("sphere", transform.position, transform.rotation, 0);
        // sphere = (GameObject) Resources.Load("sphere");
        // gameObject.AddComponent<Sphere>();
        // sphere = GetComponent<Sphere>();
            sphere.SetActive(true);
            sphere.transform.localScale = new Vector3(2.2f, 2.2f, 0.12f);
        }
        if (!photonView.IsMine && GetComponent<PlayerController>() != null)
            Destroy(GetComponent<PlayerController>());

    }

    public void setTag(string ID)
    {
        myTag = ID;
        gameObject.tag = ID;
        setHeroType();
    }

    public void setHero(Hero hero)
    {
        myHero = hero;
    }

    public string getPlayerTag()
    {
        return myTag;
    }

    public string getHeroType()
    {
        return heroType;
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "UnityMadeMeSaveToFile")
            return;

        if (PhotonNetwork.IsMasterClient){ setTag("Player-Male-Wizard"); }
        else{ setTag("Player-Male-Dwarf"); }
    }

    private void checkClick()
    {

        if (realPosition != transform.position)
            transform.position = realPosition;
        if (Input.GetMouseButtonDown(0))
        {
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

             // RaycastHit hitInfo;
             RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);
            Debug.Log("clicked" );
             if (hitInfo == null)
             {
                 return;
             }

            string colliderTag = hitInfo.collider.gameObject.tag;
            Debug.Log("want to move to: " + colliderTag);
            if (colliderTag == null)
            {
                return;
            }
            int cTag;
            try
            {
                cTag = int.Parse(colliderTag);
                GameObject go = GameObject.FindWithTag(colliderTag);
                BoardPosition bp = go.GetComponent<BoardPosition>();
                // player.gameObject.transform.position = bp.getMiddle();
                sphere.transform.position = bp.getMiddle();
                // newPos = player.sphere.transform.position;
                // transform.position = new Vector3(-6.81f, 7.57f, 0.0f);
            }
            catch (InvalidCastException e)
            {
            }
        }
    }

    // extracts the hero tag from our own tag
    // EX.)   if (myTag == "Player-Male-Dwarf") -> output "Male-Dwarf"
    private void setHeroType()
    {
        // must initialize before adding strings
        heroType = "";
        int ct = 0;
        string[] partsOfTag = myTag.Split('-');
        if (partsOfTag.Length != 3)
        {
            Debug.Log("Found a bad tag in Player.setHeroType: " + myTag);
        }

        heroType = partsOfTag[1] + "-" + partsOfTag[2];
        //foreach(string partOfTag in myTag.Split('-'))
        //{
        //    // skip first val: "Player"
        //    if (ct++ == 0) continue;

        //    heroType += partOfTag;
        //}
    }

    // public void moveSlowly(Vector3 newPos)
    // {

    // }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sphere.transform.position);
        }
        else
        {
            sphere.transform.position = (Vector3) stream.ReceiveNext();
        }
    }

    public static void RefreshInstance(ref Player player, string prefab)
    {
        var position = Vector3.zero;
        var rotation = Quaternion.identity;
        if (player != null)
        {
            position = player.transform.position;
            rotation = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }

        player = PhotonNetwork.Instantiate(prefab, position, rotation).GetComponent<Player>();
        // sphere = player;
    }

}
}
