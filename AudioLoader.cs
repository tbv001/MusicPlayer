using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MusicPlayer;

public enum MusicType
{
    None,
    Menu,
    ActiveWave,
    BossRiot,
    BossQueen,
    BossReaper
}

public class AudioLoader : MonoBehaviour
{
    private readonly string _MUSIC_FOLDER = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Music");
    private const float _FADE_SPEED = 1f;
    public static AudioLoader Instance { get; private set; }
    public AudioSource audioSource;
    public MusicType currentMusicType = MusicType.None;
    private string _currentPlayingPath;
    private float _targetVolume = 1f;

    private void Awake()
    {
        Instance = this;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        audioSource.volume = 0f;
    }

    private void Update()
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, _targetVolume, Time.deltaTime * _FADE_SPEED);
            
            if (_targetVolume <= 0f && audioSource.volume <= 0f && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private async void LoadAndPlay(string filePath)
    {
        if (_currentPlayingPath == filePath && audioSource.isPlaying) return;

        var clip = await LoadAudioClip(filePath);
        if (clip != null)
        {
            audioSource.clip = clip;
            if (!audioSource.isPlaying) audioSource.Play();
            _currentPlayingPath = filePath;
        }
    }

    private async Task<AudioClip> LoadAudioClip(string path)
    {
        var uri = "file://" + path;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
        {
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(www);
            }
            else
            {
                MusicPlayer.Logger.LogError($"Failed to load audio: {www.error}");
                return null;
            }
        }
    }

    public void PlayMusic(MusicType musicType)
    {
        switch (musicType)
        {
            case MusicType.Menu:
                var menuMusicPath = Path.Combine(_MUSIC_FOLDER, "Menu.mp3");

                if (File.Exists(menuMusicPath))
                {
                    _targetVolume = 1f;
                    LoadAndPlay(menuMusicPath);
                    currentMusicType = MusicType.Menu;
                }
                else
                {
                    MusicPlayer.Logger.LogError($"Menu music not found at: {menuMusicPath}");
                }

                break;

            case MusicType.ActiveWave:
                var waveMusicPath = Path.Combine(_MUSIC_FOLDER, "Wave.mp3");

                if (File.Exists(waveMusicPath))
                {
                    _targetVolume = 1f;
                    LoadAndPlay(waveMusicPath);
                    currentMusicType = MusicType.ActiveWave;
                }
                else
                {
                    MusicPlayer.Logger.LogError($"Wave music not found at: {waveMusicPath}");
                }

                break;

            case MusicType.BossRiot:
                var riotBossMusicPath = Path.Combine(_MUSIC_FOLDER, "BossRiot.mp3");

                if (File.Exists(riotBossMusicPath))
                {
                    _targetVolume = 1f;
                    LoadAndPlay(riotBossMusicPath);
                    currentMusicType = MusicType.BossRiot;
                }
                else
                {
                    MusicPlayer.Logger.LogError($"Riot Boss music not found at: {riotBossMusicPath}");
                }

                break;

            case MusicType.BossQueen:
                var queenBossMusicPath = Path.Combine(_MUSIC_FOLDER, "BossQueen.mp3");

                if (File.Exists(queenBossMusicPath))
                {
                    _targetVolume = 1f;
                    LoadAndPlay(queenBossMusicPath);
                    currentMusicType = MusicType.BossQueen;
                }
                else
                {
                    MusicPlayer.Logger.LogError($"Queen Boss music not found at: {queenBossMusicPath}");
                }

                break;

            case MusicType.BossReaper:
                var reaperBossMusicPath = Path.Combine(_MUSIC_FOLDER, "BossReaper.mp3");

                if (File.Exists(reaperBossMusicPath))
                {
                    _targetVolume = 1f;
                    LoadAndPlay(reaperBossMusicPath);
                    currentMusicType = MusicType.BossReaper;
                }
                else
                {
                    MusicPlayer.Logger.LogError($"Reaper Boss music not found at: {reaperBossMusicPath}");
                }

                break;

            default:
                MusicPlayer.Logger.LogError($"Got: {musicType}. Did you forget something?");
                break;
        }
    }

    public void StopMusic()
    {
        currentMusicType = MusicType.None;
        _targetVolume = 0f;
    }
}
