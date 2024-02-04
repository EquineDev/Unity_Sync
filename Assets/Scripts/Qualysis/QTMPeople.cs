using System.Collections;
using System.Collections.Generic;
using QualisysRealTime.Unity;
using UnityEngine;
[RequireComponent(typeof(RTSkeleton))]
public class QTMPeople : MonoBehaviour, IQTMObjectInterface
{
    private RTSkeleton m_People;

    private void Awake()
    {
        m_People = GetComponent<RTSkeleton>();
    }

    public void SetupObject(string objectName)
    {
        m_People.SkeletonName = objectName;
    }
}
