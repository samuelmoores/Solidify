using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinningItem : MonoBehaviour
{
    public bool x_axis;
    public bool y_axis;
    public bool z_axis;

    // Update is called once per frame
    void Update()
    {
        if (x_axis) { 
            transform.Rotate(100f * Time.deltaTime, 0f, 0f, Space.Self);
        }

        if (y_axis)
        {
            transform.Rotate(0f, 100f * Time.deltaTime,  0f, Space.Self);
        }

        if (z_axis)
        {
            transform.Rotate(0f, 0f, 100f * Time.deltaTime,  Space.Self);
        }
    }
}
