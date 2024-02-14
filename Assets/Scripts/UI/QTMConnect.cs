
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
        ConnectionManager.Instance.GetQTMManager().ConnectAction += HasFinishedTryingToConnect;
    }

    private void OnDisable()
    {
        ConnectionManager.Instance.GetQTMManager().ConnectAction -= HasFinishedTryingToConnect;
    }

    #region public
    
    public void TryToconnectToQTM()
    {
        if (ConnectionManager.Instance.GetQTMManager().m_Connected || m_tryingToConnect)
            return;
        ConnectionManager.Instance.GetQTMManager().SetConnection(m_inputField.text);
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
