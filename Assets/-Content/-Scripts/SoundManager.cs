using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public enum TypeSound
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
    [SerializeField] private RSE_Sound _rsePlaySound, _rseStopSound;

    private void StopSound(TypeSound typeSound, AudioClip audioClip, bool isLoop) 
    { 
        AudioSource source = GetSourceTarget(typeSound, audioClip);
        if (source != null)
        {
            source.clip = null;
            source.Stop();
        }
    }

    private void LaunchSound(TypeSound typeSound, AudioClip audioClip, bool isLoop)
    {
        if (IsSoundPlaying(typeSound, ref audioClip)) 
        { 
			return;
        } 

		AudioSource source = GetSourceTarget(typeSound, audioClip); 
		source.clip = audioClip; 
		source.Play();
		source.loop = isLoop;

		if (!isLoop) 
		{
			StartCoroutine(UnlockSourceAudio(source, audioClip.length));
		}
    }

    private bool IsSoundPlaying(TypeSound typeSound, ref AudioClip audioClip)
    {
        return _sources
			.Find(source => source.typeSound == typeSound).audioSources[0]
			.clip == audioClip;
    }

    private AudioSource GetSourceTarget(TypeSound typeSound, AudioClip audioClip)
    {
        AudioSource source = _sources
			.Find(source => source.typeSound == typeSound).audioSources
			.Find(source => source.clip == audioClip);

        return (source == null) ? _sources.Find(o => o.typeSound == typeSound).audioSources.Find(o => o.clip == null) : source;
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
        _rseStopSound.action += StopSound;
        _rsoSfxVolume.OnChanged += UpdateSfxVolume;
        _rsoMusicVolume.OnChanged += UpdateMusicVolume;
    }

    private void OnDisable()
    {
        _rsePlaySound.action -= LaunchSound;
        _rseStopSound.action -= StopSound;
        _rsoSfxVolume.OnChanged -= UpdateSfxVolume;
        _rsoMusicVolume.OnChanged -= UpdateMusicVolume;
    }
}

[System.Serializable]
public class AudioSourceList
{
    public TypeSound typeSound;
    public List<AudioSource> audioSources;
}