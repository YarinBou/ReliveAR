using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vuforia;
using UnityEngine.Video;
using UnityEngine.UI;
using Vimeo;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Analytics;
using UnityEngine.Events;


public class SimpleCloudHandler : MonoBehaviour
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    public GameObject GoWebsiteBtn;
    public ImageTargetBehaviour ImageTargetTemplate;
    public introduction instance;
    public GameObject vim; //vimeo gameobject
    public Button restartButton; //the X button
    public GameObject VIdeo_playing_panel;
    public GameObject vimScreen; // imeo screen
    public GameObject IntroButton;
    public Text GotoWebText;

    UnityEvent m_MyEvent;


    public String websiteURL;

    int framePassedFromVimIsPlaying = 0;
    int framenumberToStartShowVinAfterVideoIsPlaying = 2;


    bool vimTrasperent;
    public Material defaltVimMaterial;
    public Material trasperentMaterial;

    float targetWidth = 0;

    string sizeType;
    string firstWord;
    string secondWord;





    public void LinkPressed()
    {
        //Analytics.CustomEvent("press_on_link");
        print("hello");
    }
      
    


    void Start()
    {


        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

     

        //hide vimeo screen
        hideVimScreenWhenVimIsNotReady();

        // register this event handler at the cloud reco behaviour -vuforia stuff
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

    void Ping()
    {
        Debug.Log("Ping");
    }

    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }

   public void GoToIntro()
    {
        SceneManager.LoadScene("intro");
    }

    public void TurnOnFlash()
    {
        
        CameraDevice.Instance.SetFlashTorchMode(true);
        print("hry");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
        }
        

        //if the video starts playing- show the vimeo screen
        if (vim.GetComponent<Vimeo.Player.VimeoPlayer>().IsPlaying())
        {

            if (vimTrasperent == true)
            {

                if (framePassedFromVimIsPlaying == framenumberToStartShowVinAfterVideoIsPlaying)
                {
                    StartCoroutine(WaitOneFrame());
                }
                else
                    framePassedFromVimIsPlaying++;


            }

        }

    }

    //waits for end of frame to show the screen in the exact time
    IEnumerator WaitOneFrame()
    {
        yield return new WaitForEndOfFrame();
        ShowTheScreen();
    }

    //shows vimeo screen
    void ShowTheScreen()
    {

        showVimScreenWhenReady();
        //enabled the X button
        enabledButton(false);

    }

    //enable X button
    void enabledButton(bool enabled)
    {
        restartButton.gameObject.SetActive(enabled);
        VIdeo_playing_panel.SetActive(!enabled);
       
        
    }



    //vuforia function
    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized");

        enabledButton(true);
        GoWebsiteBtn.SetActive(false);


    }

    //vuforia function
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    //vuforia function
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    //vuforia function
    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }


    }

    // Here we handle a cloud target recognition event - vuforia function I have changed
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {

        //vuforia stuff
        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult =
            (TargetFinder.CloudRecoSearchResult)targetSearchResult;

        // keeps the metafata of the target
        mTargetMetadata = cloudRecoSearchResult.MetaData;
       

        // splits the metadata to url and size
        string[] splitArray = mTargetMetadata.Split(char.Parse(" "));
        string url = splitArray[0];
        sizeType = splitArray[1];


        if(splitArray[0] == "url" && splitArray[1] == "url" )
        {
            websiteURL = splitArray[2];
            Application.OpenURL(websiteURL);


            enabledButton(true); // button X dissapears
            mTargetMetadata = null; // deleate the metadate
            vim.GetComponent<Vimeo.Player.VimeoPlayer>().Pause(); // stops playing vimeo video
            mCloudRecoBehaviour.CloudRecoEnabled = true; //vuforia stuff

            hideVimScreenWhenVimIsNotReady(); //hides vimeo screen


            instance.IsPlaying = false;
            instance.StopAnim();
            IntroButton.SetActive(true);
        }
        
        
        if(splitArray.Length>2)
        {
            websiteURL = splitArray[2];
            if (splitArray.Length > 3)
            {
                firstWord = splitArray[3];
                secondWord = splitArray[4];
                GotoWebText.text = $"{firstWord} {secondWord}";
                //changeGoToText(goToText);
            }
            
            
        }
        else
        {
            websiteURL = null;
        }
        


        //sends the size . for update vimeo screen size
        changeSize(sizeType);

        if (websiteURL != null)
        {
            GoWebsiteBtn.SetActive(true);
        }
        else
        {
            GoWebsiteBtn.SetActive(false);
        }


        // stop the target finder (i.e. stop scanning the cloud) -vuforia stuff
        mCloudRecoBehaviour.CloudRecoEnabled = false;


        // Build augmentation based on target - vuforia stuff
        if (ImageTargetTemplate)
        {

            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
        }

        //printing in the debug it starts to play
        print("was told to Play &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");

        //enable the vimeo screen so we can see it
        vim.SetActive(true);
        //tells the vimeo to start to play
        if(splitArray[0] == "url")
        {
            return;
        }
        else
        {
            vim.GetComponent<Vimeo.Player.VimeoPlayer>().LoadVideo(url);
            vim.GetComponent<Vimeo.Player.VimeoPlayer>().PlayVideo(url);
        }
       
        print("here");
        print(websiteURL);

        instance.IsPlaying = true;
        instance.StopAnim();
        IntroButton.SetActive(false);



    }

    public void GotoWebsite()
    {
        if(websiteURL!=null)
        {
            Application.OpenURL(websiteURL);
        }


    }





    //when we press on the X button 
    public void RestartTargetFinder()
    {
        
        enabledButton(true); // button X dissapears
        mTargetMetadata = null; // deleate the metadate
        vim.GetComponent<Vimeo.Player.VimeoPlayer>().Pause(); // stops playing vimeo video
        mCloudRecoBehaviour.CloudRecoEnabled = true; //vuforia stuff

        hideVimScreenWhenVimIsNotReady(); //hides vimeo screen


        instance.IsPlaying = false;
        instance.StopAnim();
        IntroButton.SetActive(true);


    }


    //hides and shows vimeo screen by making the screen matirial to a transperent matirial
    void hideVimScreenWhenVimIsNotReady()
    {
        vimScreen.GetComponent<Renderer>().material = trasperentMaterial;
        vimTrasperent = true;
    }

    void showVimScreenWhenReady()
    {
        vimScreen.GetComponent<Renderer>().material = defaltVimMaterial;
        vimTrasperent = false;
    }


    //changes the size of the vimeoscreen , the size is the letter whicj present the size
    //please change the x and z , the y is not importent
    void changeSize(string size)
    {
        if (size == "A") // 7.5cm *10 cm
        {
            vimScreen.transform.localScale = new Vector3(0.10f, 0.90f, 0.10f);
        }

        if (size == "B")//10cm*10cm
        {
          vimScreen.transform.localScale = new Vector3(0.10f, 0.90f, 0.35f);
        }



      //  if (size == "C") // 15cm *10 cm
       // {
        //    vimScreen.transform.localScale = new Vector3(0.98f, 0.55f, 0.65f);
       // }

       // if (size == "D") // 15cm *20 cm
      //  {
        //    vimScreen.transform.localScale = new Vector3(0.7f, 0.55f, 0.53f);

        //}
    }

 






}

