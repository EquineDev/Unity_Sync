using System;

[Serializable]
public struct  ConnectionData
{
    public QualisysConfig qualysisConfig;
    public BCIConfig bciConfig;
}

[Serializable]
public struct QualisysConfig
{
    public QualisysConfig(string IP)
    {
        Adresss = IP;
    }
    public string Adresss;
}
[Serializable]
public struct BCIConfig
{
    public BCIConfig(string IP)
    {
        Adresss = IP;
    }
    public string Adresss;
}