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
    
    [NonSerialized] 
    private static Players playersObj;

    
    public void Start()
    {
        if (!gameObject.activeSelf) return;
        if (!playersObj) playersObj = GameObject.Find("Canvas/Players").GetComponent<Players>();
    }
	
    public void Update()
    {
        if (!gameObject.activeSelf) return;
        var num = processText.preferredWidth / 2f + 30f;
        loadingImage.rectTransform.localPosition = new Vector3(0f - num, -51f, 0f);
        loadingImage.rectTransform.Rotate(new Vector3(0f, 0f, -3f));
    }

    public void UpdateTexts(string step, string process)
    {
        stepText.text = step;
        processText.text = process;
    }
}