using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class InitConfig
{
    public string modelName { get; set; } = "ALLS HX";
    public string logoPath { get; set; } = "";
    public string batchFilePath { get; set; } = "";
    public string detectProcessName { get; set; } = "Sinmai.exe";
    public DisplayType displayType { get; set; } = DisplayType.Landscape;
    public List<Step> steps { get; set; } = new List<Step>();
    public Dictionary<string, string> i18n { get; set; } = new Dictionary<string, string>();
}

public enum DisplayType
{
    Landscape,
    Portrait,
    Maimai,
    MaimaiDual,
}

public class Step
{
    public string name { get; set; } = "";
    public string describe { get; set; } = "";
    public float awaitTime { get; set; } = 0;
    public bool isBootGame { get; set; } = false;

    public override string ToString() => $"{name} - {describe} - {awaitTime}";
}

public class Config
{ 
    public static InitConfig GetConfig()
    {
        var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        var exeDirectory = Path.GetDirectoryName(exePath);
        var filePath = Path.Combine(exeDirectory, "init_config.json");
        
        var commandLine = Environment.CommandLine;
        var commandLineArgs = Environment.GetCommandLineArgs().ToList();
        var cfgIndex = commandLineArgs.FindIndex(x => x.StartsWith("/config:") || x.StartsWith("--config:") || x.StartsWith("-c:") || x.StartsWith("/c:"));
        if (cfgIndex >= 0)
            filePath = commandLineArgs[cfgIndex].Split(':')[1];
        
#if UNITY_EDITOR
        filePath = @"D:\FakeALLSBoot\FakeALLSBootloader\init_config.json";
#endif
        var cfg = new InitConfig
        {
            steps = new List<Step>()
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
            }
        };

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(cfg, Formatting.Indented));
            return cfg;
        }
        
        var json = File.ReadAllText(filePath);
        // 检查文件是否为空或只包含空白字符
        if (string.IsNullOrWhiteSpace(json))
        {
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