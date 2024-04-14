using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingBehaviour : MonoBehaviour
{
    public int countMax = 3;
    private int _countDown;
    // Use this for initialization
    void Start()
    {
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
    }
}
