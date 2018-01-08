using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (GameControl.instance.isGameOver)
        {
            GameControl.instance.isGameOver = true;
            //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           // GameControl.instance.nextOrder();

        }
    }
}
