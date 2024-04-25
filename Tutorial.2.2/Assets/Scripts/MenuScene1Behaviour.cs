using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene1Behaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderRL;
    public WheelCollider wheelColliderRR;
    private Prefs _prefs;
    
    // Start is called before the first frame update
    void Start()
    {
        _prefs = new Prefs();
        _prefs.Load();
        _prefs.SetAll(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
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
