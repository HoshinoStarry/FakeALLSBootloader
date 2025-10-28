using System;
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
        var num = processText.preferredWidth / 2f + 20f;
        loadingImage.rectTransform.localPosition = new Vector3(0f - num, -51f, 0f);
        loadingImage.rectTransform.Rotate(new Vector3(0f, 0f, -3f));
    }

    public System.Collections.IEnumerator DisplayTextWithDelay()
    {
        var cfg = Config.InitConfig;
        foreach (var step in cfg.steps)
        {
            stepText.text = step.name;
            processText.text = step.describe;
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
                    errorComponent.SetError("ERROR 0022", "ゲームプログラムが見つかりません\nインストールメディア（DVD など）を使用してゲームプログラムを再度インストールしてください。");
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
    }
}