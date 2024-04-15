using System.Collections;
using TMPro;
using UnityEngine;

public class TimingBehaviour : MonoBehaviour
{
    public int countMax = 3;
    private int _countDown;
    public TMP_Text timeText;
    
    private float _pastTime = 0;
    private bool _isFinished = false;
    private bool _isStarted = false;

    private CarBehaviour _carScript;
    
    // Use this for initialization
    void Start()
    {
        _carScript = GameObject.Find("Buggy").GetComponent<CarBehaviour>();
        _carScript.thrustEnabled = false;
        
        print("Begin Start:" + Time.time);
        StartCoroutine(GameStart());
        print("End Start:" + Time.time);
    }
    
    // GameStart CoRoutine
    IEnumerator GameStart()
    { 
        print(" Begin GameStart:" + Time.time);
        for(_countDown = countMax; _countDown > 0; _countDown--)
        { 
            yield return new WaitForSeconds(1);
            print(" WaitForSeconds:" + Time.time);
        }
        
        print(" End GameStart:" + Time.time);

        _carScript.thrustEnabled = true;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        { if (!_isStarted)
                _isStarted = true;
            else _isFinished = true;
        }
    }
    
    void OnGUI ()
    {
        //TODO correct time measure start... (2.6)
        //TODO peep sound before race start.
        if (_carScript.thrustEnabled)
        {
            if (_isStarted && !_isFinished)
                _pastTime += Time.deltaTime;
            timeText.text = _pastTime.ToString("0.0") + " sec.";
        }
        else
            timeText.text = _countDown.ToString("0.0") + " sec.";
    }
}
