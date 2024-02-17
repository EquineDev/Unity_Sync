using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class TrackerManager<T> : NetworkObject where T : Component
{
    public bool m_Connected { get; protected set; }
    public Action TryingConnection;
    public Action<bool> ConnectAction;

    [SerializeField] 
    protected GameObject m_RigidBodyPrefab;
    [SerializeField] 
    protected GameObject m_HorsePrefab;
    [SerializeField] 
    protected GameObject m_PeoplePrefab;
    [SerializeField] 
    protected GameObject m_ForcePlatePrefab;

    protected Dictionary<String, GameObject> m_rigidBodies = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_people = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_horses = new Dictionary<string, GameObject>();
    protected Dictionary<String, GameObject> m_forcePlates = new Dictionary<string, GameObject>();

    #region Public

    public virtual void SetConnection(string ip = "127.0.0.1", short port = -1)
    {
        
    }

    public virtual void CreateRigidBody(string objectName)
    {
        if (!m_Connected) return;

        if (m_rigidBodies.ContainsKey(objectName))
        {
            Debug.Log("Rigidbody: " + objectName + " Already Exist");
            return;
        }

        RPC_CreateDeleteRigidBody(objectName, true);
    }

    public virtual void CreatePeople(string objectName)
    {
        if (!m_Connected) return;

        if (m_people.ContainsKey(objectName))
        {
            Debug.Log("People: " + objectName + " Already Exist");
            return;
        }

        RPC_CreateDeletePeople(objectName, true);
    }

    public virtual void CreateHorse(string objectName)
    {
        if (!m_Connected) return;

        if (m_horses.ContainsKey(objectName))
        {
            Debug.Log("Horse: " + objectName + " Already Exist");
            return;
        }

        RPC_CreateDeleteHorse(objectName, true);
    }

    public virtual void CreateForcePlate(string objectName)
    {
        if (!m_Connected) return;

        if (m_forcePlates.ContainsKey(objectName))
        {
            Debug.Log("ForcePlate: " + objectName + " Already Exist");
            return;
        }

        RPC_CreateDeleteForcePlate(objectName, true);
    }

    public virtual void DeleteRigidBody(string objectName)
    {
        if (!m_Connected) return;

        if (m_rigidBodies.ContainsKey(objectName))
        {
            RPC_CreateDeleteRigidBody(objectName, false);
            return;
        }

        Debug.Log("Rigidbody: " + objectName + " Doesn't Exist");
    }

    public virtual void DeletePeople(string objectName)
    {
        if (!m_Connected) return;

        if (m_people.ContainsKey(objectName))
        {
            RPC_CreateDeletePeople(objectName, false);

            return;
        }

        Debug.Log("People: " + objectName + " Doesn't Exist");
    }

    public virtual void DeleteHorse(string objectName)
    {
        if (!m_Connected) return;

        if (m_horses.ContainsKey(objectName))
        {
            RPC_CreateDeleteHorse(objectName, true);

            return;
        }

        Debug.Log("Horse: " + objectName + " Doesn't Exist");
    }

    public virtual void DeleteForcePlate(string objectName)
    {
        if (!m_Connected) return;

        if (m_forcePlates.ContainsKey(objectName))
        {
            RPC_CreateDeleteForcePlate(objectName, false);
            return;
        }

        Debug.Log("ForcePlate: " + objectName + " Doesn't Exist");
    }

    #endregion

    protected virtual void SetupObject(GameObject obj, string objectName)
    {
        
    }

    #region RPC Calls

    [Rpc(RpcSources.All, RpcTargets.All)]
    protected virtual void RPC_CreateDeleteRigidBody(string objectName, bool create)
    {
        if (create)
        {
            GameObject rb = Instantiate(m_RigidBodyPrefab.gameObject, Vector3.zero, Quaternion.identity);
            SetupObject(rb, objectName);
            rb.name = objectName;
            m_rigidBodies.Add(objectName, rb);
        }
        else
        {
            if (m_rigidBodies.ContainsKey(objectName))
            {
                GameObject del = m_rigidBodies[objectName];
                m_rigidBodies.Remove(objectName);
                Destroy(del);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    protected virtual void RPC_CreateDeletePeople(string objectName, bool create)
    {
        if (create)
        {
            GameObject people = Instantiate(m_PeoplePrefab.gameObject, Vector3.zero, Quaternion.identity);
            SetupObject(people, objectName);
            people.name = objectName;
            m_people.Add(objectName, people);
        }
        else
        {
            if (m_people.ContainsKey(objectName))
            {
                GameObject del = m_rigidBodies[name];
                m_people.Remove(objectName);
                Destroy(del);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    protected virtual void RPC_CreateDeleteHorse(string objectName, bool create)
    {
        if (create)
        {
            GameObject horse = Instantiate(m_HorsePrefab.gameObject, Vector3.zero, Quaternion.identity);
            SetupObject(horse, objectName);
            horse.name = objectName;
            m_horses.Add(objectName, horse);
        }
        else
        {
            if (m_horses.ContainsKey(objectName))
            {
                GameObject del = m_horses[objectName];
                m_horses.Remove(objectName);
                Destroy(del);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    protected virtual void RPC_CreateDeleteForcePlate(string objectName, bool create)
    {
        if (create)
        {
            GameObject forcePlate = Instantiate(m_ForcePlatePrefab.gameObject, Vector3.zero, Quaternion.identity);
            SetupObject(forcePlate, objectName);
            forcePlate.name = objectName;
            m_forcePlates.Add(objectName, forcePlate);
        }
        else
        {
            if (m_forcePlates.ContainsKey(objectName))
            {
                GameObject del = m_forcePlates[objectName];
                m_forcePlates.Remove(objectName);
                Destroy(del);
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public virtual void RPC_LateJoinSync(RpcInfo info = default)
    {
        foreach (string key in m_rigidBodies.Keys)
        {
            GameObject rb = Instantiate(m_rigidBodies[key], Vector3.zero, Quaternion.identity);
        }

        foreach (string key in m_people.Keys)
        {
            GameObject people = Instantiate(m_people[key], Vector3.zero, Quaternion.identity);
        }

        foreach (string key in m_horses.Keys)
        {
            GameObject horse = Instantiate(m_horses[key], Vector3.zero, Quaternion.identity);
        }

        foreach (string key in m_forcePlates.Keys)
        {
            GameObject forcePlate = Instantiate(m_forcePlates[key], Vector3.zero, Quaternion.identity);
        }
    }

    #endregion
}
