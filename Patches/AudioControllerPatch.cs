using HarmonyLib;

namespace MusicPlayer;

[HarmonyPatch(typeof(AudioController))]
public class AudioControllerPatch
{
    [HarmonyPatch("SetAmbientVolumes")]
    [HarmonyPostfix]
    public static void SetAmbientVolumes_Postfix(AudioController __instance)
    {
        if (!ZBMain.instance.mapIsLoaded)
        {
            __instance.ambientTargetVolume[0] = 0f;
            __instance.ambientSource[0].volume = 0f;

            if (AudioLoader.Instance != null && AudioLoader.Instance.currentMusicType != MusicType.Menu && !AudioLoader.Instance.audioSource.isPlaying)
            {
                AudioLoader.Instance.PlayMusic(MusicType.Menu);
            }
        }
        else
        {
            if (AudioLoader.Instance != null && AudioLoader.Instance.currentMusicType == MusicType.Menu && AudioLoader.Instance.audioSource.isPlaying)
            {
                AudioLoader.Instance.StopMusic();
            }
        }
    }
}
