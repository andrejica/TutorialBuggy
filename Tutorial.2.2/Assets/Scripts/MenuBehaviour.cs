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
    
    public void OnSliderChangedSuspDistance(float suspDistance)
    {
        txtDistanceNum.text = sliSuspDistance.value.ToString("0.00");
        
        _prefs.suspensionDistance = sliSuspDistance.value;
        
        _prefs.SetWheelColliderSuspension(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }

    public void OnSliderChangedSuspSpring(float suspSpring)
    {
        txtSpringNum.text = sliSuspSpringForce.value.ToString("0");
        _prefs.suspensionSpringForce = sliSuspSpringForce.value;
        
        _prefs.SetWheelColliderSuspensionSpring(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }
    
    public void OnSliderChangedSuspDamper(float suspDamper)
    {
        txtDamperNum.text = sliSuspDamperForce.value.ToString("0");
        _prefs.suspensionDamperForce = sliSuspDamperForce.value;
        
        _prefs.SetWheelColliderSuspensionDamper(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }
    
    public void OnSliderChangedForwardStiffness(float forwardStiffnes)
    {
        txtFrictForwNum.text = sliFrictForwForce.value.ToString("0.0");
        _prefs.forwardStiffness = sliFrictForwForce.value;
        
        _prefs.SetWheelColliderForwardStiffness(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }
    
    public void OnSliderChangedSidewaysStiffness(float sidewaysStiffnes)
    {
        txtFrictSideNum.text = sliFrictSideForce.value.ToString("0.0");
        _prefs.sidewaysStiffness = sliFrictSideForce.value;
        
        _prefs.SetWheelColliderSidewaysStiffness(ref wheelColliderFL, ref wheelColliderFR, ref wheelColliderRL, ref wheelColliderRR);
    }
    
    public void OnSliderChangedMaxTorque(float maxTorque)
    {
        txtMaxTorqueNum.text = sliMaxTorqueForce.value.ToString("0");
        _prefs.maxTorque = sliMaxTorqueForce.value;
        
        _prefs.SetBuggyMaxTorque(ref buggy);
    }
    
    public void OnBtnStartClick()
    {
        _prefs.Save();
        SceneManager.LoadScene("Scene1");
    }
    
    void OnApplicationQuit() { _prefs.Save(); }
}
