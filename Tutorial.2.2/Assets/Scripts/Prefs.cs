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
        suspensionDamperForce = PlayerPrefs.GetFloat("suspensionDamperForce", 4000f);
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
        SetWheelColliderSuspension(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
        SetWheelColliderSuspensionSpring(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
        SetWheelColliderSuspensionDamper(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
        SetWheelColliderForwardStiffness(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
        SetWheelColliderSidewaysStiffness(ref wheelFL, ref wheelFR, ref wheelRL, ref wheelRR);
        SetBuggyMaxTorque(ref buggy);
        
    }

    public void SetWheelColliderSuspension(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR)
    {
        //https://forum.unity.com/threads/changing-suspension-spring.267623/
        JointSpring suspSpring = wheelFL.suspensionSpring;
        
        wheelFL.suspensionDistance = suspensionDistance;
        wheelFR.suspensionDistance = suspensionDistance;
        wheelRL.suspensionDistance = suspensionDistance;
        wheelRR.suspensionDistance = suspensionDistance;
    }

    public void SetWheelColliderSuspensionSpring(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR)
    {
        JointSpring suspSpring = wheelFL.suspensionSpring;
        suspSpring.spring = suspensionSpringForce;
        
        wheelFL.suspensionSpring = suspSpring;
        wheelFR.suspensionSpring = suspSpring;
        wheelRL.suspensionSpring = suspSpring;
        wheelRR.suspensionSpring = suspSpring;
    }
    
    public void SetWheelColliderSuspensionDamper(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR)
    {
        JointSpring suspSpring = wheelFL.suspensionSpring;
        
        suspSpring.damper = suspensionDamperForce;
        wheelFL.suspensionSpring = suspSpring;
        wheelFR.suspensionSpring = suspSpring;
        wheelRL.suspensionSpring = suspSpring;
        wheelRR.suspensionSpring = suspSpring;
    }

    public void SetWheelColliderForwardStiffness(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR)
    {
        WheelFrictionCurve fwWFC = wheelFL.forwardFriction;
        fwWFC.stiffness = forwardStiffness;
        
        wheelFL.forwardFriction = fwWFC;
        wheelFR.forwardFriction = fwWFC;
        wheelRL.forwardFriction = fwWFC;
        wheelRR.forwardFriction = fwWFC;
    }
    
    public void SetWheelColliderSidewaysStiffness(
        ref WheelCollider wheelFL,
        ref WheelCollider wheelFR,
        ref WheelCollider wheelRL,
        ref WheelCollider wheelRR)
    {
        WheelFrictionCurve swWFC = wheelFL.sidewaysFriction;
        swWFC.stiffness = sidewaysStiffness;
        
        wheelFL.sidewaysFriction = swWFC;
        wheelFR.sidewaysFriction = swWFC;
        wheelRL.sidewaysFriction = swWFC;
        wheelRR.sidewaysFriction = swWFC;
    }

    public void SetBuggyMaxTorque(ref GameObject buggy)
    {
        CarBehaviour carScript = buggy.GetComponent<CarBehaviour>();
        carScript.maxTorque = maxTorque;
    }
}
