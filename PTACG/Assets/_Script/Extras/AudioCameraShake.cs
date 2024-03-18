using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class AudioCameraShake : MonoBehaviour
{
	public AudioSource source;
	public AudioLoudnessDetection detector;
	public ShakeData shakeData;
	public float gap = 0.3f;

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip);
		//Debug.Log(loudness);
		if(loudness > gap)
		{
			CameraShakerHandler.Shake(shakeData);
		}
    }
}
