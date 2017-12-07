using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject iceCreamScoop;
    public GameObject iceCreamCone;



    private float[] conePos = { -2.1f, 0, 2.1f, -3.61f };
    private float[] levels = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };

    private float[] conePos_orders = { -1.33f, 0, 1.33f, 2.65f};
    private float[] levels_orders = { 3.69f, 3.99f, 4.29f, 4.59f, 4.89f, 5.39f };
    private float orderScale_cone = 0.65f;
    private float orderScale_scoop = 0.65f;
   


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

        //Load the Ice Creams
        DisplayIceCreams(conePos, levels, coneLayout, 1, 1);

        //Displayt the order
        DisplayIceCreams(conePos_orders, levels_orders, coneLayout, orderScale_scoop, orderScale_cone);

        /*
        for (int c = 0; c<3; c++)
        {
            GameObject cone = Instantiate(iceCreamCone, new Vector3(conePos[c], conePos[3], 1), Quaternion.identity) as GameObject;

            //Debug.Log("Cone " + c.ToString());
            for (int s = 0; s< scoopStackSize[c]; s++)
            {
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[s],-s), Quaternion.identity) as GameObject; 
                scoop.GetComponent<IceCream>().setFlavour("F"+coneLayoutData[c,s]);
                coneLayout[s].Push(scoop);

                //For the top scoop
                if (s == scoopStackSize[c] - 1)
                {
                    scoop.GetComponent<IceCream>().isTop = true;
                }
                //Otherwise
                else
                {
                    scoop.GetComponent<IceCream>().isTop = false;
                }
            }
         

        } 
        */

    }

    void DisplayIceCreams(float[] conePos, float[] levels, Stack<GameObject>[] layout, float scoopScale, float coneScale)
    {
        for (int c = 0; c < 3; c++)
        {
            GameObject cone = Instantiate(iceCreamCone, new Vector3(conePos[c], conePos[3], 1), Quaternion.identity) as GameObject;
            cone.transform.localScale = new Vector2(cone.transform.localScale.x * coneScale, cone.transform.localScale.y * coneScale);

            //Debug.Log("Cone " + c.ToString());
            for (int s = 0; s < scoopStackSize[c]; s++)
            {
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[s], -s), Quaternion.identity) as GameObject;
                scoop.GetComponent<IceCream>().setFlavour("F" + coneLayoutData[c, s]);
                scoop.transform.localScale = new Vector2(scoop.transform.localScale.x*scoopScale, scoop.transform.localScale.y * scoopScale);
                coneLayout[s].Push(scoop);

                //For the top scoop
                if (s == scoopStackSize[c] - 1)
                {
                    scoop.GetComponent<IceCream>().isTop = true;
                }
                //Otherwise
                else
                {
                    scoop.GetComponent<IceCream>().isTop = false;
                }
            }


        }
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
