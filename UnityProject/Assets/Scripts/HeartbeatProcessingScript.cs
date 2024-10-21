using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class HeartbeatProcessingScript : MonoBehaviour
{
    private string csvFilePath = "C:/Users/maxms/Documents/BiometricallyResponsiveGamePlugin/heartbeat_data.csv";

    void Start()
    {
        StartCoroutine(ReadHeartbeatData());
    }

    private IEnumerator ReadHeartbeatData()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f); // Check every second
            ReadCSV();
        }
    }

    private void ReadCSV()
    {
        if (File.Exists(csvFilePath))
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            if (lines.Length > 1) // Check if there's data (first line is header)
            {
                string[] latestData = lines[lines.Length - 1].Split(','); // Get the last line
                string timestamp = latestData[0];
                string heartRateStr = latestData[1];

                if (float.TryParse(heartRateStr, out float heartRate))
                {
                    Debug.Log($"Latest heart rate: {heartRate} bpm at {timestamp}");
                    // Add your threshold logic here
                }
            }
        }
    }

}
