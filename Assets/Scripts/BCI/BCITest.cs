using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using brainflow;
using brainflow.math;


public class BCITest : MonoBehaviour
{
    
    private BoardShim board_shim = null;
    private MLModel concentration = null;
    private int sampling_rate = 0;
    private int[] eeg_channels = null;

    private BoardShim boardShim;
    private int samplingRate;
    
    private void Start()
    {
        try
        {
            BoardShim.set_log_file("brainflow_log.txt");
            BoardShim.enable_dev_board_logger();

            BrainFlowInputParams input_params = new BrainFlowInputParams();
            int board_id = (int)BoardIds.SYNTHETIC_BOARD;
            board_shim = new BoardShim(board_id, input_params);
            board_shim.prepare_session();
            board_shim.start_stream(450000, "file://brainflow_data.csv:w");
            BrainFlowModelParams concentration_params = new BrainFlowModelParams((int)BrainFlowMetrics.RESTFULNESS, 0);
            concentration = new MLModel(concentration_params);
            concentration.prepare();

            sampling_rate = BoardShim.get_sampling_rate(board_id);
            eeg_channels = BoardShim.get_eeg_channels(board_id);
            Debug.Log("Brainflow streaming was started");
        }
        catch (BrainFlowError  e)
        {
            Debug.Log(e);
        }
    }
    
    void Update()
    {
        if ((board_shim == null) || (concentration == null))
        {
            return;
        }
        int number_of_data_points = sampling_rate * 4; // 4 second window is recommended for concentration and relaxation calculations
        double[,] data = board_shim.get_current_board_data(number_of_data_points);
        if (data.GetRow(0).Length < number_of_data_points)
        {
            // wait for more data
            return;
        }
        // prepare feature vector
        Tuple<double[], double[]> bands = DataFilter.get_avg_band_powers (data, eeg_channels, sampling_rate, true);
        double[] feature_vector = bands.Item1.Concatenate (bands.Item2);
        // calc and print concetration level
        // for synthetic board this value should be close to 1, because of sin waves ampls and freqs
        Debug.Log("Concentration: " + concentration.predict (feature_vector));
    }

    // You need to call release_session and ensure that all resources correctly released
    private void OnDestroy()
    {
        if (board_shim != null)
        {
            try
            {
                board_shim.release_session();
                concentration.release();
            }
            catch (BrainFlowError e)
            {
                Debug.Log(e);
            }
            Debug.Log("Brainflow streaming was stopped");
        }
    }
}

[Serializable]
public enum BrainFlowChannelType
{
    EEG,
    EXG,
    EMG,
    ECG,
    EOG,
    EDA,
    PPG,
    Accel,
    Analog,
    Gyro,
    Temperature,
    Resistance,
    Other
}

public enum VisualizationType
{
    Line,
    Bar
}
