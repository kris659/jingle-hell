using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static float musicVolume
    {
        get { return _musicVolume; }
        set {            
            _musicVolume = value;
            UpdateMusicVolume();
        }
    }
    public static float masterVolume
    {
        get { return _masterVolume; }
        set
        {
            _masterVolume = value;
            UpdateMusicVolume();
        }
    }

    private static float _musicVolume;
    private static float _masterVolume;
    public static float sfxVolume;

    public SoundAudioClip[] soundAudioClips;
    static SoundAudioClip[] _soundAudioClips;

    public float normalMusicVolume = 1;
    static float _normalMusicVolume;
    public float bossMusicVolume = 1;
    static float _bossMusicVolume;
    static AudioSource mainMusicAudioSource;

    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip bossMusic;
    private static bool _isPlayingNormalMusic;
    private static bool isPlayingNormalMusic
    {
        get { return _isPlayingNormalMusic; }
        set
        {
            _isPlayingNormalMusic = value;
            UpdateMusicVolume();
        }
    }

    public enum Sound { 
        AnvilHit,
        AnvilPickup,
        AnvilThrowCharge,
        AnvilThrow,
        BigPresentAttack,
        SmallPresentsAttack,
        StompAttack,
        PresentExplosion1, PresentExplosion2, PresentExplosion3, PresentExplosion4,
        ElfDamage1, ElfDamage2, ElfDamage3,
        PlayerDamage1, PlayerDamage2,
        SantaDamage1, SantaDamage2, SantaDamage3,
        ElfAttack,
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public SoundInfo soundInfo;
    }
    [System.Serializable]
    public class SoundInfo {
        public AudioClip audioClip;
        [Range(0,1)]
        public float volume;
    }



    private void Awake()
    {
        _soundAudioClips = soundAudioClips;
        mainMusicAudioSource = player.GetComponent<AudioSource>();
        _normalMusicVolume = normalMusicVolume;
        _bossMusicVolume = bossMusicVolume;

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.75f);

        isPlayingNormalMusic = true;
        mainMusicAudioSource.clip = normalMusic;
        mainMusicAudioSource.Play();

        BossEnter.OnBossEnter += () => {
            isPlayingNormalMusic = false;
            mainMusicAudioSource.clip = bossMusic;
            mainMusicAudioSource.Play();
        };
    }

    public static GameObject PlaySound(Sound sound)
    {
        return PlaySound(GetSoundInfo(sound));
    }
    public static GameObject PlaySound(Sound sound, Transform parent)
    {
        return PlaySound(GetSoundInfo(sound), parent);
    }

    public static GameObject PlayMusic(AudioClip audioClip, float volume)
    {
        GameObject soundGameObject = new GameObject("Music");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = volume * musicVolume * masterVolume;
        audioSource.PlayOneShot(audioClip);
        return soundGameObject;
    }

    public static GameObject PlaySound(SoundInfo soundInfo)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.volume = sfxVolume * soundInfo.volume * masterVolume;
        audioSource.PlayOneShot(soundInfo.audioClip);
        Destroy(soundGameObject, soundInfo.audioClip.length);
        return soundGameObject;
    }
    public static GameObject PlaySound(SoundInfo soundInfo, Transform parent)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.parent = parent;
        soundGameObject.transform.localPosition = Vector3.zero;        
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = soundInfo.audioClip;
        audioSource.volume = sfxVolume * soundInfo.volume * masterVolume;
        audioSource.Play();
        return soundGameObject;
    }
    static SoundInfo GetSoundInfo(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in _soundAudioClips){
            if (soundAudioClip.sound == sound) return soundAudioClip.soundInfo;
        }
        Debug.LogError("Nie znaleziono dzwiêku!");
        return null;
    }
    static void UpdateMusicVolume()
    {
        Debug.Log("A");
        if(isPlayingNormalMusic)
            mainMusicAudioSource.volume = _musicVolume * _normalMusicVolume * _masterVolume;
        else
            mainMusicAudioSource.volume = _musicVolume * _bossMusicVolume * _masterVolume;
    }
}
