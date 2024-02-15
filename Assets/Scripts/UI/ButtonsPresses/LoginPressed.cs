using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LoginPressed : MonoBehaviour
{
    public UnityEvent<string> m_loginSuccessful ; 
    [SerializeField]
    private TMP_InputField m_userNameField;
    [SerializeField]
    private TMP_InputField m_passwordField;
    [SerializeField] 
    private TMP_Text m_errorField;
    private void OnEnable()
    {
        m_errorField.text = " ";
    }
    #region public

    public void Pressed()
    {
        m_errorField.text = " ";
        if (IsValidInput(m_userNameField.text, m_passwordField.text))
        {
            if (DatabaseManager.Login(m_userNameField.text,  m_passwordField.text))
            {
                m_loginSuccessful?.Invoke(m_userNameField.text);
                
            }
            else
            {
                m_errorField.text = "Bad Password or Username";
            }
        }
        else
        {
            m_errorField.text = "Missing Password or Username";
        }
    }
    #endregion
    
    private bool IsValidInput(string name, string password)
    {
        return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password);
    }
}
