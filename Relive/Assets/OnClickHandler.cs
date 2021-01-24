using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeText(string newText)
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        txt.text = newText;
    }
}

