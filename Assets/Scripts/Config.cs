using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class InitConfig
{
    public string modelName { get; set; } = "ALLS HX";
    public string logoPath { get; set; } = "";
    public string batchFilePath { get; set; } = "";
    public List<Step> steps { get; set; } = new List<Step>()
    {
        new Step()
        {
            name = "STEP_01",
            awaitTime = 5
        },
        new Step()
        {
            name = "STEP_04",
            awaitTime = 3
        },
        new Step()
        {
            name = "STEP_21",
            awaitTime = 0.5f
        },
        new Step()
        {
            name = "STEP_30",
            awaitTime = 5f,
            isBootGame = true
        }
    };
    public Dictionary<string, string> i18n { get; set; } = new Dictionary<string, string>();
}

public class Step
{
    public string name { get; set; } = "";
    public string describe { get; set; } = "";
    public float awaitTime { get; set; } = 0;
    public bool isBootGame { get; set; } = false;
}

public class Config
{ 
    public static InitConfig GetConfig()
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "init_config.json");
        if (!File.Exists(filePath))
        {
            var cfg = new InitConfig();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            return cfg;
        }
        
        var json = File.ReadAllText(filePath);
        // 检查文件是否为空或只包含空白字符
        if (string.IsNullOrWhiteSpace(json))
        {
            var cfg = new InitConfig();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            return cfg;
        }
        
        try 
        {
            return JsonConvert.DeserializeObject<InitConfig>(json);
        }
        catch (Exception ex)
        {
            // 如果JSON解析失败，创建一个新的配置文件
            Debug.LogException(ex);
            var cfg = new InitConfig();
            File.WriteAllText(filePath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            return cfg;
        }
    }

    private static InitConfig _config = null;
    public static InitConfig InitConfig
    {
        get
        {
            _config ??= GetConfig();
            return _config;
        }
    }
}