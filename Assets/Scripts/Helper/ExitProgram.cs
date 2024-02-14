
using UnityEngine;

public class ExitProgram : MonoBehaviour
{
    #region public

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
    
    #endregion
}
