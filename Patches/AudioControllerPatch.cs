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
            var activeBoss = GetHighestTierActiveBoss();

            if (activeBoss != null)
            {
                var currentBoss = activeBoss.identity.type;
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
                var curWaveTier = Traverse.Create(WavesController.instance).Field("WaveDefinition").Method("GetWaveTier", WavesController.instance.LastSpawnedWave).GetValue<int>();
                audioLoaderInstance?.PlayMusic(MusicType.ActiveWave, curWaveTier);
            }
            else if (audioLoaderInstance != null && audioLoaderInstance.currentMusicType != MusicType.None && audioLoaderInstance.audioSource.isPlaying)
            {
                audioLoaderInstance.StopMusic();
            }
        }
    }

    private static Zombie GetHighestTierActiveBoss()
    {
        if (ZombieLoader.Instance == null || BossfightController.instance == null || ZombieController.instance == null) return null;
        if (!BossfightController.IsBossActive && !ZombieController.instance.respawningBossExists) return null;

        Zombie highestTierBoss = null;
        int maxTier = -1;

        var zombies = ZombieLoader.Instance.zombies;
        for (int i = 0; i < zombies.Count; i++)
        {
            var zombie = zombies[i];
            if (zombie.IsBoss && zombie.health.isAlive)
            {
                int tier = BossfightController.instance.GetBossTier(zombie.identity.type);
                if (tier > maxTier)
                {
                    maxTier = tier;
                    highestTierBoss = zombie;
                }
            }
        }

        return highestTierBoss;
    }
}
