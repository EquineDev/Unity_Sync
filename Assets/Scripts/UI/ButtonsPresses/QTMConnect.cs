
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class QTMConnect : MonoBehaviour
{
    private TMP_InputField m_inputField;
    private bool m_tryingToConnect = false;
    private void Awake()
    {
        m_inputField = GetComponent<TMP_InputField>();
    }

    private void OnEnable()
    {
        ConnectionManager.Instance.QTMManager.ConnectAction += HasFinishedTryingToConnect;
    }

    private void OnDisable()
    {
        ConnectionManager.Instance.QTMManager.ConnectAction -= HasFinishedTryingToConnect;
    }

    #region public
    
    public void TryToconnectToQTM()
    {
        if (ConnectionManager.Instance.QTMManager.m_Connected || m_tryingToConnect)
            return;
        ConnectionManager.Instance.QTMManager.SetConnection(m_inputField.text);
        m_tryingToConnect = true; 
    }
    #endregion
    
    #region private

    private void HasFinishedTryingToConnect(bool Value)
    {
        m_tryingToConnect = false;
    }

    #endregion
}
