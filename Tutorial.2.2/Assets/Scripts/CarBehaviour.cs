using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderRL;
    public WheelCollider wheelColliderRR;
    private float antiRoll = 5000;
    
    public Transform steeringWheel;
    public float maxSteeringWheelAngle = 89;
    float maxTorque = 1500;
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

    public Transform centerOfMass;
    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        var localPositionCenterOfMass = centerOfMass.localPosition;
        _rigidBody.centerOfMass = new Vector3(localPositionCenterOfMass.x, 
            localPositionCenterOfMass.y,
            localPositionCenterOfMass.z);
        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
    }
    
    void FixedUpdate ()
    {
        _currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;

        StabilizeCar();

        //TODO somehow the car turns slightly when driving ca. 150 KM/H and slides afterwards... why?
        //Correct angles of both front wheel according to current speed
        var steerAngleCorrection = 1 - _currentSpeedKMH / maxSpeedKMH;
        SetSteerAngle(steerAngleCorrection * maxSteerAngle * Input.GetAxis("Horizontal"));

        BrakeBuggy();
        
        LimitToMaxSpeedBoundaries();
        
        Debug.Log($"Buggy speed in KM/H: {_currentSpeedKMH}");
        // Debug.Log($"Buggy moves forward: {BuggyMovesForward()}");
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
        steeringWheel.transform.Rotate(0f, maxSteeringWheelAngle * Input.GetAxis("Horizontal"), 0f);
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

    /// <summary>
    /// toggles Buggy camera between third-person-view and first-person-view
    /// </summary>
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

    /// <summary>
    /// Check for _currentSpeed and limit for forward and backwards max speed limits
    /// </summary>
    private void LimitToMaxSpeedBoundaries()
    {
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
    }
    
    private bool BuggyMovesForward()
    {
        Vector3 velocity = _rigidBody.velocity;
        Vector3 localVel = transform.InverseTransformDirection(velocity);

        return localVel.z > 0;
    }

    /// <summary>
    /// Stabilizer for buggy
    /// Used script and adapted to this project from this URL https://forum.unity.com/threads/how-to-make-a-physically-real-stable-car-with-wheelcolliders.50643/
    /// </summary>
    private void StabilizeCar()
    {
        var travelL = 1.0;
        var travelR = 1.0;
 
        var groundedL = wheelColliderFL.GetGroundHit(out var hitFl);
        if (groundedL)
            travelL = (-wheelColliderFL.transform.InverseTransformPoint(hitFl.point).y - wheelColliderFL.radius) / wheelColliderFL.suspensionDistance;
 
        var groundedR = wheelColliderFR.GetGroundHit(out var hitFr);
        if (groundedR)
            travelR = (-wheelColliderFR.transform.InverseTransformPoint(hitFr.point).y - wheelColliderFR.radius) / wheelColliderFR.suspensionDistance;
 
        var antiRollForce = (travelL - travelR) * antiRoll;
 
        if (groundedL)
        {
            var transformWheelFl = wheelColliderFL.transform;
            _rigidBody.AddForceAtPosition(transformWheelFl.up * (float)-antiRollForce,
                transformWheelFl.position);
        }

        if (groundedR)
        {
            var transformWheelFr = wheelColliderFR.transform;
            _rigidBody.AddForceAtPosition(transformWheelFr.up * (float)antiRollForce,
                transformWheelFr.position);
        }
    }

    /// <summary>
    /// Buggy will brake and stop if pressing opposite direction of current movement
    /// </summary>
    private void BrakeBuggy()
    {
        // Determine if the cursor key input means braking
        bool doBraking = _currentSpeedKMH > 0.5f &&
                         (Input.GetAxis("Vertical") < 0 && BuggyMovesForward() ||
                          Input.GetAxis("Vertical") > 0 && !BuggyMovesForward());

        if (doBraking)
        { 
            wheelColliderFL.brakeTorque = 5000;
            wheelColliderFR.brakeTorque = 5000;
            wheelColliderRL.brakeTorque = 5000;
            wheelColliderRR.brakeTorque = 5000;
            wheelColliderFL.motorTorque = 0;
            wheelColliderFR.motorTorque = 0;
        } 
        else
        { 
            wheelColliderFL.brakeTorque = 0;
            wheelColliderFR.brakeTorque = 0;
            wheelColliderRL.brakeTorque = 0;
            wheelColliderRR.brakeTorque = 0;
            wheelColliderFL.motorTorque = maxTorque * Input.GetAxis("Vertical");
            wheelColliderFR.motorTorque = wheelColliderFL.motorTorque;
        }
    }
}
