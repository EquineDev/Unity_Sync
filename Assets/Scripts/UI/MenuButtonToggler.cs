using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButtonToggler : MonoBehaviour
{

    [SerializeField] 
    private GameObject m_menuMain;
    [SerializeField]
    private string m_menuName; 
    [SerializeField] 
    private GameObject m_menuSecondary;
    [SerializeField]
    private string m_menuSecondaryName; 
    [SerializeField] 
    private TMP_Text m_textButton;

    #region public

    public void Pressed()
    {
        if (DatabaseManager.IsLoggedIn)
        {
            m_menuSecondary.SetActive(true);
            m_textButton.text = m_menuSecondaryName;
        }
        else
        {
            m_menuMain.SetActive(true);
            m_textButton.text = m_menuName;
        }
    }

    #endregion
}
