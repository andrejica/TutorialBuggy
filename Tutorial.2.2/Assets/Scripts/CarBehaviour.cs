using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderRL;
    public WheelCollider wheelColliderRR;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;

    public float maxSpeedKMH = 150;
    public float maxSpeedBackwardKMH = 30;
    private float _currentSpeedKMH = 0f;

    void Start()
    {
        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    }
    
    void FixedUpdate ()
    { 
        SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));
    }
    
    void SetSteerAngle(float angle)
    { 
        wheelColliderFL.steerAngle = angle;
        wheelColliderFR.steerAngle = angle;
    }
    
    void SetMotorTorque(float amount)
    { 
        wheelColliderFL.motorTorque = amount;
        wheelColliderFR.motorTorque = amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SetWheelFrictionStiffness(float newForwardStiffness, float newSidewaysStiffness)
    {
        WheelFrictionCurve fwWFC = wheelColliderFL.forwardFriction;
        WheelFrictionCurve swWFC = wheelColliderFL.sidewaysFriction;
        fwWFC.stiffness = newForwardStiffness;
        swWFC.stiffness = newSidewaysStiffness;
        wheelColliderFL.forwardFriction = fwWFC;
        wheelColliderFL.sidewaysFriction = swWFC;
        wheelColliderFR.forwardFriction = fwWFC;
        wheelColliderFR.sidewaysFriction = swWFC;
        wheelColliderRL.forwardFriction = fwWFC;
        wheelColliderRL.sidewaysFriction = swWFC;
        wheelColliderRR.forwardFriction = fwWFC;
        wheelColliderRR.sidewaysFriction = swWFC;
    }
}
