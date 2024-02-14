
using UnityEngine;

public static class ConnectionDataManager
{
    
    private static string fileName = "connectionData.json";
    private static ConnectionData m_connectionData;
    
    public static ConnectionData ConnectionData
    {
        get { return m_connectionData; }
        set { m_connectionData = value; }
    }
    
    public static void LoadData()
    {
        if (JSONLoader.Load(fileName, ref m_connectionData))
        {
          
            Debug.Log("Data loaded");
        }
        else
        {
            Debug.Log("File doesn't exist to load data.");
        }
    }

    public static void SaveData()
    {
        JSONLoader.Write(fileName, m_connectionData);
        Debug.Log("Data Saved");
    }
}
