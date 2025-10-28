using UnityEngine;
using UnityEngine.UI; // 添加UI命名空间

public class Main : MonoBehaviour
{
    public Text modelText;

    void Start()
    {
        var cfg = Config.InitConfig;
        modelText.text = string.IsNullOrWhiteSpace(cfg.modelName) ? "ALLS HX" : cfg.modelName;
        
        var logoObj = GameObject.Find("Canvas/Main/LogoImage");
        if (logoObj is not null)
        {
            Debug.Log("have logo");
            var logoPath = cfg.logoPath;
            var imageComponent = logoObj.GetComponent<Image>();
            if (imageComponent is not null && !string.IsNullOrWhiteSpace(logoPath))
            {
                var data = System.IO.File.ReadAllBytes(logoPath);
                var texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(data);
                var sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                imageComponent.sprite = sprite2;
            }
        }
        if (Screen.width <= Screen.height)
        {
            Screen.SetResolution(1080, 1920, true);
            return;
        }
        
        Screen.SetResolution(1920, 1080, true);
        transform.localPosition = new Vector3(0, 0f, 0f);
        Camera.main.backgroundColor = new Color(255, 255, 255);
    }
}
