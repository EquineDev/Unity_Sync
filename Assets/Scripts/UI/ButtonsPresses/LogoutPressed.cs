using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LogoutPressed : MonoBehaviour
{

    public UnityEvent<string> m_logoutSuccessful ;
    [SerializeField]
    private string m_statusUpdated = "Not Logged In";

    #region public

    public void Pressed()
    {

        DatabaseManager.IsLoggedIn = false;
        m_logoutSuccessful?.Invoke(m_statusUpdated);
        
    }
    
    #endregion
}

