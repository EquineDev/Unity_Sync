using brainflow;
using UnityEngine;


[System.Serializable]
public struct BCIData
{
    public string logFilePath;
    public string dataFilePath;
    public int streamChunkSize;
    public BoardIds boardId;
}
