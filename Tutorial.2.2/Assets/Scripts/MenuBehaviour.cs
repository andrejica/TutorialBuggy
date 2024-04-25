using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderRL;
    public WheelCollider wheelColliderRR;
    public Slider sliSuspDistance;
    public TMP_Text txtDistanceNum;
    private Prefs _prefs;
    
    // Start is called before the first frame update
    void Start()
    {
        _prefs = new Prefs();
        _prefs.Load();
        _prefs.SetAll(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
        sliSuspDistance.value = _prefs.suspensionDistance;
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
    }
    
    public void OnSliderChangedSuspDistance(float suspDistance)
    {
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
        _prefs.suspensionDistance = sliSuspDistance.value;
        _prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }
    
    public void OnBtnStartClick()
    {
        _prefs.Save();
        SceneManager.LoadScene("Scene1");
    }
    
    void OnApplicationQuit() { _prefs.Save(); }
}
