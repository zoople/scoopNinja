using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderComplete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("I did hear it");
        if (GameControl.instance.isOrderComplete && 
            !GameControl.instance.isGameOver &&
            !GameControl.instance.isLevelComplete)
        {
            Debug.Log("Next Order Please");


            GameControl.instance.isOrderComplete = false;
            GameControl.instance.nextOrder();
            

        }
    }
}
