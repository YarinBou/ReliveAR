using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScenes : MonoBehaviour
{

    public Text alertText;
    public GameObject alert;

    IEnumerator checkInternetConnection(System.Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
            Debug.Log("internet not connected");
        }
        else
        {
            action(true);
            Debug.Log("internet connected");
        }
    }

    void invoke ()
    {
       alertText.text = "check your internet connction";
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(checkInternetConnection((isConnected) => {
            if(isConnected)
            {
                Debug.Log("scene 1");
                SceneManager.LoadScene("Player");
            } else
            {
                Invoke("invoke", 3);
            }
        }));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
