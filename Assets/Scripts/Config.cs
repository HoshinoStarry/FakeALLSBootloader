using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitConfig
{
    public string modelName { get; set; } = "ALLS HX";
    public string logoPath { get; set; } = "";
    public string batchFilePath { get; set; } = "";
    public List<Step> steps { get; set; } = new List<Step>()
    {
        new Step()
        {
            name = "STEP1",
            describe = "起動しています",
            awaitTime = 5
        },
        new Step()
        {
            name = "STEP4",
            describe = "ネットワークの設定をしています",
            awaitTime = 3
        },
        new Step()
        {
            name = "STEP10",
            describe = "インストールメディアを接続してください",
            awaitTime = 0.5f
        },
        new Step()
        {
            name = "STEP30",
            describe = "まもなくゲームプログラムが起動します",
            awaitTime = -1,
            isBootGame = true
        }
    };
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
            File.WriteAllText(filePath, JsonUtility.ToJson(cfg, true));
            return cfg;
        }
        
        var json = File.ReadAllText(filePath);
        // 检查文件是否为空或只包含空白字符
        if (string.IsNullOrWhiteSpace(json))
        {
            var cfg = new InitConfig();
            File.WriteAllText(filePath, JsonUtility.ToJson(cfg, true));
            return cfg;
        }
        
        try 
        {
            return JsonUtility.FromJson<InitConfig>(json);
        }
        catch (ArgumentException)
        {
            // 如果JSON解析失败，创建一个新的配置文件
            var cfg = new InitConfig();
            File.WriteAllText(filePath, JsonUtility.ToJson(cfg, true));
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