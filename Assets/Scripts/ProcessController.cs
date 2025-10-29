using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ProcessController
{
    private static string _lastBatchFilePath = "";

    public static void StartProcess(string batchFilePath)
    {
        if (_lastBatchFilePath.Equals(batchFilePath))
        {
            Debug.Log("Same batch file path, skipping.");
            return;
        }
        _lastBatchFilePath = batchFilePath;
        
        var startInfo = new System.Diagnostics.ProcessStartInfo(batchFilePath)
        {
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = new System.Diagnostics.Process();
        process.StartInfo = startInfo;
        process.Start();
    }

    public static bool IsProcessRunning(string processName)
    {
        try
        {
            var processes = Process.GetProcesses();
            return processes.Any(p => p.ProcessName.ToLower().Equals(processName.ToLower()));
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Cannot process request because the process")) return true;
            Debug.LogError($"Error checking process existence: \n{ex}");
            return false;
        }
    }
}