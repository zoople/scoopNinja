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


    // Use this for initialization
    void Awake () {

        flavourSprite = GetComponent<SpriteRenderer>();
        flavour = "F1";
        flavourSprite.sprite = F1;
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseDown()
    {
        if (isTop)         Debug.Log("Ice Cream: " + gameObject.transform.position.y);
        else Debug.Log("You cant click this one: " + gameObject.transform.position.y);

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
}
