using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowerHandler : MonoBehaviour {
	
	public List<GameObject> Flowers = new List<GameObject>();
	public bool flower1 = false;
	public bool flower2 = false;
	public bool flower3 = false;

	public GameObject Door;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (flower1 == true && flower2 == true && flower3 == true) 
		{
			Door.gameObject.SetActive (false);
		} 
		else 
		{
			Door.gameObject.SetActive (true);
		}
	}

	void flowerOne(bool val)
	{
		flower1 = val;
	}

	void flowerTwo(bool val)
	{
		flower2 = val;
	}

	void flowerThree(bool val)
	{
		flower3 = val;
	}
}
