using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneCapture : MonoBehaviour
{
    // Boolean flags shows if the microphone is connected 
    private bool micConnected = false;

    //The maximum and minimum available recording frequencies  
    private int minFreq;
    private int maxFreq;

    //A handle to the attached AudioSource  
    private AudioSource goAudioSource;

    void Start()
    {
        //Check if there is at least one microphone connected  
        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present  
        {
            //Set our flag 'micConnected' to true  
            micConnected = true;
            Debug.Log("connected");
            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
            Debug.Log(minFreq);
            Debug.Log(maxFreq);
            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }

            //Get the attached AudioSource component  
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }
    
    void OnGUI()
    {
        //If there is a microphone  
        if (micConnected)
        {
            Debug.Log("1");
            //If the audio from any microphone isn't being captured  
            if (!Microphone.IsRecording(null))
            {
                Debug.Log("2");
                //Case the 'Record' button gets pressed  
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Record"))
                {
                    Debug.Log("3");
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                    goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                }
            }
            else //Recording is in progress  
            {
                Debug.Log("4");
                //Case the 'Stop and Play' button gets pressed  
                if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Stop and Play!"))
                {
                    Debug.Log("5");
                    Microphone.End(null); //Stop the audio recording  
                    goAudioSource.Play(); //Playback the recorded audio  
                }
                Debug.Log("6");
                GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 25, 200, 50), "Recording in progress...");
            }
        }
        else // No microphone  
        {
            Debug.Log("7");
            //Print a red "Microphone not connected!" message at the center of the screen  
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Microphone not connected!");
        }

    }
}