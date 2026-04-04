using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SuperSpace.Addition.i18n;

public static class i18n
{
    private static readonly JsonElement _rootNode;
    private static readonly string _currentLang;
    private static readonly bool _isInitialized;

    static i18n()
    {
        try
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Languages.json");
            if (!File.Exists(jsonPath))
            {
                _currentLang = CultureInfo.CurrentUICulture.Name;
                return;
            }

            // 直接读取字节数组，避开字符串二次转换
            byte[] jsonData = File.ReadAllBytes(jsonPath);
            using (JsonDocument doc = JsonDocument.Parse(jsonData))
            {
                // Clone 出一份内存中的副本，允许 JsonDocument 释放
                _rootNode = doc.RootElement.Clone();
            }

            _isInitialized = _rootNode.ValueKind == JsonValueKind.Object;
            _currentLang = DetermineLanguage(_rootNode);
        }
        catch
        {
            _currentLang = "en-US";
            _isInitialized = false;
        }
    }

    private static string DetermineLanguage(JsonElement root)
    {
        var ui = CultureInfo.CurrentUICulture;
        string full = ui.Name;
        string shortName = ui.TwoLetterISOLanguageName;

        if (root.TryGetProperty(full, out _)) return full;
        if (shortName == "zh" && root.TryGetProperty("zh-CN", out _)) return "zh-CN";
        if (root.TryGetProperty(shortName, out _)) return shortName;

        return root.TryGetProperty("en-US", out _) ? "en-US" : "default";
    }

    public static string T(string path, params object[] args)
    {
        if (!_isInitialized) return path;

        // 查找优先级：当前语言 -> en-US -> default
        if (TryResolve(path, _currentLang, out string? res)) return Format(res, args);
        if (_currentLang != "en-US" && TryResolve(path, "en-US", out res)) return Format(res, args);
        if (_currentLang != "default" && TryResolve(path, "default", out res)) return Format(res, args);

        return path;
    }

    private static bool TryResolve(string path, string langKey, [NotNullWhen(true)] out string? result)
    {
        result = null;
        if (!_rootNode.TryGetProperty(langKey, out JsonElement current)) return false;

        // 高性能优化：使用 Span 遍历路径，不产生 string.Split 的堆分配
        ReadOnlySpan<char> pathSpan = path.AsSpan();
        int start = 0;
        while (start < pathSpan.Length)
        {
            int dotIndex = pathSpan[start..].IndexOf('.');
            ReadOnlySpan<char> key = dotIndex == -1 ? pathSpan[start..] : pathSpan.Slice(start, dotIndex);

            if (current.ValueKind != JsonValueKind.Object || !current.TryGetProperty(key, out current))
                return false;

            if (dotIndex == -1) break;
            start += dotIndex + 1;
        }

        result = current.ValueKind == JsonValueKind.String ? current.GetString() : null;
        return result != null;
    }

    private static string Format(string template, object[] args)
    {
        if (args.Length == 0) return template;
        try
        {
            return string.Format(CultureInfo.CurrentCulture, template, args);
        }
        catch { return template; }
    }
}