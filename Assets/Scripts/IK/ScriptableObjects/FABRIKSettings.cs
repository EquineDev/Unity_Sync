
using UnityEngine;
[CreateAssetMenu(fileName = "FABRIKSettings", menuName = "IKSolver/FABRIK")]
public class FABRIKSettings : ScriptableObject
{
    [Header("Solver Parameters")]
    public float tolerance = 0.1f;
    public float targetThreshold = 0.1f;
    public int maxIterations = 10;
    public bool applyConstraints = true;
    public float damping = 0.5f;

    [Header("Joint Constraints")]
    public Vector3[] jointConstraints;
}
