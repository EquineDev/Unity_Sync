using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private
    
    private string GetDatabasePath()
    {
    
        TextAsset config = Resources.Load<TextAsset>("DatabaseConfig");
        return config.text.Trim();
    }
    

    #endregion
}
