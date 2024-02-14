using UnityEngine;

public static class FABRIKSolver
{

    #region public
    
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
            Forward(ref bones, ref curPos, ref curRot, ref settings);
            Backward(ref bones, ref curPos, ref curRot, ref settings);
            PoleConstraint(ref target, ref curPos, ref curRot, ref settings);

            // Convergence 
            if ((curPos[0] - bones[0].position).sqrMagnitude < settings.tolerance * settings.tolerance)
            {
                break;
            }

            // Target is reached
            if ((curPos[curPos.Length - 1] - target.position).sqrMagnitude < settings.targetThreshold * settings.targetThreshold)
            {
                break;
            }

            // Damping
            for (int i = 0; i < bones.Length; i++)
            {
                curPos[i] = Vector3.Lerp(curPos[i], bones[i].position, settings.damping);
                bones[i].position = curPos[i];
            }
        }
    }
    #endregion

    #region private
    
    private static void Forward(ref Transform[] bones, ref Vector3[] curPos, ref Quaternion[] curRot,
        ref FABRIKSettings settings)
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

            // Calculate rotation to point bone in the direction of movement
            curRot[i] = Quaternion.LookRotation(dir, bones[i].up);
            bones[i].rotation = curRot[i];
        }
    }

    private static void Backward(ref Transform[] bones, ref Vector3[] curPos, ref Quaternion[] curRot,
        ref FABRIKSettings settings)
    {
        for (int i = bones.Length - 1; i > 0; i--)
        {
            Vector3 dir = curPos[i - 1] - curPos[i];
            float dis = dir.magnitude;

            if (settings.applyConstraints && settings.jointConstraints != null &&
                settings.jointConstraints.Length > i - 1)
            {
                Vector3 constraintDir = Vector3.ProjectOnPlane(settings.jointConstraints[i - 1], dir);
                curPos[i - 1] = curPos[i] + constraintDir.normalized * dis;
            }
            else
            {
                curPos[i - 1] = curPos[i] + dir.normalized * dis;
            }

            // Calculate rotation to point bone in the direction of movement
            curRot[i] = Quaternion.LookRotation(dir, bones[i].up);
            bones[i].rotation = curRot[i];
        }
    }

    private static void PoleConstraint(ref Transform poleTarget, ref Vector3[] curPos, ref Quaternion[] curRot,
        ref FABRIKSettings settings)
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

                // Calculate the new bone rotation
                Quaternion newRot = angle * Quaternion.LookRotation(curPos[i + 1] - curPos[i], Vector3.up);

                // Update the current rotation array
                curRot[i] = newRot;
            }
        }
    }
    #endregion
}