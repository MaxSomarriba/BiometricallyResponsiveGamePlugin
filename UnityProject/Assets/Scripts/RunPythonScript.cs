using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class RunPythonScript : MonoBehaviour
{
    // Path to the Python executable

    void Start()
    {
        string scriptPath = "C:/Users/maxms/Documents/BiometricallyResponsiveGamePlugin/PythonPairing/simulator.py";  // Replace with your Python script path
        string scriptArguments = "";  // Optional: pass arguments if needed
        RunPython(scriptPath, scriptArguments);
        UnityEngine.Debug.Log("Python script executed");
    }

    private void RunPython(string scriptPath, string arguments = "")
    {
        try
        {
            // Create a new process to run Python
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "python";  // If Python is not in PATH, you can provide full path like @"C:\Python39\python.exe"
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
                // Console.WriteLine("Output from Python script:");
                // Console.WriteLine(result);
                UnityEngine.Debug.Log("Output from Python script:");
                UnityEngine.Debug.Log(result);
                // Display any errors
                if (!string.IsNullOrEmpty(errors))
                {
                    // Console.WriteLine("Errors:");
                    // Console.WriteLine(errors);
                    UnityEngine.Debug.Log("Errors:");
                    UnityEngine.Debug.Log(errors);
                }
            }
        }
        catch (Exception e)
        {
            // Console.WriteLine($"An error occurred: {e.Message}");
            UnityEngine.Debug.Log($"An error occurred: {e.Message}");
        }
    }
}
