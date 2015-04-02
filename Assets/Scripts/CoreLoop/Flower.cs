using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

	GameObject Door;

	public GameObject FlowerHandler;
	float countDown = 1;
	bool begin;


	void Update () {
		
		if (countDown > 0 && begin == true) {
			
			countDown -= Time.deltaTime;
		} else if (countDown <= 0) {
			begin = false;
			gameObject.renderer.material.color = Color.white;
			FlowerHandler.SendMessage(transform.tag, false);
		}
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		print (col.transform.tag);
		if (col.transform.CompareTag ("Player")) {
			
			gameObject.renderer.material.color = Color.blue;
			FlowerHandler.SendMessage(transform.tag, true);
			countDown = 7;
		}
	}
	
	void OnTriggerExit2D (Collider2D col) {
		
		begin = true;
	}
}
