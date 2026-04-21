using UnityEngine;
using UnityEngine.Audio;

namespace TacticalEleven.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Mixer Reference")]
        public AudioMixer audioMixer;

        [Header("Audio Sources")]
        public AudioSource musicSource;
        public AudioSource sfxSource;

        [Header("Volumes (0-1)")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.15f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            ApplyVolumeSettings();
        }

        // --- MUSIC & SFX METHODS ---
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (musicSource.clip == clip) return;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        // --- VOLUME CONTROL METHODS ---
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            SetMixerVolume(Constants.MASTER_VOLUME_PARAMETER, masterVolume);
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            SetMixerVolume("MusicVolumeParameter", musicVolume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            SetMixerVolume(Constants.SFX_VOLUME_PARAMETER, sfxVolume);
        }

        private void ApplyVolumeSettings()
        {
            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);
        }

        private void SetMixerVolume(string parameter, float normalizedVolume)
        {
            // Convertimos 0–1 a decibelios (AudioMixer usa dB)
            float dB = Mathf.Log10(Mathf.Max(normalizedVolume, 0.0001f)) * 20f;
            audioMixer.SetFloat(parameter, dB);
        }
    }
}