using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When moving the space bubble with the curve we change its position a bit at a time
/// To prevent the space bubble to move and be left behind by the curve we reallocate it 
/// frame by frame deleting the delta change of position the curve needs. Not elegant but it works
/// </summary>
public class CurveSpaceReallocator : MonoBehaviour
{
    void Update()
    {
        gameObject.transform.position = Vector3.zero;
    }
}
