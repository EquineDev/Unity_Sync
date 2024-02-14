
using System.Reflection;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class UpdateConnectionStatus : MonoBehaviour
{
    
    protected TMP_Text m_displayText;

    #region protect
    
    protected void Awake()
    {
        m_displayText = GetComponent<TMP_Text>();
    }

    protected void TryingConnection()
    {
        m_displayText.text = "Connecting...";
        m_displayText.color = Color.yellow;
    }

    protected void ConnectionStatus(bool valid)
    {
        m_displayText.color = Color.black;
        if (valid)
        {
            m_displayText.text = "Connected";
        }
        else
        {
            m_displayText.text = "Disconnected";
        }
    }
    
    #endregion
}
