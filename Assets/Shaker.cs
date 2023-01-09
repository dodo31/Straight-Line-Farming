using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField]
    float power;
    [SerializeField]
    float length;
    [SerializeField]
    bool isCamera;

    public static Shaker CameraShaker;

    Vector3 pos;
    float powerMult = 0f;
    private void Start()
    {
        if (isCamera) CameraShaker = this;
        pos = transform.position;
    }

    public void Shake()
    {
        powerMult = 1;
    }
    private void Update()
    {
        if(powerMult > 0)
        {
            powerMult -= 1 / length * Time.deltaTime;
            transform.position = new Vector3(pos.x + Mathf.Sin(Time.time*200f) * power * powerMult, pos.y, pos.z);
        }
    }
}
