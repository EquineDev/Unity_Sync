using System.Collections;
using System.Collections.Generic;
using QualisysRealTime.Unity;
using UnityEngine;

[RequireComponent(typeof(RTObject))]
public class QTMRigidbody : MonoBehaviour, IQTMObjectInterface
{
    private RTObject m_RBobject;

    private void Awake()
    {
        m_RBobject = GetComponent<RTObject>();
    }

    public void SetupObject(string objectName)
    {
        m_RBobject.ObjectName = objectName;
    }
}
