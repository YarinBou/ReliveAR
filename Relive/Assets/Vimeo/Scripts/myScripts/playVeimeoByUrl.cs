using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vimeo;


// I made this script only to pull out the functions I need from Vimeo.player.VimeoPlayer.cs
public class playVeimeoByUrl : MonoBehaviour
{
     //video url
    public string url;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            play(url);
        }
    }

    public void printit()
    {
        print("printed in playVimeoByUrl");
        //play(something);
       // play(url);
    }

    public void play(string newurl)
    {
        // GetComponent<Vimeo.Player.VimeoPlayer>().m_file_url;
        GetComponent<Vimeo.Player.VimeoPlayer>().PlayVideo("https://vimeo.com/375870674");
       // GetComponent<Vimeo.Player.VimeoPlayer>().LoadAndPlayVideo();
    }
}
