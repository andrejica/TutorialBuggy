using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public float maxTorque = 500;
    public float maxSteerAngle = 45;
    
    void Start () {}
    
    void FixedUpdate ()
    { SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        SetSteerAngle(maxSteerAngle * Input.GetAxis("Horizontal"));
    }
    
    void SetSteerAngle(float angle)
    { wheelColliderFL.steerAngle = angle;
        wheelColliderFR.steerAngle = angle;
    }
    
    void SetMotorTorque(float amount)
    { wheelColliderFL.motorTorque = amount;
        wheelColliderFR.motorTorque = amount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
