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
    
    public GameObject mainObj;

    [NonSerialized] private bool started = false;
    
    public void Start()
    {
        if (!gameObject.activeSelf) return;
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
                    GotoError("ERROR_0010");
                    yield break;
                }
                yield return new WaitForSeconds(step.awaitTime);
                QuitGame();
            }
            if (step.awaitTime > 0)
                yield return new WaitForSeconds(step.awaitTime);
        }
    }
	
    public void StartGame(string batchFilePath)
    {
        if (started) return;
        var startInfo = new System.Diagnostics.ProcessStartInfo(batchFilePath)
        {
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = new System.Diagnostics.Process();
        process.StartInfo = startInfo;
        process.Start();
        started = true;
    }
    
    private void GotoError(string errorCode)
    {
        gameObject.SetActive(false);
        var errorComponent = errorObj.GetComponent<Error>();
        errorComponent.SetError(errorCode);
        errorObj.SetActive(true);
    }
    
    public void QuitGame()
    {
        Camera.main.backgroundColor = new Color(0, 0, 0);
        mainObj.SetActive(false);
        gameObject.SetActive(false);
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