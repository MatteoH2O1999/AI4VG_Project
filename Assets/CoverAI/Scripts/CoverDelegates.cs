using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoverDelegates : MonoBehaviour
{
    abstract public Vector3 GetPositionOnTerrain(Vector3 position);
    abstract public bool IsReachable(Vector3 startPosition, Vector3 endPosition);
    abstract public bool IsOnTerrain(Vector3 position);
}
