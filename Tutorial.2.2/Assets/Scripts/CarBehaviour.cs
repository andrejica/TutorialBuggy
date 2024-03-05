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
        //TODO Tutorial 2.2.7 Task for checking max speeds
        if (BuggyMoves())
        {
            SetMotorTorque(maxTorque * Input.GetAxis("Vertical"));
        }
        Debug.Log($"Buggy speed: {_currentSpeedKMH}");
        // Debug.Log($"Buggy moves forward: {BuggyMoves()}");
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
    private bool BuggyMoves()
    {
        Vector3 velocity = _rigidBody.velocity;
        Vector3 localVel = transform.InverseTransformDirection(velocity);

        switch (localVel.z * Input.GetAxis("Vertical"))
        {
            case > 0 when _currentSpeedKMH <= maxSpeedKMH && _currentSpeedKMH > 0:
                return true;
            case <= 0 when _currentSpeedKMH <= maxSpeedBackwardKMH && _currentSpeedKMH > 0:
                return true;
            default:
                return false;
        }
    }
}
