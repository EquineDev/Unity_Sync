
using UnityEngine;

public static class FABRIKSolver 
{
    public static void Solve(ref Transform target, ref Transform[] bones, ref FABRIKSettings settings)
    {
        if (target == null || bones == null || bones.Length == 0 || settings == null)
        {
            Debug.LogWarning("FABRIK parameters are not set correctly.");
            return;
        }
        
        Vector3[] curPos = new Vector3[bones.Length];
        Quaternion[] curRot = new Quaternion[bones.Length];
        
        for (int i = 0; i < bones.Length; i++)
        {
            curPos[i] = bones[i].position;
            curRot[i] = bones[i].rotation;
        }

        for (int itr = 0; itr < settings.maxIterations; itr++)
        {
           
            Foward(ref bones, ref curPos, ref settings);
            Backwards(ref bones, ref curPos, ref settings);
            PoleConstraint(ref target, ref curPos);

            // Convergence 
            if ((curPos[0] - bones[0].position).sqrMagnitude < settings.tolerance * settings.tolerance)
            {
                break;
            }
               

            // target is reached
            if ((curPos[curPos.Length - 1] - target.position).sqrMagnitude < settings.targetThreshold *
                settings.targetThreshold)
            {
                break;
            }
              

            //damping
            for (int i = 0; i < bones.Length; i++)
            {
                curPos[i] = Vector3.Lerp(curPos[i], bones[i].position, settings.damping);
                bones[i].position = curPos[i];
            }
        }
    }

    private static void Foward(ref Transform[] bones, ref Vector3[] curPos, ref FABRIKSettings settings )
    {
        for (int i = 0; i < bones.Length - 1; i++)
        {
            Vector3 dir = curPos[i + 1] - curPos[i];
            float dis = dir.magnitude;

            if (settings.applyConstraints && settings.jointConstraints != null && settings.jointConstraints.Length > i)
            {
                Vector3 constraintDir = Vector3.ProjectOnPlane(settings.jointConstraints[i], dir);
                curPos[i + 1] = curPos[i] + constraintDir.normalized * dis;
            }
            else
            {
                curPos[i + 1] = curPos[i] + dir.normalized * dis;
            }
        }
    }

    private static void Backwards(ref Transform[] bones, ref Vector3[] curPos, ref FABRIKSettings settings)
    {
        for (int i = bones.Length - 1; i > 0; i--)
        {
            Vector3 dir = curPos[i - 1] - curPos[i];
            float dis = dir.magnitude;
        
            if (settings.applyConstraints && settings.jointConstraints != null && settings.jointConstraints.Length > i - 1)
            {
                Vector3 constraintDir = Vector3.ProjectOnPlane(settings.jointConstraints[i - 1], dir);
                curPos[i - 1] = curPos[i] + constraintDir.normalized * dis;
            }
            else
            {
                curPos[i - 1] = curPos[i] + dir.normalized * dis;
            }
        }
        
    }
    
    private static void PoleConstraint(ref Transform poleTarget, ref Vector3[] curPos)
    {
        if (poleTarget != null && curPos.Length >= 2)
        {
            for (int i = 0; i < curPos.Length - 1; i++)
            {
                Vector3 limbAxis = (curPos[i + 1] - curPos[i]).normalized;

                Vector3 poleDirection = (poleTarget.position - curPos[i]).normalized;
                Vector3 boneDirection = (curPos[Mathf.Min(i + 1, curPos.Length - 1)] - curPos[i]).normalized;

                Vector3.OrthoNormalize(ref limbAxis, ref poleDirection);
                Vector3.OrthoNormalize(ref limbAxis, ref boneDirection);

                Quaternion angle = Quaternion.FromToRotation(boneDirection, poleDirection);

                curPos[Mathf.Min(i + 1, curPos.Length - 1)] = angle * 
                    (curPos[Mathf.Min(i + 1, curPos.Length - 1)] - curPos[i]) + curPos[i];
            }
        }
    }
}
