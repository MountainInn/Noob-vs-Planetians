using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Linq;

public static class GameSettingsMenu
{
    public const string GameSettingsPath = "Assets/Resources/" + GameSettingsResourcePath;
    public const string GameSettingsResourcePath = "GameSettings.asset";

    static Object[] previousSelection = null;

    [Shortcut("Open Game Settings")]
    [MenuItem("☸ Configs/Global Game Settings")]
    static public void OpenGlobalGameSettings()
    {
        var gameSettingsAsset = EnsureLoadAssetAtPath<GameSettings>(GameSettingsPath);
        
        if (Selection.objects.Contains(gameSettingsAsset))
        {
            Selection.objects = previousSelection;
        }
        else
        {
            previousSelection = Selection.objects;
            
            Selection.objects = new[] { gameSettingsAsset };
        }
    }

    static public T EnsureLoadAssetAtPath<T>(string path)
        where T : UnityEngine.Object, new()
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null)
            {
                AssetDatabase.CreateAsset(new T(), path);
            }

            asset = AssetDatabase.LoadAssetAtPath<T>(path);

            return asset;
        }
}
