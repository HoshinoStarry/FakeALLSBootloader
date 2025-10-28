using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class Error : MonoBehaviour
{
    public Text errorText;

    public Text errorMessageText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检查当前对象是否被激活
        if (!gameObject.activeSelf) return;

        // errorText 和 errorMessageText 透明度闪烁 
        var alpha = 0.8f + Mathf.PingPong(Time.time * 0.3f, 0.2f);
        errorText.color = new Color(errorText.color.r, errorText.color.g, errorText.color.b, alpha);
        errorMessageText.color = new Color(errorMessageText.color.r, errorMessageText.color.g, errorMessageText.color.b, alpha);
    }

    public void SetError(string errorId, string errorMsg)
    {
        // 0x0009629C: ERROR_0022 "ゲームプログラムが見つかりません"
        // 0x000962CE: ERROR_0022"インストールメディア（DVD など）を使用してゲームプログラムを再度インストールしてください。" 
        errorText.text = errorId;
        errorMessageText.text = errorMsg;
    }
}
