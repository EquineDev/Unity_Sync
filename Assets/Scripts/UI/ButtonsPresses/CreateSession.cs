using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class CreateSession : MonoBehaviour
{
    [SerializeField] 
    private TMP_InputField m_nameField;

    [SerializeField] 
    private TMP_InputField m_passwordField;

    #region public
    
    public void SubmitSessionName()
    {
        if (IsValidInput(m_nameField.text, m_passwordField.text))
        {
            ConnectionManager.Instance.ConnectToLobby(m_nameField.text, SessionLobby.Custom);
        }
    
    }
    #endregion
    
    #region  private
    
    private bool IsValidInput(string name, string password)
    {
        return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password);
    }
    #endregion
}
