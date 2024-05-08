using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public enum AudioChannel
{
    MUSIC = 0,
    SFX,
}

public class SoundManager : MonoBehaviour
{
    [Header("Internal references")]
    [SerializeField] private AudioMixerGroup _musicAudioMixer;
    [SerializeField] private AudioMixerGroup _sfxAudioMixer;
    [SerializeField] private List<AudioSourceList> _sources;

    [Header("External references")]
    [SerializeField] private RSO_Volume _rsoMusicVolume;
    [SerializeField] private RSO_Volume _rsoSfxVolume;
    [SerializeField] private RSE_PlaySound _rsePlaySound;

    private void LaunchSound(Sound soundData)
    {
        if (IsSoundPlaying(soundData.channel, ref soundData.clip)) 
        { 
			return;
        } 

		AudioSource source = GetSourceTarget(soundData.channel, soundData.clip); 
		source.clip = soundData.clip; 
		source.loop = soundData.doLoop;
		source.Play();

		if (!soundData.doLoop) 
		{
			StartCoroutine(UnlockSourceAudio(source, soundData.clip.length));
		}
    }

    private bool IsSoundPlaying(AudioChannel typeSound, ref AudioClip audioClip)
    {
        return _sources
			.Find(source => source.channel == typeSound).sources[0]
			.clip == audioClip;
    }

    private AudioSource GetSourceTarget(AudioChannel typeSound, AudioClip audioClip)
    {
        AudioSource source = _sources
			.Find(source => source.channel == typeSound).sources
			.Find(source => source.clip == audioClip);

        return (source == null) ? _sources.Find(o => o.channel == typeSound).sources.Find(o => o.clip == null) : source;
    }

    private IEnumerator UnlockSourceAudio(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.clip = null;
    }

    private void UpdateMusicVolume() 
	{
		if (_rsoMusicVolume.value == 0)
		{
			_musicAudioMixer.audioMixer.SetFloat("MusicVolume", -80);
			return;
		}
		_musicAudioMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(_rsoMusicVolume.value) * 20);
	}

    private void UpdateSfxVolume() 
	{
		if (_rsoSfxVolume.value == 0)
		{
			_sfxAudioMixer.audioMixer.SetFloat("SFXVolume", -80);
			return;
		}
		_sfxAudioMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(_rsoSfxVolume.value) * 20);
	}

    private void OnEnable()
    {
        _rsePlaySound.action += LaunchSound;
        _rsoSfxVolume.OnChanged += UpdateSfxVolume;
        _rsoMusicVolume.OnChanged += UpdateMusicVolume;
    }

    private void OnDisable()
    {
        _rsePlaySound.action -= LaunchSound;
        _rsoSfxVolume.OnChanged -= UpdateSfxVolume;
        _rsoMusicVolume.OnChanged -= UpdateMusicVolume;
    }
}

[System.Serializable]
public class AudioSourceList
{
    public AudioChannel channel;
    public List<AudioSource> sources;
}