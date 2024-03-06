using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class JoinLobbyPassword : MonoBehaviour
{

    
    [SerializeField] 
    private TMP_InputField m_passwordField;
    [SerializeField] 
    private TextDisplay m_errorField;
    
    private string key;
    private string room; 
    #region public

    

    #endregion
    public void Setup(ref string session, ref string password)
    {
        key = password;
        room = session;
        m_errorField.UpdateText("Error Bad password");
        m_errorField.ToggleText(false);
    }


    public void JoinLobby()
    {
        if (IsValidInput(m_passwordField.text))
        {
            ConnectionManager.Instance.ConnectToLobby(room, SessionLobby.Custom);
        }
        else
        {
            m_errorField.ToggleText(true);
        }
    }
    #region private

    

 
    private bool IsValidInput(string name)
    {
        return string.Equals(key, name);
    }
    
    #endregion
}
