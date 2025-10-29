using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Error : MonoBehaviour
{
    public Text errorText;

    public Text errorMessageText;

    void Update()
    {
        if (!gameObject.activeSelf) return;

        var alpha = 0.5f + Mathf.PingPong(Time.time * 0.5f, 0.5f);
        errorText.color = new Color(errorText.color.r, errorText.color.g, errorText.color.b, alpha);
        errorMessageText.color = new Color(errorMessageText.color.r, errorMessageText.color.g, errorMessageText.color.b, alpha);
    }

    public void SetError(string errorId, string errorMsg="")
    {
        errorText.text = errorId.Replace("_", " ");
        if (!string.IsNullOrWhiteSpace(errorMsg))
        {
            errorMessageText.text = errorMsg;
            return;
        }
        var message = Config.InitConfig.i18n.GetValueOrDefault($"{errorId}_MESSAGE","");
        var workaround = Config.InitConfig.i18n.GetValueOrDefault($"{errorId}_WORKAROUND","");
        errorMessageText.text = $"{message}\n{workaround}";
    }
}
