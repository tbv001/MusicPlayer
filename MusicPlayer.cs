using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MusicPlayer;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
public class MusicPlayer : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    public const string PLUGIN_GUID = "com.theblackvoid.musicplayer";
    public const string PLUGIN_NAME = "Music Player";
    public const string PLUGIN_VERSION = "1.1.0";
    public Harmony HarmonyInstance = new Harmony(PLUGIN_GUID);
    public static int musicVolume = 100;
    
    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            ConfigEntry<int> musicVolumeConfig = Config.Bind("Settings", "Music Volume", 50, new ConfigDescription("Sets the music volume.", new AcceptableValueRange<int>(0, 100)));
            musicVolume = musicVolumeConfig.Value;

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
