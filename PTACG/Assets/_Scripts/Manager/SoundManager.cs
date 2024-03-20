using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{    
    [Header("Music")]
	[SerializeField] private AudioClip musicClip;

    [Header("Sounds")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip itemClip;
	
    
    public AudioClip ShootClip => shootClip;
    public AudioClip ImpactClip => impactClip;
    public AudioClip CoinClip => coinClip;
    public AudioClip ItemClip => itemClip;
    public AudioClip DashClip => dashClip; 

	private AudioSource musicAudioSource;    
    private ObjectPooler soundObjectPooler;

	private Dictionary<AudioClip, AudioSource> activeAudioSources = new Dictionary<AudioClip, AudioSource>(); // Track active audio sources for each clip

    protected override void Awake()
    {
        soundObjectPooler = GetComponent<ObjectPooler>();
        musicAudioSource = GetComponent<AudioSource>();
        
        PlayMusic();
	}

    public void PlayMusic()
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = musicClip;
		musicAudioSource.volume = 0.6f; // Set volume to 0.6f
        musicAudioSource.Play();
    }
	
	public void StopMusic()
    {
		musicAudioSource.clip = musicClip;
		musicAudioSource.volume = 0f; // Set volume to 0f
        musicAudioSource.Stop();
    }
    
    public void PlaySound(AudioClip clipToPlay, float volume)
    {
        GameObject audioPooled = soundObjectPooler.GetObjectFromPool();
        AudioSource audioSource = null;

        if (audioPooled != null)
        {
            audioPooled.SetActive(true);
            audioSource = audioPooled.GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.volume = volume;
            audioSource.Play();

            activeAudioSources[clipToPlay] = audioSource; // Add the audio source to the dictionary with the clip as key
            StartCoroutine(ReturnToPool(audioPooled, clipToPlay.length + 1));
        }
    }
	
	public void StopSound(AudioClip clipToStop)
    {
        if (activeAudioSources.ContainsKey(clipToStop))
        {
            AudioSource audioSource = activeAudioSources[clipToStop];
            if (audioSource != null)
            {
                audioSource.Stop();
                activeAudioSources.Remove(clipToStop); // Remove the audio source from the dictionary
            }
        }
    }

    private IEnumerator ReturnToPool(GameObject objectPool, float delay)
    {
        yield return new WaitForSeconds(delay);
        objectPool.SetActive(false);
    }
}