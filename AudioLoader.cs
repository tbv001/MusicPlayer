using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MusicPlayer;

public class AudioLoader : MonoBehaviour
{
    private readonly string _musicFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Music");
    private const float _fadeSpeed = 1f;
    public static AudioLoader Instance { get; private set; }
    public AudioSource audioSource;
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
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, _targetVolume, Time.deltaTime * _fadeSpeed);
            
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

    public void PlayMenuMusic()
    {
        var menuMusicPath = Path.Combine(_musicFolder, "Menu.mp3");

        if (File.Exists(menuMusicPath))
        {
            _targetVolume = 1f;
            LoadAndPlay(menuMusicPath);
        }
        else
        {
            MusicPlayer.Logger.LogError($"Menu music not found at: {menuMusicPath}");
        }
    }

    public void StopMusic()
    {
        _targetVolume = 0f;
    }
}
