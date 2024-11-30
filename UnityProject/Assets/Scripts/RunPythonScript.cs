using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

public class RunPythonScript : MonoBehaviour
{
    public static RunPythonScript Instance { get; private set; }
    public event EventHandler OnHeartRateTooHigh;
    public event EventHandler OnHeartRateSpike;
    string scriptPath = "C:/Users/maxms/Documents/BiometricallyResponsiveGamePlugin/PythonPairing/main.py";  // Replace with your Python script path
    // string scriptArguments = "";  // Optional: pass arguments if needed
    [SerializeField] private string deviceAddress = "E6:7F:A6:74:4F:F0";
    [SerializeField] private int heartbeatThreshold = 83;
    [SerializeField] private bool lookForSpikes = false;
    [SerializeField] private int spikeThreshold = 10;

    private void Awake()  {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    public void StartScript()
    {
        string scriptArguments = $"{deviceAddress} {heartbeatThreshold} {lookForSpikes} {spikeThreshold}";
        RunPythonAsync(scriptPath, scriptArguments);
        UnityEngine.Debug.Log("Python script started");
    }

    private async void RunPythonAsync(string scriptPath, string arguments = "")
    {
        try
        {
            // Run the Python script asynchronously
            await Task.Run(() => RunPython(scriptPath, arguments));
            UnityEngine.Debug.Log("Python script finished");

            // Rerun
            if(Application.isPlaying){
                StartScript();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"An error occurred: {e.Message}");
        }
    }

    private void RunPython(string scriptPath, string arguments = "")
    {
        try
        {
            // Create a new process to run Python
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "python";  // If Python is not in PATH, you can provide the full path like @"C:\Python39\python.exe"
            psi.Arguments = $"{scriptPath} {arguments}";  // Arguments can be passed to the script if needed
            psi.RedirectStandardOutput = true;  // To capture output
            psi.RedirectStandardError = true;   // To capture errors
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            // Start the process
            using (Process process = Process.Start(psi))
            {
                // Read output from the script
                string result = process.StandardOutput.ReadToEnd();
                string errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Display the output
                UnityEngine.Debug.Log("Output from Python script:");
                UnityEngine.Debug.Log(result);

                // Check if heart rate exceeds threshold
                string checkStringSpike = "Heart rate spike detected";
                string checkStringRate = "Heart rate is too high";
                if (result.Contains(checkStringSpike)){
                    UnityEngine.Debug.Log("WOOOAH heart rate spiked");
                    OnHeartRateSpike?.Invoke(this, EventArgs.Empty);
                }else if (result.Contains(checkStringRate)){
                    UnityEngine.Debug.Log("BRUHHH heart rate was too high");
                    OnHeartRateTooHigh?.Invoke(this, EventArgs.Empty);
                }

                // Display any errors
                if (!string.IsNullOrEmpty(errors))
                {
                    UnityEngine.Debug.Log("Errors:");
                    UnityEngine.Debug.Log(errors);
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"An error occurred: {e.Message}");
        }
    }
}
