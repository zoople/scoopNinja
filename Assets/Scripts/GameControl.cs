using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject iceCreamScoop;


    private float[] conePos = { -2.1f, 0, 2.1f };
    private float[] levels = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };
    private int[,] coneLayoutData = new int[3,5];
    private int[] scoopStackSize = new int[3];
    private Stack<GameObject>[] coneLayout = new Stack<GameObject>[3]; 
    

    // private IceCream[] ConeA;
    // Use this for initialization
    void Start () {

        parseOrder(GetNextOrder());

        coneLayout[0] = new Stack<GameObject>();
        coneLayout[1] = new Stack<GameObject>();
        coneLayout[2] = new Stack<GameObject>();


        // Debug.Log("Ready to show");
        for (int c = 0; c<3; c++)
        {
            //Debug.Log("Cone " + c.ToString());
            for (int s = 0; s< scoopStackSize[c]; s++)
            {
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[s],-s), Quaternion.identity) as GameObject;  //create sorting layers (no need to because can just set Z when move)
                scoop.GetComponent<IceCream>().setFlavour("F"+coneLayoutData[c,s]);
                coneLayout[s].Push(scoop);
            }
         

        }
       
      
    

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Test2");
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse over");
    }
    void OnMouseDown ()
    {
        Debug.Log("Test");
    }

	// Update is called once per frame
	void Update () {
	
	}

    void parseOrder(string order)
    {
       // Debug.Log("Parsing Order: " + order);
        string[] coneDetails;
        string[][] scoopDetails = new string[3][];


        coneDetails = order.Split(';');
        for (int i = 0; i < 3; i++)
        {
            scoopDetails[i] = coneDetails[i].Split(',');  
        }

        for (int c =0; c<coneDetails.Length; c++)
        {
           //  Debug.Log("Cone " + c.ToString() + " has " + scoopDetails[c].Length);
            scoopStackSize[c] = scoopDetails[c].Length;
            for (int s = 0; s < scoopDetails[c].Length; s++)
            {
               // Debug.Log(scoopDetails[c][s]);
                coneLayoutData[c,s] = int.Parse(scoopDetails[c][s]);
            }
            
        }


    }


    string GetNextOrder()
    {
       // Debug.Log("Getting next order");
        return "1,1,2;3,2;1,2,4";  // deal with empty
    }
}
