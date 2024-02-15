using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CreateUserPressed : MonoBehaviour
{
    
    public UnityEvent m_creationSuccessful ; 
    [SerializeField]
    private TMP_InputField m_nameInputField;
    [SerializeField]
    private TMP_InputField m_emailInputField;
    [SerializeField]
    private TMP_InputField m_institutionInputField;
    [SerializeField]
    private TMP_InputField m_roleInputField;
    [SerializeField]
    private TMP_InputField m_passwordInputField;
    [SerializeField]
    private TMP_Text m_errorLog; 
    
    
    private void OnEnable()
    {
        m_errorLog.text = " ";
    }
    
    #region Public

    public void SignupUserPressed()
    {
        string name = m_nameInputField.text;
        string email = m_emailInputField.text;
        string institution = m_institutionInputField.text;
        string role = m_roleInputField.text;
        string password = m_passwordInputField.text;
        if (IsValidInput(name, email, institution, role, password))
        {
            if (DatabaseManager.InsertResearchAccount(name, email, institution, role, password) != -1)
            {
                m_creationSuccessful?.Invoke();
            }
            else
            {
                m_errorLog.text = "User has already signed up with email Address";
                return;
            }
        }
        else
        {
            m_errorLog.text = "Invalid input. Please fill in all fields.";
         
        }
    }
    
    #endregion

    #region private
    
    private bool IsValidInput(string name, string email, string institution, string role, string password)
    {
       
        return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email) &&
               !string.IsNullOrEmpty(institution) && !string.IsNullOrEmpty(role) && string.IsNullOrEmpty(password);
    }
    
    #endregion
}
