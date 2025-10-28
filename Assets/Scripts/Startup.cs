using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Startup : MonoBehaviour
{
    public Text processText;

    public Text stepText;
    
    public RawImage loadingImage;
    
    public GameObject errorObj;
    
    public void Start()
    {
        StartCoroutine(DisplayTextWithDelay());
    }
	
    public void Update()
    {
        if (!gameObject.activeSelf) return;
        var num = processText.preferredWidth / 2f + 20f;
        loadingImage.rectTransform.localPosition = new Vector3(0f - num, -51f, 0f);
        loadingImage.rectTransform.Rotate(new Vector3(0f, 0f, -3f));
    }

    public System.Collections.IEnumerator DisplayTextWithDelay()
    {
        var cfg = Config.InitConfig;
        foreach (var step in cfg.steps)
        {
            stepText.text = step.name.Replace("_0", " ").Replace("_", " ");
            processText.text = string.IsNullOrWhiteSpace(step.describe) ? cfg.i18n.GetValueOrDefault($"{step.name}_MESSAGE", "") : step.describe;
            if (step.isBootGame)
            {
                try
                {
                    StartGame(cfg.batchFilePath);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    
                    gameObject.SetActive(false);
                    var errorComponent = errorObj.GetComponent<Error>();
                    errorComponent.SetError("ERROR_0022");
                    errorObj.SetActive(true);
                }
            }
            if (step.awaitTime > 0)
                yield return new WaitForSeconds(step.awaitTime);
        }
    }
	
    public void StartGame(string batchFilePath)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo(batchFilePath)
        {
            UseShellExecute = true,
            CreateNoWindow = false
        };
        var process = new System.Diagnostics.Process();
        process.StartInfo = startInfo;
        process.Start();
        
        // 最小化当前游戏窗口
        // MinimizeWindow();
        Application.Quit();
    }
    
    private void MinimizeWindow()
    {
        #if UNITY_STANDALONE_WIN
        try
        {
            var module = System.Diagnostics.Process.GetCurrentProcess().MainModule;
            if (module != null)
            {
                var hwnd = FindWindow(module.ModuleName, Application.productName);
                if (hwnd != System.IntPtr.Zero)
                {
                    ShowWindow(hwnd, SW_MINIMIZE);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to minimize window: " + ex.Message);
        }
        #endif
    }
    
    #if UNITY_STANDALONE_WIN
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);
    
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ShowWindow(System.IntPtr hWnd, int nCmdShow);
    
    private const int SW_MINIMIZE = 2;
    #endif
}