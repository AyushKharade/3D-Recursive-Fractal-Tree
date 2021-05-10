using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour
{
    //variables
    public float scalingFactor;
    public float depth;

    public void SetData(float scaleFactor, float d)
    {
        scalingFactor = scaleFactor;
        depth = d;
    }
}
