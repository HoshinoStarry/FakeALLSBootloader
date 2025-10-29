using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
// 添加System.Threading.Tasks命名空间以支持异步操作
using System.Threading.Tasks;

public class Players : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    
    [NonSerialized]
    private InitConfig initConfig;

    [NonSerialized] private string batchFilePath;
    void Start()
    {
        initConfig = Config.InitConfig;
        batchFilePath = Path.GetFullPath(initConfig.batchFilePath);
        
        switch (initConfig.displayType)
        {
            case DisplayType.Landscape:
            {
                Screen.SetResolution(1920, 1080, true);
                player1.SetActive(true);
                player1.transform.localPosition = new Vector3(0, 0f, 0f);
                Camera.main.backgroundColor = Color.white;
                break;
            }
            case DisplayType.Portrait:
            {
                Screen.SetResolution(1080, 1920, true);
                player1.SetActive(true);
                player1.transform.localPosition = new Vector3(0, 0f, 0f);
                Camera.main.backgroundColor = Color.white;
                break;
            }
            case DisplayType.Maimai:
            {
                Screen.SetResolution(1080, 1920, true);
                player1.transform.localPosition = new Vector3(0f, -420f, 0f);
                player1.SetActive(true);
                break;
            }
            case DisplayType.MaimaiDual:
            { 
                Screen.SetResolution(2160, 1920, true);
                player1.SetActive(true);
                player2.SetActive(true);
                break;
            }
        }

        // 启动异步任务而不是协程
        _ = DisplayTextWithDelayAsync();
    }
    
    // 将协程方法改为异步方法
    public async Task DisplayTextWithDelayAsync()
    {
        foreach (var step in initConfig.steps)
        {
            var stepText = step.name.Replace("_0", " ").Replace("_", " ");
            var processText = string.IsNullOrWhiteSpace(step.describe) ? initConfig.i18n.GetValueOrDefault($"{step.name}_MESSAGE", "") : step.describe;
            UpdateStartupText(stepText, processText);
            switch (step.name)
            {
                case "STEP_21":
                {
                    await Task.Delay((int)(step.awaitTime * 1000));
#if !UNITY_EDITOR
                    if (!File.Exists(batchFilePath))
                    {
                        GotoError("ERROR_0089");
                        return;
                    }
#endif
                    break;
                }
                case "STEP_30":
                {
                    try
                    {
                        ProcessController.StartProcess(initConfig.batchFilePath);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        GotoError("ERROR_0010");
                        return;
                    }

                    var timeout = 30000; // 30 seconds timeout
                    var elapsed = 0;
                    while (!ProcessController.IsProcessRunning(initConfig.detectProcessName) && elapsed < timeout)
                    {
                        await Task.Delay(100);
                        elapsed += 100;
                    }

                    if (elapsed >= timeout)
                    {
                        Debug.LogError("Timeout waiting for process to start: " + initConfig.detectProcessName);
                        return;
                    }

                    QuitGame();
                    break;
                }
                default:
                {
                    await Task.Delay((int)(step.awaitTime * 1000));
                    break;
                }
            }
        }
    }

    public void GotoError(string errorCode)
    {
        foreach (var player in new[] {player1, player2})
        {
            var main = player.GetComponentInChildren<Main>();
            main!.GotoError(errorCode);
        }
    }

    public void UpdateStartupText(string step, string process)
    {
        foreach (var player in new[] {player1, player2})
        {
            var main = player.GetComponentInChildren<Main>();
            main!.UpdateStartupText(step, process);
        }
    }
    
    public void QuitGame()
    {
        Camera.main.backgroundColor = new Color(0, 0, 0);
        player1.SetActive(false);
        player2.SetActive(false);
        Application.Quit();
    }
}