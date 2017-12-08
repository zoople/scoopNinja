using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject iceCreamScoop;
    public GameObject iceCreamCone;


    //Display settings
    private float[] conePos = { -2.1f, 0, 2.1f, -3.61f };
    private float[] levels = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };

    private float[] conePos_orders = { -1.33f, 0, 1.33f, 2.65f};
    private float[] levels_orders = { 3.69f, 3.99f, 4.29f, 4.59f, 4.89f, 5.39f };
    private float orderScale_cone = 0.65f;
    private float orderScale_scoop = 0.65f;
   


    private Stack<int>[] coneLayoutData = new Stack<int>[3];
    private Stack<GameObject>[] coneLayout = new Stack<GameObject>[3];

    private Stack<int>[] coneLayoutData_order = new Stack < int >[3];
    private Stack<GameObject>[] coneLayout_order = new Stack<GameObject>[3];


    // private IceCream[] ConeA;
    // Use this for initialization
    void Start () {

        //Initialise the staks
        for (int i=0; i<3; i++)
        {
            coneLayout[i] = new Stack<GameObject>();
            coneLayout_order[i] = new Stack<GameObject>();
            coneLayoutData[i] = new Stack<int>();
            coneLayoutData_order[i] = new Stack<int>();
        }

        parseOrder(GetNextOrder(), coneLayoutData);


        //Load the Ice Creams
        DisplayIceCreams(conePos, levels, coneLayout, coneLayoutData, 1, 1);

        parseOrder(GetNextOrder(), coneLayoutData_order);
        shuffleOrder(coneLayoutData_order);

        //Displayt the order
        DisplayIceCreams(conePos_orders, levels_orders, coneLayout_order, coneLayoutData_order, orderScale_scoop, orderScale_cone);

        

    }
    
    void shuffleOrder(Stack<int>[] layout)
    {
       layout[2].Push(layout[0].Pop());
    }

    void DisplayIceCreams(float[] conePos, float[] levels, Stack<GameObject>[] layout, Stack<int>[] layoutData, float scoopScale, float coneScale)
    {
        

        for (int c = 0; c < 3; c++)
        {
            GameObject cone = Instantiate(iceCreamCone, new Vector3(conePos[c], conePos[3], 5), Quaternion.identity) as GameObject;
            cone.transform.localScale = new Vector2(cone.transform.localScale.x * coneScale, cone.transform.localScale.y * coneScale);


            Debug.Log("Cone " + c.ToString() + " has " + layoutData[c].Count);
            int s = 0;
            foreach (int flavour in layoutData[c])
            {
                Debug.Log(flavour);
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[layoutData[c].Count-s-1], s), Quaternion.identity) as GameObject;
                scoop.GetComponent<IceCream>().setFlavour("F" + flavour);
                scoop.transform.localScale = new Vector2(scoop.transform.localScale.x*scoopScale, scoop.transform.localScale.y * scoopScale);
                layout[c].Push(scoop);

                //For the top scoop
                if (s == layout[c].Count - 1)
                {
                    scoop.GetComponent<IceCream>().isTop = true;
                }
                //Otherwise
                else
                {
                    scoop.GetComponent<IceCream>().isTop = false;
                }
                s++;
            }


        }
    }
    


	// Update is called once per frame
	void Update () {
	
	}

    void parseOrder(string order, Stack<int>[] layout)
    {
        Debug.Log("Parsing Order: " + order);
        string[] coneDetails;
        string[][] scoopDetails = new string[3][];


        coneDetails = order.Split(';');
        for (int i = 0; i < 3; i++)
        {
            scoopDetails[i] = coneDetails[i].Split(',');  
        }


        for (int c =0; c<coneDetails.Length; c++)
        {
           Debug.Log("Cone " + c.ToString() + " has " + scoopDetails[c].Length);
            for (int s = 0; s < scoopDetails[c].Length; s++)
            {
                Debug.Log(scoopDetails[c][s]);
                layout[c].Push(int.Parse(scoopDetails[c][s]));
            }
            
        }


    }


    string GetNextOrder()
    {
       // Debug.Log("Getting next order");
        return "3,1,4;3,2;1,3,4";  // deal with empty
    }
}
