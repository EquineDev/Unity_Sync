using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region public
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    public void AdditiveLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
  
    public void AdditiveLoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
  
    public void RemoveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.IsValid())
        {
            SceneManager.UnloadSceneAsync(scene);
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " not found or already unloaded.");
        }
    }
    #endregion
}
