
using UnityEngine;

[CreateAssetMenu(fileName = "CCDIKSettings", menuName = "IKSolver/CCDIK")]
public class CCDIKSettings : ScriptableObject
{
    [Header("CCDIK Settings")]
    public int maxIterations = 100; 
    public float tolerance = 0.001f; 

    [Header("Rotation Limits")]
    public bool useRotationLimits = false;
    public float rotationLimitAngle = 90f;
}
