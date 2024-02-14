
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAction : MonoBehaviour
{
    public UnityEvent m_onPress; 

    private Button m_button; 

    void Start()
    {
      
        m_button = GetComponent<Button>();
        
        if (m_button != null)
        {
            m_button.onClick.AddListener(() => m_onPress.Invoke());
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }
}
