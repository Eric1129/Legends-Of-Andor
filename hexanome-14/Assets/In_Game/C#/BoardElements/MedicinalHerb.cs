using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicinalHerb 
{

	private Node location;
	public bool used;
	protected GameObject prefab;

	public MedicinalHerb(Node startingPos, GameObject prefab)
	{
		location = startingPos;
		this.prefab = prefab;
		used = false;
	}

	private MedicinalHerb() { }

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

}
