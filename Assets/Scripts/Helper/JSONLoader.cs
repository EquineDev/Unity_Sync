using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONLoader 
{
    #region public
    
    public static bool Load<T>(string fileName, ref T jsonData)
    {
        string file = Path.Combine(Application.streamingAssetsPath, fileName);
           
        if (File.Exists(file))
        {

            string dataAsJson = File.ReadAllText(file);
            jsonData = JsonUtility.FromJson<T>(dataAsJson);
            return true;
        }

        return false;
    }
    
    
    public static void Write<T>(string fileName,  T jsonData)
    {
        string data = JsonUtility.ToJson(jsonData, true);
        string file = Path.Combine(Application.streamingAssetsPath, fileName);
        File.WriteAllText(file, data);
    }
    
    #endregion
}
