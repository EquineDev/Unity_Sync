using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using QualisysRealTime.Unity;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(RTForcePlate))]
public class QTMForcePlate : MonoBehaviour, IQTMObjectInterface
{

    private RTForcePlate m_ForcePlate;

    private void Awake()
    {
        m_ForcePlate = GetComponent<RTForcePlate>();
    }

    public void SetupObject(string objectName)
    {
        m_ForcePlate.forcePlateName = objectName;
    }
}
