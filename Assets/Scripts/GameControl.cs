using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    //Singleton
    public static GameControl instance;

    //Linking
    public GameObject iceCreamScoop;
    public GameObject iceCreamCone;
    public GameObject limitLine;
    public GameObject timeBar;
    public GameObject timeBarHolder;
    public GameObject orderboard;
    public GameObject displayText;
    public GameObject levelCompleteCelebration;
    public GameObject orderCompleteCelebration;
    public GameObject gameOverAlert;

    //For levels with time
    private float amtTimeLeft;
    private float maxtime;
    private float timerGraphicFullScale;
    bool isTimerActive;

    //For levels with move limit
    private int maxMoves;
    private int numMovesLeft;
    bool isMoveLimitActive;

    private Stack<GameObject> coneCollection = new Stack<GameObject>();

    //To help control the level
    public bool isGameOver;
    public bool isOrderComplete;
    public bool isLevelComplete;
    private int numOrdersComplete;

    //Load settings
    public float[] conePos = { -1f, 0, 1f, -3.61f };
    // public float[] levels = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f };
    // public float[] zlayer = { -1f, -2f, -3f, -4f, -5f };
    public float[] levels;
    public float[] zlayer;

    //Order display parameters
    private float[] conePos_orders = { -1.25f, 0, 1.25f, 2.3f };  //The cone X positions. [3] is the Y position
    private static float level_order_delta = 0.30f;
    private static float level_order_base = 3.35f;
    private float[] levels_orders = { level_order_base,
                                      level_order_base + level_order_delta,
                                      level_order_base + level_order_delta*2,
                                      level_order_base + level_order_delta*3,
                                      level_order_base + level_order_delta*4,
                                      level_order_base + level_order_delta*5,
    };
    private float orderScale_cone = 0.65f;
    private float orderScale_scoop = 0.65f;

    //Display paramaters and control for the limit line
    public int maxHeight;
    private float[] maxHeightY = { -2.0f, -1.5f, -1.0f, -0.5f, 0.0f, 0.5f, 1f };


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
            levels[i] = -2.5f + i * 0.5f;
        }
        maxHeight = 4;

        isGameOver = false;
        isLevelComplete = false;
        isOrderComplete = false;
        levelCompleteCelebration.SetActive(false);
        numOrdersComplete = 0;
    }


    public void nextOrder()
    {
        orderCompleteCelebration.SetActive(false);

        Debug.Log("Loading the next order");

        foreach (GameObject test in coneCollection)
        {
            Destroy(test);
        }

        for (int i = 0; i< 3; i++)
        {
            coneLayoutData[i].Clear();
            coneLayoutData_order[i].Clear();
            Debug.Log("Cleared the data from cone "+i);

            if (coneLayout[i].Count > 0)
            {
                foreach (GameObject test in coneLayout[i])
                {
                    Destroy(test);
                }
                coneLayout[i].Clear();
            }

      
            if (coneLayout_order[i].Count > 0)
            {
                foreach (GameObject test in coneLayout_order[i])
                {
                    Destroy(test);
                }
                coneLayout_order[i].Clear();
            }
            


            Debug.Log("Cleared the objects from cone " + i);

            maxHeight = 0;
            
        }

        //levelCompleteCelebration.SetActive(false);
        //levelCompleteCelebration.GetComponent<SpriteRenderer>().enabled = false;
        //isGameOver = false;
        isOrderComplete = false;
    
        setupPuzzle();

    }
  

    void Start () {

        timerGraphicFullScale = timeBar.transform.localScale.x;

        //Initialise the staks
        for (int i=0; i<3; i++)
        {
            coneLayout[i] = new Stack<GameObject>();
            coneLayout_order[i] = new Stack<GameObject>();
            coneLayoutData[i] = new Stack<int>();
            coneLayoutData_order[i] = new Stack<int>();
            conePos[i] = -1.9f + 1.9f * i;
        }

    

        setupPuzzle();

    }
    
    void setupPuzzle()
    {
        string order;

        displayText.GetComponent<Text>().text = numOrdersComplete.ToString();
   
            
        order = GetNextOrder();
        //Load the Ice Creams
        parseOrder(order, coneLayoutData);
        LoadIceCreams(conePos, levels, coneLayout, coneLayoutData, 1, 1);

        //Load the order
        parseOrder(order, coneLayoutData_order);
        LoadIceCreams(conePos_orders, levels_orders, coneLayout_order, coneLayoutData_order, orderScale_scoop, orderScale_cone);



        while (checkOrder()) shuffleOrder();

        //Set the limitline
        limitLine.transform.position = new Vector2(0, maxHeightY[maxHeight + 1] - 0.5f);

        showIceCreams();

        if (numOrdersComplete == 3)        orderboard.transform.RotateAround(new Vector3(orderboard.transform.position.x, orderboard.transform.position.y, orderboard.transform.position.z), new Vector3(0, 0, 1), 90);

        // Rotate(0, 0, 90);
        isTimerActive = false;
        isMoveLimitActive = false;

        if (numOrdersComplete == 2)
        {
            isTimerActive = true;
            maxtime = 20;
            amtTimeLeft = maxtime;
        } 
    }

    void shuffleOrder()
    {
        // layout[2].Push(layout[0].Pop());
        // moveCone(0, 1, true);
  
        int hardMax = -1;
        int currentMax = 0;

        for (int i=0; i<3; i++) if (coneLayout[i].Count>currentMax) currentMax = coneLayout[i].Count;
       // Debug.Log("The maximum height is " + currentMax);
       hardMax = currentMax + Random.Range(0, 2);
        
        int lastFrom = -1;
        int lastTo = -2;
        int from = -1;
        int to = -2;

        int numMoves = 1;
           
        bool validMove = false;
        int attempts = 0;
        string reason;

        for (int m = 0; m <numMoves; m++)
        {
            validMove = false;
            attempts = 0;

            while (!validMove)
            {
                from = Random.Range(0, 3);
                to = Random.Range(0, 3);
           
                validMove = true;
                reason = " fuck knows! ";
                if (from == to) { validMove = false; reason = " from=to"; } //cant move to the same place
                if (coneLayout_order[from].Count == 0) { validMove = false; reason = " cone empty"; }//cant move if there is nothing there
                if (coneLayout_order[to].Count >= hardMax) { validMove = false; reason = " cone full"; } //cant move if it is going to make it go over the max
                if (from == lastTo) { validMove = false; reason = " that was the last one"; }  //you cant pick a scoop that you put down last time
                attempts++;
                if (attempts > 200) validMove = true;
                Debug.Log("attempt "+from + "," + to+" "+validMove+reason);
            }
            if (attempts < 100)
            {
                moveOrder(from, to);
                lastFrom = from;
                lastTo = to;
                Debug.Log("***************************** " + from + "," + to);
                // Debug.Log("Cone " + to + " is now " + coneLayout_order[to].Count);
                if (coneLayout_order[to].Count > currentMax) currentMax = coneLayout_order[to].Count;
            }
            else Debug.Log("........................failed move");

        }
        
       //Debug.Log("The maximum height was " + currentMax); //moveOrder(0, 1);
                              // moveOrder(0, 2);
                              //moveOrder(1, 0);
                              // moveOrder(2, 1);
                              //   moveOrder(2, 0);
                              //moveOrder(1, 2);
        maxHeight = currentMax;



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
            //levelCompleteCelebration.SetActive(true);
            //levelCompleteCelebration.GetComponent<SpriteRenderer>().enabled = true;

            //isGameOver = true;
            isOrderComplete = true;

            numOrdersComplete++;

            if (numOrdersComplete < 5)
            {
                orderCompleteCelebration.SetActive(true);
            }
            else
            {
                isLevelComplete = true;

                levelCompleteCelebration.SetActive(true);
            }
            
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
            if (scoopScale !=1) cone.transform.parent = orderboard.transform;
            //Debug.Log(temp);
            coneCollection.Push(cone);

           // Debug.Log("Cone " + c.ToString() + " has " + layoutData[c].Count);
            
            foreach (int flavour in layoutData[c])
            {
                
                GameObject scoop = Instantiate(iceCreamScoop, new Vector3(conePos[c], levels[layout[c].Count], zlayer[layout[c].Count]), Quaternion.identity) as GameObject;
                scoop.GetComponent<IceCream>().setFlavour("F" + flavour);
                scoop.transform.localScale = new Vector2(scoop.transform.localScale.x*scoopScale, scoop.transform.localScale.y * scoopScale);
                scoop.GetComponent<IceCream>().coneLocation = c;
                if (scoopScale != 1) scoop.transform.parent = orderboard.transform;
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

        if (isTimerActive)
        {
            amtTimeLeft -= Time.deltaTime;
            timeBar.transform.localScale = new Vector2(timerGraphicFullScale*(amtTimeLeft/maxtime), timeBar.transform.localScale.y);
            
            if (amtTimeLeft <=0)
            {
                gameOverAlert.SetActive(true);
                isGameOver = true;
                isTimerActive = false;
            }
        }
      
        
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

        string[] orders =
         new string[]
         {
            "5,3,4,2;3,2;4",
            "1,3;4,5,2;4",
            "4,5;3,2;4,5",

         };


        int order = Random.Range(0, 3);
        return orders[order]; 


    }
}
