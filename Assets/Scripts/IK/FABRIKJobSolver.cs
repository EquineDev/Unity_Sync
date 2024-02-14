using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


public static class FABRIKJobSolver
{
    public static void Solve(ref Transform target, ref Transform[] bones, ref FABRIKSettings settings)
    {
        // Create NativeArrays to hold bone positions and rotations
        NativeArray<Vector3> bonePositions = new NativeArray<Vector3>(bones.Length, Allocator.TempJob);
        NativeArray<Quaternion> boneRotations = new NativeArray<Quaternion>(bones.Length, Allocator.TempJob);

        // Copy bone positions and rotations to NativeArrays
        for (int i = 0; i < bones.Length; i++)
        {
            bonePositions[i] = bones[i].position;
            boneRotations[i] = bones[i].rotation;
        }

        // Create FABRIK job
        FABRIKJob fabrikJob = new FABRIKJob
        {
            target = target.position,
            bonePositions = bonePositions,
            boneRotations = boneRotations,
            settings = settings
        };

        // Schedule the job
        JobHandle jobHandle = fabrikJob.Schedule(bones.Length, 64);

        // Complete the job
        jobHandle.Complete();

        // Copy back the result to bone positions
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = bonePositions[i];
            bones[i].rotation = boneRotations[i];
        }

        // Dispose NativeArrays
        bonePositions.Dispose();
        boneRotations.Dispose();
    }
}

public struct FABRIKJob  : IJobParallelFor
{
    public Vector3 target;
    public NativeArray<Vector3> bonePositions;
    public NativeArray<Quaternion> boneRotations;
    [ReadOnly] public NativeArray<Vector3> boneDirections;
    public FABRIKSettings settings;
    
  
    
   public void Execute(int index)
    {
        // Forward
        for (int i = index; i < bonePositions.Length - 1; i++)
        {
            Vector3 dir = bonePositions[i + 1] - bonePositions[i];
            float dis = dir.magnitude;

            if (settings.applyConstraints && settings.jointConstraints != null && settings.jointConstraints.Length > i)
            {
                Vector3 constraintDir = ProjectOnPlane(settings.jointConstraints[i], dir);
                bonePositions[i + 1] = bonePositions[i] + constraintDir.normalized * dis;
            }
            else
            {
                bonePositions[i + 1] = bonePositions[i] + boneDirections[i] * dis;
            }
        }

        // Backwards
        for (int i = index; i > 0; i--)
        {
            Vector3 dir = bonePositions[i - 1] - bonePositions[i];
            float dis = dir.magnitude;

            if (settings.applyConstraints && settings.jointConstraints != null && settings.jointConstraints.Length > i - 1)
            {
                Vector3 constraintDir = ProjectOnPlane(settings.jointConstraints[i - 1], dir);
                bonePositions[i - 1] = bonePositions[i] + constraintDir.normalized * dis;
            }
            else
            {
                bonePositions[i - 1] = bonePositions[i] + boneDirections[i - 1] * dis;
            }
        }
        
        for (int i = index; i < bonePositions.Length - 1; i++)
        {
            Vector3 dir = bonePositions[i + 1] - bonePositions[i];
            boneRotations[i] = LookRotation(dir, boneDirections[i]);
        }
        
        // PoleConstraint
        if (index < bonePositions.Length - 1)
        {
            Vector3 limbAxis = (bonePositions[index + 1] - bonePositions[index]).normalized;
            Vector3 poleDirection = (target - bonePositions[index]).normalized;
            Vector3 boneDirection = boneDirections[index];

            CustomOrthoNormalize(ref limbAxis, ref poleDirection);
            CustomOrthoNormalize(ref limbAxis, ref boneDirection);

            Quaternion angle = FromToRotation(boneDirection, poleDirection);

            bonePositions[index + 1] = angle * (bonePositions[index + 1] - bonePositions[index]) + bonePositions[index];

            // Update bone rotation
            boneRotations[index + 1] = angle * boneRotations[index + 1];
        }
    }

    // job friendly of ProjectOnPlane
    private Vector3 ProjectOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        Vector3 projected = vector - Dot(vector, planeNormal) * planeNormal;
        return projected;
    }

    // job friendly of OrthoNormalize
    private void CustomOrthoNormalize(ref Vector3 v1, ref Vector3 v2)
    {
        Vector3 projection = Dot(v2, v1) * v1;
        v2 -= projection;
        v2.Normalize();
    }
    
    //job friendly vector3.dot
    private float Dot(Vector3 lhs, Vector3 rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }
    
    //job friendly  Vector3.Cross
    private Vector3 CalculateRotationAxis(Vector3 fromDirection, Vector3 toDirection)
    {
       
        float x = fromDirection.y * toDirection.z - fromDirection.z * toDirection.y;
        float y = fromDirection.z * toDirection.x - fromDirection.x * toDirection.z;
        float z = fromDirection.x * toDirection.y - fromDirection.y * toDirection.x;

        return new Vector3(x, y, z);
    }
    
    //job friendly Quaternion.AxisAngleToQuaternion
    private Quaternion AxisAngleToQuaternion(Vector3 axis, float angle)
    {
        float halfAngle = angle * 0.5f;
        float s = (float)Math.Sin(halfAngle);
        
        return new Quaternion(axis.x * s, axis.y * s, axis.z * s, (float)Math.Cos(halfAngle));
    }
    //job friend Quaternion.FromToRotation 
    private Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        float dot = Dot(fromDirection, toDirection);

        if (dot > 0.99999f) // Vectors are (almost) parallel
            return Quaternion.identity;

        if (dot < -0.99999f) // Vectors are (almost) anti-parallel
            return Quaternion.AngleAxis(180f, Vector3.up);

        Vector3 axis = CalculateRotationAxis(fromDirection, toDirection);
        float angle = angle = (float)Math.Acos(Math.Min(Math.Max(dot, -1f), 1f));

        return AxisAngleToQuaternion(axis, angle);
    }
    //job friend Quaternion.LookRotation 
    private Quaternion LookRotation(Vector3 forward, Vector3 upwards)
    {
        if (forward == Vector3.zero)
        {
            return Quaternion.identity;
        }
         

        Vector3 normalizedForward = Vector3.Normalize(forward);

        if (CalculateRotationAxis(upwards, normalizedForward) == Vector3.zero)
        {
            return Quaternion.LookRotation(Vector3.Cross(Vector3.up, normalizedForward), Vector3.up);
        }
         

        Vector3 right = Vector3.Normalize(CalculateRotationAxis(upwards, normalizedForward));
        Vector3 newUp = CalculateRotationAxis(normalizedForward, right);

        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetColumn(0, new Vector4(right.x, right.y, right.z, 0));
        matrix.SetColumn(1, new Vector4(newUp.x, newUp.y, newUp.z, 0));
        matrix.SetColumn(2, new Vector4(normalizedForward.x, normalizedForward.y, normalizedForward.z, 0));
        matrix.SetColumn(3, new Vector4(0, 0, 0, 1));

        return Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
    }
}