using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using UnityEditor;

public class DetectBluetoothDevicesScript : MonoBehaviour
{
    
    public static DetectBluetoothDevicesScript it;
    public DetectBluetoothDevicesScript(){
        it = this;
    }

    [MenuItem("Tools/Detect Bluetooth Devices")]
    private static void x(){
        it = new DetectBluetoothDevicesScript();
        it.StartScript();
    }

    
    public void StartScript()
    {
        string scriptArguments = "";
        string scriptPath = "C:/Users/maxms/Documents/BiometricallyResponsiveGamePlugin/PythonPairing/discoverBluetoothDeviceAddr.py";  
        RunPythonAsync(scriptPath, scriptArguments);
        UnityEngine.Debug.Log("script started");
    }

    private async void RunPythonAsync(string scriptPath, string arguments = "")
    {
        try
        {
            // Run the Python script asynchronously
            await Task.Run(() => RunPython(scriptPath, arguments));
            UnityEngine.Debug.Log("script finished");
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
                UnityEngine.Debug.Log(result);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log($"An error occurred: {e.Message}");
        }
    }
}
