using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using QualisysRealTime.Unity;
using UnityEngine;

public class QTMManager : NetworkBehaviour
{
    public RTConnectionState GetClientConnectionState()
    {
        return RTClient.GetInstance().ConnectionState;
    }

    public bool m_Connected { get; private set; }
    private RTClientUpdater m_clientStreamer;

    [SerializeField] protected GameObject m_RigidBodyPrefab;
    [SerializeField] protected GameObject m_HorsePrefab;
    [SerializeField] protected GameObject m_PeoplePrefab;
    [SerializeField] protected GameObject m_ForcePlatePrefab;

    protected Dictionary<String, GameObject> m_QTMRigidBodies = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_QTMPeople = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_QTMHorses = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_QTMForcePlates = new Dictionary<string, GameObject>();

    public void SetConnection(string ip = "127.0.0.1", short port = -1)
    {
        StartCoroutine(Connect(ip, port));
    }


    #region Public

    public void CreateRigidBody(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMRigidBodies.ContainsKey(objectName))
        {
            Debug.Log("Rigidbody: " + name + " Already Exist");
            return;
        }

        RPC_CreateDeleteRigidBody(objectName, true);

    }

    public void CreatePeople(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMPeople.ContainsKey(objectName))
        {
            Debug.Log("People: " + name + " Already Exist");
            return;
        }

        RPC_CreateDeletePeople(objectName, true);

    }

    public void CreateHorse(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMHorses.ContainsKey(objectName))
        {
            Debug.Log("Horse: " + name + " Already Exist");
            return;
        }

        RPC_CreateDeleteHorse(objectName, true);

    }

    public void CreateForcePlate(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMForcePlates.ContainsKey(objectName))
        {
            Debug.Log("ForcePlate: " + name + " Already Exist");
            return;
        }

        RPC_CreateDeleteForcePlate(objectName, true);

    }

    public void DeleteRigidBody(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMRigidBodies.ContainsKey(objectName))
        {
            RPC_CreateDeleteRigidBody(objectName, false);
            return;
        }

        Debug.Log("Rigidbody: " + name + " Doesn't Exist");


    }

    public void DeletePeople(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMPeople.ContainsKey(objectName))
        {
            RPC_CreateDeletePeople(objectName, false);

            return;
        }

        Debug.Log("People: " + name + " Doesn't Exist");

    }

    public void DeleteHorse(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMHorses.ContainsKey(objectName))
        {
            RPC_CreateDeleteHorse(objectName, true);

            return;
        }

        Debug.Log("Horse: " + name + " Doesn't Exist");


    }

    public void DeleteForcePlate(string objectName)
    {
        if (!m_Connected) return;

        if (m_QTMForcePlates.ContainsKey(objectName))
        {
            RPC_CreateDeleteForcePlate(objectName, false);
            return;
        }

        Debug.Log("ForcePlate: " + name + " Doesn't Exist");


    }

    #endregion

    #region Private

    public void SetupObject(GameObject obj,  string objectName)
    {
        if (gameObject.TryGetComponent<IQTMObjectInterface>(out  IQTMObjectInterface setup))
        {
            setup.SetupObject(objectName);
        }
    }




    #region RPC Calls

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CreateDeleteRigidBody(string objectName, bool create)
    {
        if (create)
        {
            GameObject rb = Instantiate(m_RigidBodyPrefab, Vector3.zero, Quaternion.identity);
            SetupObject(rb, objectName);
            rb.name = objectName;
            m_QTMRigidBodies.Add(objectName, rb);
        }
        else
        {
            if (m_QTMRigidBodies.ContainsKey(objectName))
            {
                GameObject del = m_QTMRigidBodies[objectName];
                m_QTMRigidBodies.Remove(objectName);
                Destroy(del);
            }
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CreateDeletePeople(string objectName, bool create)
    {
        if (create)
        {
            GameObject people = Instantiate(m_PeoplePrefab, Vector3.zero, Quaternion.identity);
            SetupObject(people, objectName);
            people.name = objectName;
            m_QTMPeople.Add(objectName, people);
        }
        else
        {
            if (m_QTMPeople.ContainsKey(objectName))
            {
                GameObject del = m_QTMRigidBodies[name];
                m_QTMPeople.Remove(objectName);
                Destroy(del);
            }
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CreateDeleteHorse(string objectName, bool create)
    {
        if (create)
        {
            GameObject horse = Instantiate(m_HorsePrefab, Vector3.zero, Quaternion.identity);
            SetupObject(horse, objectName);
            horse.name = objectName;
            m_QTMHorses.Add(objectName, horse);
        }
        else
        {
            if (m_QTMHorses.ContainsKey(objectName))
            {
                GameObject del = m_QTMHorses[objectName];
                m_QTMHorses.Remove(objectName);
                Destroy(del);
            }
        }
    }
    
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_CreateDeleteForcePlate(string objectName, bool create )
    {
        if (create)
        {
            GameObject forcePlate = Instantiate(m_ForcePlatePrefab, Vector3.zero, Quaternion.identity );
            SetupObject(forcePlate, objectName);
            forcePlate.name = objectName;
            m_QTMForcePlates.Add(objectName, forcePlate);
        }
        else
        {
            if (m_QTMForcePlates.ContainsKey(objectName))
            {
                GameObject del = m_QTMForcePlates[objectName];
                m_QTMForcePlates.Remove(objectName);
                Destroy(del);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_LateJoinSync(RpcInfo info = default)
    {
        foreach (string key in m_QTMRigidBodies.Keys)
        {
            GameObject rb = Instantiate(m_QTMRigidBodies[key], Vector3.zero, Quaternion.identity);
          
        }
        foreach (string key in m_QTMPeople.Keys)
        {
            GameObject people = Instantiate(m_QTMPeople[key], Vector3.zero, Quaternion.identity);
          
        }
        foreach (string key in m_QTMHorses.Keys)
        {
            GameObject horse = Instantiate(m_QTMHorses[key], Vector3.zero, Quaternion.identity);
          
        }
        foreach (string key in m_QTMForcePlates.Keys)
        {
            GameObject forcePlate = Instantiate(m_QTMForcePlates[key], Vector3.zero, Quaternion.identity);
          
        }
    }
    
    #endregion
    
    #endregion
    IEnumerator Connect(string ip , short port)
    {
        RTClient.GetInstance().StartConnecting(ip, port, true, true, false, true, false, true, true);
        
        
        while (RTClient.GetInstance().ConnectionState == RTConnectionState.Connecting) 
        {
            yield return null;
        }
        
        m_Connected = RTClient.GetInstance().ConnectionState == RTConnectionState.Connected;
    }
    
}
