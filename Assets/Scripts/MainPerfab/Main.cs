using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Text modelText;
    
    public Image logoImage;
    
    public GameObject errorObj;
    
    public GameObject startupObj;

    void Start()
    {
        var cfg = Config.InitConfig;
        modelText.text = string.IsNullOrWhiteSpace(cfg.modelName) ? "ALLS HX" : cfg.modelName;
        var logoPath = cfg.logoPath;
        if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
        {
            var data = File.ReadAllBytes(logoPath);
            var texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(data);
            var sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            logoImage.sprite = sprite2;
        }
    }
    
    public void GotoError(string errorCode)
    {
        startupObj.SetActive(false);
        var errorComponent = errorObj.GetComponent<Error>();
        errorComponent.SetError(errorCode);
        errorObj.SetActive(true);
    }

    public void UpdateStartupText(string step, string process)
    {
        var startup = startupObj.GetComponent<Startup>();
        startup.UpdateTexts(step, process);
    }
}
