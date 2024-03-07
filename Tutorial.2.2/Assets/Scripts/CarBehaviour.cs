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
    private float _testAngle;
    public float sidewaysStiffness = 1.5f;
    public float forewardStiffness = 1.5f;

    public float maxSpeedKMH = 150;
    public float maxSpeedBackwardKMH = 30;
    private float _currentSpeedKMH = 0f;

    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;
    private bool isFirstPerson = false;

    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    }
    
    void FixedUpdate ()
    {
        _currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;
        
        //Stop speed increase if goes over MaxForward or MaxBackwards Speed
        if (BuggyMovesForward() && _currentSpeedKMH <= maxSpeedKMH)
        {
            SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        }
        else if(!BuggyMovesForward() && _currentSpeedKMH <= maxSpeedBackwardKMH)
        {
            SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        }
        else
        {
            SetMotorTorque(0);
        }
        
        //TODO calculate angle of wheels according to speed of buggy
        // var test = 
        
        Debug.Log($"Buggy speed in KM/H: {_currentSpeedKMH}");
        // Debug.Log($"Buggy moves forward: {BuggyMovesForward()}");
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
        ChangeBuggyCamera();
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

    private void ChangeBuggyCamera()
    {
        if (!isFirstPerson && Input.GetKeyDown(KeyCode.K))
        {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
            isFirstPerson = true;
        }
        else if (isFirstPerson && Input.GetKeyDown(KeyCode.K))
        {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            isFirstPerson = false;
        }
    }

    //TODO Tutorial 2.2.7 Task for checking max speeds
    private bool BuggyMovesForward()
    {
        Vector3 velocity = _rigidBody.velocity;
        Vector3 localVel = transform.InverseTransformDirection(velocity);

        return localVel.z > 0;
    }
}
