using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MusicPlayer;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class MusicPlayer : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public const string PLUGIN_GUID = "com.theblackvoid.musicplayer";
    public const string PLUGIN_NAME = "Music Player";
    public const string PLUGIN_VERSION = "0.5.0";
    public Harmony HarmonyInstance = new Harmony(PLUGIN_GUID);
    
    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            gameObject.AddComponent<AudioLoader>();
            HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Successfully loaded!");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error while loading the plugin: {ex}");
        }
    }
}
