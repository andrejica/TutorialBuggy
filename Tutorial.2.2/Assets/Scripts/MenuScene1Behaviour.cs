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
    public GameObject buggy;
    
    public Material bfhSkin;
    public Material starndardSkin;
    
    private Prefs _prefs;
    
    // Start is called before the first frame update
    void Start()
    {
        _prefs = new Prefs();
        _prefs.Load();
        
        //Set Material for buggy before SetAll()
        _prefs.bfhSkin = bfhSkin;
        _prefs.starndardSkin = starndardSkin;
        
        _prefs.SetAll(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR, ref buggy);
    }
    
    public void OnBtnBackToStartMenuClick()
    {
        buggy.GetComponent<CarBehaviour>().StopFmodEngineSound();
        _prefs.Save();
        SceneManager.LoadScene("SceneMenu");
    }
    
    void OnApplicationQuit() { _prefs.Save(); }
}
