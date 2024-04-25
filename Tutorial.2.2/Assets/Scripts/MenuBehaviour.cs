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
    public GameObject buggy;
    public Slider sliSuspDistance;
    public Slider sliSuspSpringForce;
    public Slider sliSuspDamperForce;
    public Slider sliFrictForwForce;
    public Slider sliFrictSideForce;
    public Slider sliMaxTorqueForce;
    public TMP_Text txtDistanceNum;
    public TMP_Text txtSpringNum;
    public TMP_Text txtDamperNum;
    public TMP_Text txtFrictForwNum;
    public TMP_Text txtFrictSideNum;
    public TMP_Text txtMaxTorqueNum;
    private Prefs _prefs;
    
    // Start is called before the first frame update
    void Start()
    {
        _prefs = new Prefs();
        _prefs.Load();
        _prefs.SetAll(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR, ref buggy);
        sliSuspDistance.value = _prefs.suspensionDistance;
        sliSuspSpringForce.value = _prefs.suspensionSpringForce;
        sliSuspDamperForce.value = _prefs.suspensionDamperForce;
        sliFrictForwForce.value = _prefs.forwardStiffness;
        sliFrictSideForce.value = _prefs.sidewaysStiffness;
        sliMaxTorqueForce.value = _prefs.maxTorque;
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
        txtSpringNum.text = sliSuspSpringForce.value.ToString("0");
        txtDamperNum.text = sliSuspDamperForce.value.ToString("0");
        txtFrictForwNum.text = sliFrictForwForce.value.ToString("0.0");
        txtFrictSideNum.text = sliFrictSideForce.value.ToString("0.0");
        txtMaxTorqueNum.text = sliMaxTorqueForce.value.ToString("0");
    }
    
    public void OnSliderChangedWheelSettings(float suspDistance)
    {
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
        txtSpringNum.text = sliSuspSpringForce.value.ToString("0");
        txtDamperNum.text = sliSuspDamperForce.value.ToString("0");
        
        txtFrictForwNum.text = sliFrictForwForce.value.ToString("0.0");
        txtFrictSideNum.text = sliFrictSideForce.value.ToString("0.0");
        txtMaxTorqueNum.text = sliMaxTorqueForce.value.ToString("0");
        
        _prefs.suspensionDistance = sliSuspDistance.value;
        _prefs.suspensionSpringForce = sliSuspSpringForce.value;
        _prefs.suspensionDamperForce = sliSuspDamperForce.value;
        
        _prefs.forwardStiffness = sliFrictForwForce.value; 
        _prefs.sidewaysStiffness = sliFrictSideForce.value;
        _prefs.maxTorque = sliMaxTorqueForce.value;
        
        _prefs.SetWheelColliderSettings(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR, ref buggy);
    }
    
    public void OnBtnStartClick()
    {
        _prefs.Save();
        SceneManager.LoadScene("Scene1");
    }
    
    void OnApplicationQuit() { _prefs.Save(); }
}
