using TMPro;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public WheelCollider wheelColliderFL;
    public WheelCollider wheelColliderFR;
    public WheelCollider wheelColliderRL;
    public WheelCollider wheelColliderRR;
    private float antiRoll = 5000;
    
    public Transform steeringWheel;
    private float _maxSteeringWheelAngle = 90;
    private float _steerWheelXPos;
    private float _steerWheelZPos;
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
    private bool _isFirstPerson = false;

    public RectTransform speedPointerTransform;
    public TMP_Text speedText;
    private float _tachOnZeroSpeedDeg = -34;
    private float _tachoMaxDeg = -292;

    public Transform centerOfMass;
    private Rigidbody _rigidBody;
    
    public AudioClip engineSingleRpmSoundClip;
    private AudioSource _engineAudioSource;
    private FMODUnity.StudioEventEmitter _engineEventEmitter;
    public bool useFMODEngineSound = true;

    class Gear
    {
        public Gear(float minKMH, float minRPM, float maxKMH, float maxRPM)
        { _minRPM = minRPM;
            _minKMH = minKMH;
            _maxRPM = maxRPM;
            _maxKMH = maxKMH;
        }
        private float _minRPM;
        private float _minKMH;
        private float _maxRPM;
        private float _maxKMH;
        
        public bool SpeedFits(float kmh)
        {
            return kmh >= _minKMH && kmh <= _maxKMH;
        }
        
        public float Interpolate(float kmh)
        {
            float currentRpm;
            
             currentRpm = kmh / _maxKMH * _maxRPM;
             // Debug.Log($"current RPM calculated & Speed: RPM {currentRpm} / Speed {kmh}");

             if (currentRpm > _minRPM)
             {
                 return currentRpm;
             }
            
            return _minRPM;
        }
    }
    
    float KmhToRpm(float kmh, out int gearNum)
    {
        Gear[] gears =
        { new Gear( 1, 900, 12, 1400),
            new Gear( 12, 900, 25, 2000),
            new Gear( 25, 1350, 45, 2500),
            new Gear( 45, 1950, 70, 3500),
            new Gear( 70, 2500, 112, 4000),
            new Gear(112, 3100, 180, 5000)
        };
        
        for (int i=0; i< gears.Length; ++i)
        { if (gears[i].SpeedFits(kmh))
            { gearNum = i + 1;
                return gears[i].Interpolate(kmh);
            }
        }
        gearNum = 1;
        return 800;
    }
    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        var localPositionCenterOfMass = centerOfMass.localPosition;
        _rigidBody.centerOfMass = new Vector3(localPositionCenterOfMass.x, 
            localPositionCenterOfMass.y,
            localPositionCenterOfMass.z);
        SetWheelFrictionStiffness(forewardStiffness, sidewaysStiffness);
        
        _steerWheelXPos = steeringWheel.rotation.eulerAngles.x;
        _steerWheelZPos = steeringWheel.rotation.eulerAngles.z;
        
        if (useFMODEngineSound)
        { 
            // Setup FMOD event emitter
            _engineEventEmitter=GetComponent<FMODUnity.StudioEventEmitter>();
            _engineEventEmitter.Play();
        }
        else
        {
            // Configure AudioSource component by program
            _engineAudioSource = gameObject.AddComponent<AudioSource>();
            _engineAudioSource.clip = engineSingleRpmSoundClip;
            _engineAudioSource.loop = true;
            _engineAudioSource.volume = 0.7f;
            _engineAudioSource.playOnAwake = true;
            _engineAudioSource.enabled = false; // Bugfix
            _engineAudioSource.enabled = true; // Bugfix
        }
    }
    
    void FixedUpdate ()
    {
        _currentSpeedKMH = _rigidBody.velocity.magnitude * 3.6f;

        StabilizeCar();
        
        //Correct angles of both front wheel according to current speed
        float steerAngleCorrection = (1 - _currentSpeedKMH / (maxSpeedKMH * 1.15f)) * maxSteerAngle;
        SetSteerAngle(steerAngleCorrection * Input.GetAxis("Horizontal"));

        BrakeBuggy();
        
        LimitToMaxSpeedBoundaries();
        
        // Debug.Log($"Buggy speed in KM/H: {_currentSpeedKMH}");
        // Debug.Log($"Buggy moves forward: {BuggyMovesForward()}");
        
        int gearNum = 0;
        float engineRPM = KmhToRpm(_currentSpeedKMH, out gearNum);
        // Debug.Log($"current gearNum / RPM / Speed: gearNum {gearNum} / RPM {engineRPM} / Speed {_currentSpeedKMH}");
        SetEngineSound(engineRPM);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBuggyCamera();

        float degAroundY = 0;
        // float test1 = steeringWheel.rotation.eulerAngles.x;
        // float test2 = steeringWheel.rotation.eulerAngles.z;
        //TODO let rotation work correctly for steering wheel...
        if (_currentSpeedKMH > 0)
        {
            degAroundY += _maxSteeringWheelAngle * Input.GetAxis("Horizontal");
        }
        // steeringWheel.SetLocalPositionAndRotation(Vector3.up, Quaternion.Euler(_steerWheelXPos,degAroundY, _steerWheelZPos));
        //); = Quaternion.Euler(_steerWheelXPos,degAroundY, _steerWheelZPos);
        steeringWheel.Rotate(Vector3.up, degAroundY);
    }
    
    void OnGUI()
    {
        
        float degAroundZ = _tachOnZeroSpeedDeg;
        // Speed pointer rotation
        if (_currentSpeedKMH > 0)
        {
            degAroundZ += _tachoMaxDeg * (_currentSpeedKMH / maxSpeedKMH);
        }
  
        speedPointerTransform.rotation = Quaternion.Euler(0,0, degAroundZ); 
        
        //SpeedText show current KMH
        speedText.text = _currentSpeedKMH.ToString("0") + " km/h";
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
    
    void SetEngineSound(float engineRpm)
    {
        if (useFMODEngineSound)
        {
            _engineEventEmitter.SetParameter("RPM", engineRpm); 
        }
        else
        {
            if (ReferenceEquals(_engineAudioSource, null)) return;
            float minRPM = 800;
            float maxRPM = 8000;
            float minPitch = 0.3f;
            float maxPitch = 3.0f;

            float pitch = engineRpm / maxRPM * maxPitch;

            _engineAudioSource.pitch = pitch;
        }
    }

    /// <summary>
    /// toggles Buggy camera between third-person-view and first-person-view
    /// </summary>
    private void ChangeBuggyCamera()
    {
        if (!_isFirstPerson && Input.GetKeyDown(KeyCode.K))
        {
            firstPersonCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);
            _isFirstPerson = true;
        }
        else if (_isFirstPerson && Input.GetKeyDown(KeyCode.K))
        {
            firstPersonCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);
            _isFirstPerson = false;
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
