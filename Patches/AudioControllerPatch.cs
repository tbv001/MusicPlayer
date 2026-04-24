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

            var audioLoaderInstance = AudioLoader.Instance;
            var isWaveActive = WavesController.instance.HaveToKillZombies;
            var isBossActive = BossfightController.IsBossActive;

            if (isBossActive)
            {
                var currentBoss = BossfightController.instance.GetZombieTypeForTier(WavesController.instance.CurrentlyEnabledBossTier);
                switch (currentBoss)
                {
                    case ZombieType.BossRiot:
                        audioLoaderInstance?.PlayMusic(MusicType.BossRiot);
                        break;
                        
                    case ZombieType.BossQueen:
                        audioLoaderInstance?.PlayMusic(MusicType.BossQueen);
                        break;

                    case ZombieType.BossReaper:
                        audioLoaderInstance?.PlayMusic(MusicType.BossReaper);
                        break;
                    
                    default:
                        audioLoaderInstance?.StopMusic();
                        break;
                }
            }
            else if (isWaveActive)
            {
                //var waveNum = WavesController.instance.LastSpawnedWave;

                audioLoaderInstance?.PlayMusic(MusicType.ActiveWave);
            }
            else if (audioLoaderInstance != null && audioLoaderInstance.currentMusicType != MusicType.None && audioLoaderInstance.audioSource.isPlaying)
            {
                audioLoaderInstance.StopMusic();
            }
        }
    }
}
