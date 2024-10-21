using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

public class RunPythonScript : MonoBehaviour
{
    void Start()
    {
        string scriptPath = "C:/Users/maxms/Documents/BiometricallyResponsiveGamePlugin/PythonPairing/simulator.py";  // Replace with your Python script path
        string scriptArguments = "";  // Optional: pass arguments if needed
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
