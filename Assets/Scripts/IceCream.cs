using UnityEngine;
using System.Collections;

public class IceCream : MonoBehaviour {

    private  SpriteRenderer flavourSprite;
    public Sprite F1;
    public Sprite F2;
    public Sprite F3;
    public Sprite F4;
    public Sprite F5;

    public string flavour;
    public bool isTop;

    public int coneLocation;
    private int newConeLocation;

    private Vector3 screenPoint;
    private Vector3 offset;
    private float distanceTravelled;



    // Use this for initialization
    void Awake () {

        flavourSprite = GetComponent<SpriteRenderer>();
        flavour = "F1";
        flavourSprite.sprite = F1;
        isTop = false;
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseDown()
    {


        if (isTop)
        {
           // Debug.Log("Ice Cream: " + gameObject.transform.position.y);
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        }
       // else Debug.Log("You cant click this one: " + gameObject.transform.position.y);

    }

    void OnMouseDrag()
    {
        float buffer = 0.5f;
        if (isTop)
        { 

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

             // Debug.Log("Mouse: "+Camera.main.ScreenToWorldPoint(curScreenPoint).y);
            //  Debug.Log("LImit: " +GameControl.instance.limitLine.transform.position.y);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            if (Camera.main.ScreenToWorldPoint(curScreenPoint).y > (GameControl.instance.limitLine.transform.position.y - buffer)) 

            curPosition.y = GameControl.instance.limitLine.transform.position.y - buffer;


            curPosition.z = -6f; //bring it in front while dragging



        transform.position = curPosition;
        
        //distanceTravelled = Vector3.Distance(curScreenPoint, screenPoint);
        }
    }

    private void OnMouseUp()
    {
        if (isTop)
        {
            if (newConeLocation == coneLocation) // if you tried to move it somewhere full
            {
                //Pysiclly move the cone
                int scoopNumber = GameControl.instance.coneLayout[newConeLocation].Count;
                if (coneLocation == newConeLocation) scoopNumber--;
                transform.position = new Vector3(GameControl.instance.conePos[newConeLocation], GameControl.instance.levels[scoopNumber], GameControl.instance.zlayer[scoopNumber]);

            }
            else
            {


                //Pysiclly move the cone
                int scoopNumber = GameControl.instance.coneLayout[newConeLocation].Count;
                if (coneLocation == newConeLocation) scoopNumber--;
                transform.position = new Vector3(GameControl.instance.conePos[newConeLocation], GameControl.instance.levels[scoopNumber], GameControl.instance.zlayer[scoopNumber]);

                //Logiclly move the cone
                GameControl.instance.moveCone(coneLocation, newConeLocation, false);
                coneLocation = newConeLocation; //update my record of it
            }

        }
    }

    public void setFlavour(string newFlavour)
    {
     
        flavour = newFlavour;

        switch(flavour)
        {
            case "F1": flavourSprite.sprite = F1; break;
            case "F2": flavourSprite.sprite = F2; break;
            case "F3": flavourSprite.sprite = F3; break;
            case "F4": flavourSprite.sprite = F4; break;
            case "F5": flavourSprite.sprite = F5; break;


            default: flavourSprite.sprite = F1; break;

        }
     
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //snapToX = col.transform.position.x;
        //snapToY = col.transform.position.y;
       
        newConeLocation = col.GetComponent<ConeSelect>().coneNumber;
        if (GameControl.instance.coneLayout[newConeLocation].Count == GameControl.instance.maxHeight)
            newConeLocation =coneLocation;
        //Debug.Log(newConeLocation);

    }
}
