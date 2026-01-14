using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuperSpace.Pages.i18n;

// 1. 定义源代码生成器上下文
[JsonSerializable(typeof(Dictionary<string, JsonElement>))]
internal partial class i18nJsonContext : JsonSerializerContext
{
}

public static class i18n
{
    private static Dictionary<string, JsonElement>? _localizedData;
    private static string _currentLang = "en-US";
    private static string _debugInfo = string.Empty;

    // 2. 配置序列化选项（注意：这里不再需要那么多反射配置）
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        AllowTrailingCommas = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        PropertyNameCaseInsensitive = true,
        TypeInfoResolver = i18nJsonContext.Default // 关键：指定生成的 Resolver
    };

    static i18n()
    {
        LoadLanguage();
    }

    private static void LoadLanguage()
    {
        try
        {
            string assemblyPath = AppContext.BaseDirectory;
            string jsonPath = Path.Combine(assemblyPath, "Languages.json");

            if (!File.Exists(jsonPath))
            {
                _debugInfo = $"[Missing: {jsonPath}]";
                return;
            }

            string jsonContent = File.ReadAllText(jsonPath);

            // 3. 使用 Context 实例进行反序列化，彻底消除 IL3050/IL2026
            _localizedData = JsonSerializer.Deserialize(
                jsonContent,
                i18nJsonContext.Default.DictionaryStringJsonElement);

            if (_localizedData == null) return;

            // 语言匹配逻辑保持不变...
            var uiCulture = CultureInfo.CurrentUICulture;
            string fullTag = uiCulture.Name;
            string langTwoLetter = uiCulture.TwoLetterISOLanguageName;

            if (_localizedData.ContainsKey(fullTag))
                _currentLang = fullTag;
            else if (langTwoLetter == "zh")
                _currentLang = _localizedData.ContainsKey("zh-CN") ? "zh-CN" : "en-US";
            else
                _currentLang = _localizedData.ContainsKey(langTwoLetter) ? langTwoLetter : "en-US";
        }
        catch (Exception ex)
        {
            _debugInfo = $"[Error: {ex.Message}]";
            _currentLang = "en-US";
        }
    }

    // T 方法和 TryGetFromLang 方法保持不变，使用之前提供的 Null 安全版本即可
    public static string T(string path, params object[] args)
    {
        if (_localizedData == null) return $"{path} {_debugInfo}";
        string[] keys = path.Split('.');
        if (TryGetFromLang(_currentLang, keys, out string? result)) return FormatString(result, args);
        if (TryGetFromLang("en-US", keys, out result)) return FormatString(result, args);
        if (TryGetFromLang("default", keys, out result)) return FormatString(result, args);
        return path;
    }

    private static bool TryGetFromLang(string langKey, string[] keys, [NotNullWhen(true)] out string? result)
    {
        result = null;
        if (_localizedData == null || !_localizedData.TryGetValue(langKey, out JsonElement current))
            return false;
        try
        {
            foreach (var key in keys)
            {
                if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(key, out var next))
                    current = next;
                else
                    return false;
            }
            result = current.GetString();
            return result != null;
        }
        catch { return false; }
    }

    private static string FormatString(string template, object[] args)
    {
        if (string.IsNullOrEmpty(template)) return string.Empty;
        try
        {
            return args.Length == 0 ? template : string.Format(CultureInfo.CurrentCulture, template, args);
        }
        catch { return template; }
    }
}