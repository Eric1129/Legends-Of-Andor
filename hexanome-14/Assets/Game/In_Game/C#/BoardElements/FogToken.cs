using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogToken
{
	private Node location;
    public string type;
	public bool uncovered;
    public bool used;
	protected GameObject prefab;

	public FogToken(Node startingPos, GameObject prefab, string fog_type)
	{
		location = startingPos;
        type = fog_type;
		this.prefab = prefab;
		used = false;
        uncovered = false;
	}

	private FogToken() { }

	public void useFogToken()
	{
        //this.prefab =
        this.uncovered = true;
		this.used = true;
		//this.prefab.SetActive(false);
		//this.prefab.GetComponent<Material>().color = Color.gray;
		//this.prefab.GetComponent<Renderer>().enabled = false ;
	}

	public void uncoverFogToken()
	{
		this.uncovered = true;
	}

	public int getLocation()
	{
		return location.getIndex();
	}

	public void setLocationNode(Node x)
	{
		location = x;
	}

	public Node getLocationNode()
	{
		return location;
	}

	public GameObject getPrefab()
	{
		return prefab;
	}

    public void setPrefab(GameObject go)
    {
        this.prefab = go;
    }

    public string getType()
    {
        return type;
    }
}
