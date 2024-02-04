using System.Collections;
using System.Collections.Generic;
using QualisysRealTime.Unity;
using UnityEngine;
[RequireComponent(typeof(RTSkeleton))]
public class QTMHorse : MonoBehaviour, IQTMObjectInterface
{
    private RTSkeleton m_HorseSkeleton;

    private void Awake()
    {
        m_HorseSkeleton = GetComponent<RTSkeleton>();
    }

    public void SetupObject(string objectName)
    {
        m_HorseSkeleton.SkeletonName = objectName;
    }
}
