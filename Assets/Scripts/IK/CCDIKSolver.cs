using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDIKSolver : MonoBehaviour
{
    public static void Solve(ref Transform target, ref Transform[] segments, ref CCDIKSettings settings)
    {
        Vector3 targetPosition = target.position;

        for (int i = segments.Length - 1; i >= 0; i--)
        {
            Vector3 segmentPosition = segments[i].position;
            Vector3 toTarget = targetPosition - segmentPosition;
            Vector3 toEndEffector = segments[segments.Length - 1].position - segmentPosition;

            Vector3 rotationAxis = Vector3.Cross(toEndEffector.normalized, toTarget.normalized);
            float angleDiff = Vector3.Angle(toEndEffector, toTarget);

            RotateSegment(ref segments[i], rotationAxis, angleDiff);

            if (i == 0)
            {
                Vector3 endEffectorPosition = segments[segments.Length - 1].position;
                float distanceToTarget = Vector3.Distance(endEffectorPosition, targetPosition);

                if (distanceToTarget < settings.tolerance || Mathf.Abs(angleDiff) < settings.tolerance || settings.maxIterations <= 0)
                    return;
            }
        }
    }

    static void RotateSegment(ref Transform segment, Vector3 axis, float angle)
    {
        segment.Rotate(axis, angle, Space.World);
    }
}
