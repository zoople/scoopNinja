using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public GameObject iceCreamScoop;
    public GameObject iceCreamCone;
    public GameObject limitLine;
    public bool isGameOver;

    public GameObject winCelebration;

    public static GameControl instance;
    public GameObject selectedScoop;

    //Load settings
    public float[] conePos = { -2.1f, 0, 2.1f, -3.61f };
    // public float[] levels = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };
    // public float[] zlayer = { -1f, -2f, -3f, -4f, -5f };
    public float[] levels;
    public float[] zlayer;

    public float[] conePos_orders = { -1.33f, 0, 1.33f, 2.65f};
    public float[] levels_orders = { 3.69f, 3.99f, 4.29f, 4.59f, 4.89f, 5.39f };
    private float orderScale_cone = 0.65f;
    private float orderScale_scoop = 0.65f;

    public int maxHeight;
    private float[] maxHeightY = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };


    private Stack<int>[] coneLayoutData = new Stack<int>[3];
    public Stack<GameObject>[] coneLayout = new Stack<GameObject>[3];

    private Stack<int>[] coneLayoutData_order = new Stack < int >[3];
    private Stack<GameObject>[] coneLayout_order = new Stack<GameObject>[3];


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        for (int i = 0; i < 5; i++)
        {
            zlayer[i] = -i-1;
            levels[i] = -2.0f + i * 0.5f;
        }
        maxHeight = 4;
        isGameOver = false;
        winCelebration.SetActive(false);
    }

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

        //Load the Ice Creams
        parseOrder(GetNextOrder(), coneLayoutData);       
        LoadIceCreams(conePos, levels, coneLayout, coneLayoutData, 1, 1);

        //Load the order
        parseOrder(GetNextOrder(), coneLayoutData_order);      
        LoadIceCreams(conePos_orders, levels_orders, coneLayout_order, coneLayoutData_order, orderScale_scoop, orderScale_cone);

        //Set the limitline
        limitLine.transform.position = new Vector2(0, maxHeightY[maxHeight]);

        shuffleOrder();

        showIceCreams();

    }
    
    void shuffleOrder()
    {
        // layout[2].Push(layout[0].Pop());
        // moveCone(0, 1, true);

        moveOrder(0, 1);
        moveOrder(0, 2);
        moveOrder(1, 0);
       moveOrder(2, 1);
       moveOrder(2, 0);
       moveOrder(1, 2);



    }

    private bool checkOrder()
    {
        Debug.Log("Checking the order");
        //Not even going to look if there arent at least the same size on all cones
        for (int c = 0; c < 3; c++) if (coneLayout[c].Count != coneLayout_order[c].Count) return false;

        for (int c = 0; c < 3; c++)
        {
            string orderString = "";
            string currentLayoutString = "";

            foreach (GameObject orderScoop in coneLayout_order[c]) orderString += orderScoop.GetComponent<IceCream>().flavour;
            foreach (GameObject scoop in coneLayout[c]) currentLayoutString += scoop.GetComponent<IceCream>().flavour;
             Debug.Log("Order: " + orderString);
            Debug.Log("Current: " + currentLayoutString);

            if (orderString != currentLayoutString) return false;
        }

        
        return true;
    }

    public void moveOrder(int from, int to)
    {
        int scoopNumber = GameControl.instance.coneLayout_order[to].Count;

        coneLayout_order[from].Peek().transform.position = new Vector3(GameControl.instance.conePos_orders[to], GameControl.instance.levels_orders[scoopNumber], GameControl.instance.zlayer[scoopNumber]);

        coneLayout_order[to].Push(coneLayout_order[from].Pop());



    }



    public void moveCone(int from, int to, bool willDoPhysicalMove)
    {
        GameObject temp;

  
        if (coneLayout[to].Count > 0)
        {
            coneLayout[to].Peek().GetComponent<IceCream>().isTop = false;
           // coneLayout[to].Push(temp);
           // Destroy(temp);
        }
        coneLayout[to].Push(coneLayout[from].Pop());

        if(coneLayout[from].Count > 0)
        coneLayout[from].Peek().GetComponent<IceCream>().isTop = true;

        if (checkOrder())
        {
            winCelebration.SetActive(true);

            winCelebration.GetComponent<SpriteRenderer>().enabled = true;
            
            isGameOver = true;

        }


    }

    void showIceCreams()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject scoop in coneLayout[i])
            {
                scoop.GetComponent<SpriteRenderer>().enabled = true;
            }
        }


        for (int i = 0; i < 3; i++)
        {
            foreach (GameObject scoop in coneLayout_order[i])
            {
                scoop.GetComponent<SpriteRenderer>().enabled = true;
            }
        }

    }



    void LoadIceCreams(float[] conePos, float[] levels, Stack<GameObject>[] layout, Stack<int>[] layoutData, float scoopScale, float coneScale)
    {
     //need to have the cones z strictly based on level not how many from the top   

        for (int c = 0; c < 3; c++)
        {
            GameObject cone = Instantiate(iceCreamCone, new Vector3(conePos[c], conePos[3], 5), Quaternion.identity) as GameObject;
            cone.transform.localScale = new Vector2(cone.transform.localScale.x * coneScale, cone.transform.localScale.y * coneScale);
            cone.GetComponent<ConeSelect>().coneNumber = c;
            //Debug.Log(temp);

           // Debug.Log("Cone " + c.ToString() + " has " + layoutData[c].Count);
            
            foreach (int flavour in layoutData[c])
            {
                
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[layout[c].Count], zlayer[layout[c].Count]), Quaternion.identity) as GameObject;
                scoop.GetComponent<IceCream>().setFlavour("F" + flavour);
                scoop.transform.localScale = new Vector2(scoop.transform.localScale.x*scoopScale, scoop.transform.localScale.y * scoopScale);
                scoop.GetComponent<IceCream>().coneLocation = c;

                layout[c].Push(scoop);
                //For the top scoop
                if (layout[c].Count ==  layoutData[c].Count && coneScale == 1)
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

    void parseOrder(string order, Stack<int>[] layout)
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
           //Debug.Log("Cone " + c.ToString() + " has " + scoopDetails[c].Length);
            for (int s = 0; s < scoopDetails[c].Length; s++)
            {
                //Debug.Log(scoopDetails[c][s]);
                layout[c].Push(int.Parse(scoopDetails[c][s]));
            }
            
        }


    }


    string GetNextOrder()
    {
       // Debug.Log("Getting next order");
        return "5,3,4;3,2;4";  // deal with empty
    }
}
