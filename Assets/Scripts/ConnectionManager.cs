using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : Singleton<ConnectionManager>
{

    [SerializeField]
    private QTMManager m_QTMManager;


    public ref QTMManager GetQTMManager()
    {
        return ref m_QTMManager;
    }
}
