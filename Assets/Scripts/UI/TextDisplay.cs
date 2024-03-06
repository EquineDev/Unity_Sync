using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TextDisplay : MonoBehaviour
{
    [SerializeField]
    private TextDataScriptableObject m_textData; // Reference to the ScriptableObject containing text data

    [SerializeField] private bool m_displayAtStart = false;
    private TMP_Text m_displayText; // Reference to the TextMeshPro component to display the text data
    
    
    void Start()
    {
        m_displayText = GetComponent<TMP_Text>();
        if (!m_displayAtStart)
            return;
        if (m_textData != null)
        {
            if (m_displayText != null)
            {
                m_displayText.text = m_textData.TextData.text;
                m_displayText.color = m_textData.TextData.color;
                m_displayText.font = m_textData.TextData.fontAsset;
                m_displayText.fontSize = m_textData.TextData.fontSize;     
            }
            else
            {
                Debug.LogError("TextMeshPro Text component reference is null!");
            }
        }
        else
        {
            Debug.LogError("TextDataScriptableObject reference is null!");
        }
    }

    #region public

    public string GetTextName()
    {
        return m_displayText.text;
    }
    public void UpdateText(string displayName)
    {
        m_displayText.name = displayName;
    }
    public void ToggleText(bool enable)
    {
        m_displayText.enabled = enable;
    }
    #endregion
}
