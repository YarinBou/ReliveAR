using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour

{

    public GameObject noInternetText;
    public int intro;

    private void Awake()
    {

        intro = PlayerPrefs.GetInt("intro");
        
        
        
    }
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
        noInternetText.SetActive(false);
        StartCoroutine(LoadScreen());
        

    }


    IEnumerator LoadScreen()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(CheckInternet());

    }



    IEnumerator CheckInternet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            
            if (intro == 0)
            {
                
                SceneManager.LoadScene("intro");
                PlayerPrefs.SetInt("intro", 1);
               

            }
            else
            {
                
                SceneManager.LoadScene("Player");

            }
            
             
        }
        else
        {
            noInternetText.SetActive(true);
            yield return new WaitForSeconds(1);

            StartCoroutine(CheckInternet());

        }
    }

 

 
}
