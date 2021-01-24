using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoCOntrollerIntro : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject InfoPanel;
    public Text InfoPanelText;
    public GameObject SkipButton;
    public GameObject ContinueBtn;
    public GameObject RecordButton;
    public GameObject StopButton;
    public GameObject ExitButton;
    

    int count = 0;

   
    // Start is called before the first frame update
    void Start()
    {

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        StartCoroutine(first(.5F));
       
        
    }

    private void OnDestroy()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
        }

        
    }


    IEnumerator first( float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pause();
    }

    IEnumerator OnClickNext(float seconds, bool infoPanel, bool skipButton, string infoPanelText, bool Record, bool Exit, bool Stop)
    {
        yield return new WaitForSeconds(seconds);
        pause();
        Vector3 pos = ContinueBtn.transform.localPosition;
        pos.x = 0;
        ContinueBtn.transform.localPosition = pos;
        InfoPanel.SetActive(infoPanel);
        SkipButton.SetActive(skipButton);
        RecordButton.SetActive(Record);
        ExitButton.SetActive(Exit);
        StopButton.SetActive(Stop);
        InfoPanelText.text = infoPanelText;
        
    }


    IEnumerator OnFinishVideo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Player");
    }


    void play()
    {
        VideoPlayer.Play(); 
    }


    void pause()
    {
        VideoPlayer.Pause();
    }

    public void skip()
    {
        SceneManager.LoadScene("Player");
    }

    public void next()
    {

        switch (count)
        {
            case 0:
                {
                    InfoPanel.SetActive(false);
                    play();
                    StartCoroutine(OnClickNext(5.3F, true, false , "להקלטת הסרטון לחצו על כפתור ההקלטה"  , true, false, false));
                    
                    count++;
                    break;
                }

            case 1:
                {
                    InfoPanel.SetActive(false);
                    play();
                    StartCoroutine(OnClickNext(2.1F, true, false, "להפסקת הסרטון לחצו על כפתור היציאה" , false, true, false));
                    //InfoPanelText.rectTransform.sizeDelta = new Vector2(256, 100);
                    RecordButton.SetActive(false);
                    count++;
                    
                    break;
                }

            case 2:
                {
                    InfoPanel.SetActive(false);
                    play();
                    StartCoroutine(OnClickNext(2F, true, false, "להפסקת ההקלטה לחצו על כפתור העצירה", false, false, true));
                    ExitButton.SetActive(false);
                    count++;
                    break;
                }
            case 3:
                {
                    InfoPanel.SetActive(false);
                    StopButton.SetActive(false);
                    play();
                    StartCoroutine(OnFinishVideo(2));
                    
                    break;
                }
        }
        
      
    }


}
