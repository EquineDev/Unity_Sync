using System;
using System.Collections;
using System.Collections.Generic;
using brainflow;
using UnityEngine;

public class BCIManager : MonoBehaviour
{
     private BoardShim boardShim;
    private int samplingRate;
    private bool isStreaming = false;

    [Header("Configuration")]
    public string logFilePath = "brainflow_log.txt";
    public string dataFilePath = "brainflow_data.csv";
    public int streamChunkSize = 450000;
    public BoardIds boardId = BoardIds.SYNTHETIC_BOARD;

    void Start()
    {
        try
        {
            BoardShim.set_log_file(logFilePath);
            BoardShim.enable_dev_board_logger();
            int board_id = (int)BoardIds.SYNTHETIC_BOARD;
            var inputParams = new BrainFlowInputParams();
            boardShim = new BoardShim((int)boardId, inputParams);
            boardShim.prepare_session();
            samplingRate = BoardShim.get_sampling_rate(board_id);
            Debug.Log("BrainFlow session prepared");
        }
        catch (BrainFlowError e)
        {
            Debug.LogError("Error initializing BrainFlow: " + e);
        }
    }

    void Update()
    {
        // Check for user input to start/stop streaming
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleStream();
        }

        // If streaming, read data
        if (isStreaming)
        {
            ReadData();
        }
    }

    void ReadData()
    {
        try
        {
            var numberOfDataPoints = samplingRate * 4; // Read data for 4 seconds
            var data = boardShim.get_current_board_data(numberOfDataPoints);
            // Process or visualize the data here
        }
        catch (BrainFlowError e)
        {
            Debug.LogError("Error reading data from BrainFlow: " + e);
        }
    }

    void ToggleStream()
    {
        if (!isStreaming)
        {
            try
            {
                string fullDataPath = Application.persistentDataPath + "/" + dataFilePath;
                boardShim.start_stream(streamChunkSize, fullDataPath);
                isStreaming = true;
                Debug.Log("BrainFlow streaming started");
            }
            catch (BrainFlowError e)
            {
                Debug.LogError("Error starting BrainFlow stream: " + e);
            }
        }
        else
        {
            try
            {
                boardShim.stop_stream();
                isStreaming = false;
                Debug.Log("BrainFlow streaming stopped");
            }
            catch (BrainFlowError e)
            {
                Debug.LogError("Error stopping BrainFlow stream: " + e);
            }
        }
    }

    void OnDestroy()
    {
        if (boardShim != null)
        {
            try
            {
                boardShim.release_session();
                Debug.Log("BrainFlow session released");
            }
            catch (BrainFlowError e)
            {
                Debug.LogError("Error releasing BrainFlow session: " + e);
            }
        }
    }
}

