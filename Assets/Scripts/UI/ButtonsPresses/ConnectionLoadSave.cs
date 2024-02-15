using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionLoadSave : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_inputFieldQualisys;
    [SerializeField]
    private TMP_InputField m_inputFieldBCI;

    #region Public

    public void Load()
    {
        ConnectionDataManager.LoadData();
        m_inputFieldQualisys.text = ConnectionDataManager.ConnectionData.qualysisConfig.Adresss;
        m_inputFieldBCI.text = ConnectionDataManager.ConnectionData.bciConfig.Adresss;
    }

    public void Save()
    {
        ConnectionData data; 
        data.qualysisConfig = new QualisysConfig(m_inputFieldQualisys.text);
        data.bciConfig = new BCIConfig( m_inputFieldBCI.text);
        ConnectionDataManager.ConnectionData = data;
        ConnectionDataManager.SaveData();
    }
    

    #endregion
}
