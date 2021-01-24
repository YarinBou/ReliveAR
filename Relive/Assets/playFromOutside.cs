using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playFromOutside : MonoBehaviour
{
    //public GameObject vimeo;
    public string url;
    // Start is called before the first frame update
    void Start()
    {
        GameObject vim = GameObject.Find("[VimeoPlayer]");
        if (vim == null)
            print("null");
        else
        {
            print("name:" + vim.name);
           // vim.GetComponent<playVeimeoByUrl>().printit(url);
            //vim.GetComponent<Vimeo.Player.VimeoPlayer>().PlayVideo(url);
            // vim.GetComponent<playVeimeoByUrl>().play(url);
        }
        //vimeo.GetComponent<Vimeo.Player.VimeoPlayer>().PlayVideo(url);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject vim = GameObject.Find("[VimeoPlayer]");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            vim.GetComponent<playVeimeoByUrl>().printit();
        }
    }

    public void playVideo(string URL)
    {

    }
}
