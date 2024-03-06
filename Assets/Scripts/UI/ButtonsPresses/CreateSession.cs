using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class CreateSession : MonoBehaviour
{
    [SerializeField] 
    private TMP_InputField m_nameField;
    
    public void SubmitSessionName()
    {
        ConnectionManager.Instance.ConnectToLobby(m_nameField.text, SessionLobby.Custom);
    }
}
