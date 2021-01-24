using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oriantation : MonoBehaviour
{

    public Sprite mask3;
    public Sprite mask2;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            image.sprite = mask2;
        } 

        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            image.sprite = mask3;
        }
       
    }
}
