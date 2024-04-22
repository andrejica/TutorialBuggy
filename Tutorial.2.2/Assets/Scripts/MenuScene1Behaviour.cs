using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene1Behaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnBtnBackToStartMenuClick()
    {
        GameObject.Find("Buggy").GetComponent<CarBehaviour>().StopFmodEngineSound();
        SceneManager.LoadScene("SceneMenu");
    }
}
