using UnityEngine;

public class WheelBehaviour : MonoBehaviour
{
    public WheelCollider wheelCol;
    private SkidmarkBehaviour _skidmarkBehaviour; // skidmark script
    private int _skidmarkLast; // index of last skidmark
    private Vector3 _skidmarkLastPos; // position of last skidmark
    void Start ()
    {
        _skidmarkBehaviour = GameObject.Find("Buggy").GetComponent<SkidmarkBehaviour>();
        _skidmarkLast = -1;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Get the wheel position and rotation from the wheel collider
        wheelCol.GetWorldPose(out Vector3 position, out Quaternion rotation);
        var myTransform = transform;
        myTransform.position = position;
        myTransform.rotation = rotation;
    }
    
    // Creates skidmarks if handbraking
    public void DoSkidmarking(bool doSkidmarking)
    {
        if (doSkidmarking)
        {
// do nothing if the wheel isn't touching the ground
            if(!wheelCol.GetGroundHit(out WheelHit hit)) return;
// absolute velocity at wheel in world space
            Vector3 wheelVelo =
                wheelCol.attachedRigidbody.GetPointVelocity(hit.point);
            if (Vector3.Distance(_skidmarkLastPos, hit.point) > 0.1f)
            { _skidmarkLast =
                    _skidmarkBehaviour.Add(hit.point + wheelVelo*Time.deltaTime,
                        hit.normal,
                        0.5f,
                        _skidmarkLast);
                _skidmarkLastPos = hit.point;
            }
        } else _skidmarkLast = -1;
    }
}
