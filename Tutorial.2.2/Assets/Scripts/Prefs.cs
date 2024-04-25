using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefs
{
    public float suspensionDistance;
    public float suspensionSpringForce;
    public float suspensionDamperForce;
    public float forwardStiffness;
    public float sidewaysStiffness;
    public float maxTorque;
    
    public void Load()
    { 
        suspensionDistance = PlayerPrefs.GetFloat("suspensionDistance", 0.2f);
        suspensionSpringForce = PlayerPrefs.GetFloat("suspensionSpringForce", 35000f);
        suspensionDamperForce = PlayerPrefs.GetFloat("suspensionDamperForce", 4500f);
        forwardStiffness = PlayerPrefs.GetFloat("forwardStiffness", 1.0f);
        sidewaysStiffness = PlayerPrefs.GetFloat("sidewaysStiffness", 1.0f);
        maxTorque = PlayerPrefs.GetFloat("maxTorque", 1500f);
    }
    
    public void Save()
    { 
        PlayerPrefs.SetFloat("suspensionDistance", suspensionDistance);
        PlayerPrefs.SetFloat("suspensionSpringForce", suspensionSpringForce);
        PlayerPrefs.SetFloat("suspensionDamperForce", suspensionDamperForce);
        PlayerPrefs.SetFloat("forwardStiffness", forwardStiffness);
        PlayerPrefs.SetFloat("sidewaysStiffness", sidewaysStiffness);
        PlayerPrefs.SetFloat("maxTorque", maxTorque);
    }
    
    public void SetAll(
        ref WheelCollider wheelFL, 
        ref WheelCollider wheelFR, 
        ref WheelCollider wheelRL, 
        ref WheelCollider wheelRR,
        ref GameObject buggy)
    { 
        SetWheelColliderSettings(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR, ref buggy);
    }

    public void SetWheelColliderSettings(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR,
        ref GameObject buggy)
    {
        //https://forum.unity.com/threads/changing-suspension-spring.267623/
        JointSpring suspSpring = wheelFL.suspensionSpring;
        WheelFrictionCurve fwWFC = wheelFL.forwardFriction;
        WheelFrictionCurve swWFC = wheelFL.sidewaysFriction;
        CarBehaviour carScript = buggy.GetComponent<CarBehaviour>();
        carScript.maxTorque = maxTorque;
        suspSpring.spring = suspensionSpringForce;
        suspSpring.damper = suspensionDamperForce;
        fwWFC.stiffness = forwardStiffness;
        swWFC.stiffness = sidewaysStiffness;
        
        wheelFL.suspensionDistance = suspensionDistance;
        wheelFL.suspensionSpring = suspSpring;
        wheelFL.forwardFriction = fwWFC;
        wheelFL.sidewaysFriction = swWFC;
        
        wheelFR.suspensionDistance = suspensionDistance;
        wheelFR.suspensionSpring = suspSpring;
        wheelFR.forwardFriction = fwWFC;
        wheelFR.sidewaysFriction = swWFC;
        
        wheelRL.suspensionDistance = suspensionDistance;
        wheelRL.suspensionSpring = suspSpring;
        wheelRL.forwardFriction = fwWFC;
        wheelRL.sidewaysFriction = swWFC;
        
        wheelRR.suspensionDistance = suspensionDistance;
        wheelRR.suspensionSpring = suspSpring;
        wheelRR.forwardFriction = fwWFC;
        wheelRR.sidewaysFriction = swWFC;
    }
}
