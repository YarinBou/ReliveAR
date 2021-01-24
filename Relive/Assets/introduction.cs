using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class introduction : MonoBehaviour
{
  
    public Animator step1Anim;
    public GameObject scanText;
    
    

    public bool IsPlaying;
  
    
    void Start()
    {
        
        scanText.SetActive(false);
        STart_Repeat_couroutine();
        
    }



    public void STart_Repeat_couroutine()
    {
        StartCoroutine(repeatStep1());
    }

    IEnumerator repeatStep1()
    {
        yield return new WaitForSeconds(5);
        if(!IsPlaying)
        {
            StartAnim();
            
        }
        else
        {
            StartCoroutine(repeatStep1());
        }
        

    }

   public void StartAnim()
    {
        step1Anim.enabled = true;
        print("Start");
        scanText.SetActive(true);
       
        


    }
    public void StopAnim()
    {
        step1Anim.enabled = false;
        scanText.SetActive(false);
        StartCoroutine(repeatStep1());
        

    }

  

}
