using System;
using System.Collections.Generic;
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
    private readonly string _musicFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Music");
    private readonly Dictionary<string, AudioClip> _audioCache = new();
    private const float _fadeSpeed = 1f;
    public static AudioLoader Instance { get; private set; }
    public AudioSource audioSource;
    public MusicType currentMusicType = MusicType.None;
    private string _currentPlayingPath;
    private float _targetVolume = 1f;
    private float _maxVolume = 1f;

    private async void Awake()
    {
        Instance = this;
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        audioSource.volume = 0f;

        if (Directory.Exists(_musicFolder))
        {
            var files = Directory.GetFiles(_musicFolder, "*.mp3");
            foreach (var file in files)
            {
                var clip = await LoadAudioClip(file);
                if (clip != null)
                {
                    _audioCache[file] = clip;
                    MusicPlayer.Logger.LogInfo($"Cached audio: {file}");
                }
            }
        }
    }

    private void Update()
    {
        if (PersistenceController.instance?.soundsMenu?.saveAudio == null)
        {
            return;
        }

        var masterVolumeGame = PersistenceController.instance.soundsMenu.saveAudio.master / 100f;
        var musicVolumeCfg = MusicPlayer.musicVolume / 100f;
        _maxVolume = masterVolumeGame * musicVolumeCfg;

        if (audioSource != null)
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, _targetVolume * _maxVolume, Time.deltaTime * _fadeSpeed);
            
            if (_targetVolume <= 0f && audioSource.volume <= 0f && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void LoadAndPlay(string filePath)
    {
        if (_currentPlayingPath == filePath && (audioSource.isPlaying || _audioCache.ContainsKey(filePath) == false)) return;

        if (_audioCache.TryGetValue(filePath, out var clip))
        {
            audioSource.clip = clip;
            if (!audioSource.isPlaying) audioSource.Play();
            _currentPlayingPath = filePath;
        }
        else
        {
            _currentPlayingPath = filePath;
            MusicPlayer.Logger.LogWarning($"Audio clip not found in cache: {filePath}. Attempting on-demand load.");
            LoadAndPlayOnDemand(filePath);
        }
    }

    private async void LoadAndPlayOnDemand(string filePath)
    {
        var clip = await LoadAudioClip(filePath);
        if (clip != null)
        {
            _audioCache[filePath] = clip;
            
            if (_currentPlayingPath == filePath)
            {
                audioSource.clip = clip;
                if (!audioSource.isPlaying) audioSource.Play();
            }
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

    private void ActuallyPlayMusic(string path, MusicType musicType)
    {
        if (currentMusicType == musicType && _currentPlayingPath == path)
        {
            _targetVolume = 1f;
            return;
        }

        currentMusicType = musicType;

        if (File.Exists(path))
        {
            _targetVolume = 1f;
            LoadAndPlay(path);
        }
        else
        {
            _currentPlayingPath = path;
            _targetVolume = 0f;
            MusicPlayer.Logger.LogError($"Music not found at: {path}");
        }
    }

    public void PlayMusic(MusicType musicType, int? waveTier = 1)
    {
        switch (musicType)
        {
            case MusicType.Menu:
                var menuMusicPath = Path.Combine(_musicFolder, "Menu.mp3");
                ActuallyPlayMusic(menuMusicPath, MusicType.Menu);

                break;

            case MusicType.ActiveWave:
                waveTier = Math.Clamp(waveTier ?? 1, 1, 3);
                var waveMusicPath = Path.Combine(_musicFolder, $"Wave{waveTier}.mp3");
                ActuallyPlayMusic(waveMusicPath, MusicType.ActiveWave);

                break;

            case MusicType.BossRiot:
                var riotBossMusicPath = Path.Combine(_musicFolder, "BossRiot.mp3");
                ActuallyPlayMusic(riotBossMusicPath, MusicType.BossRiot);

                break;

            case MusicType.BossQueen:
                var queenBossMusicPath = Path.Combine(_musicFolder, "BossQueen.mp3");
                ActuallyPlayMusic(queenBossMusicPath, MusicType.BossQueen);

                break;

            case MusicType.BossReaper:
                var reaperBossMusicPath = Path.Combine(_musicFolder, "BossReaper.mp3");
                ActuallyPlayMusic(reaperBossMusicPath, MusicType.BossReaper);

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
