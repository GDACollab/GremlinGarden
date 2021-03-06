using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> ambientSounds;

    private Dictionary<AudioClip, (AudioSource, bool)> AudioDict =
        new Dictionary<AudioClip, (AudioSource, bool)>();

    public float baseVolume = 1;

    public float minPlayCooldown = 0;
    public float maxPlayCooldown = 1;

    public float minClipDuration = 5;
    public float maxClipDuration = 10;

    public float fadeDuration = 0.5f;

    private void Awake()
    {
        ambientSounds.ForEach(clip =>
        {
            var thisSource = gameObject.AddComponent<AudioSource>();
            thisSource.clip = clip;
            thisSource.volume = baseVolume;
            AudioDict.Add(clip, (thisSource, false));
        });
    }

    private void Start()
    {
        StartCoroutine(LoopPlayAmbience());
    }

    private IEnumerator LoopPlayAmbience()
    {
        while(true)
        {
            AudioClip thisClip = ambientSounds[Random.Range(0, ambientSounds.Count)];
            AudioSource thisSource = AudioDict[thisClip].Item1;
            bool thisPlaying = AudioDict[thisClip].Item2;

            if (!thisSource.isPlaying && !thisPlaying)
            {
                float thisStart = Random.Range(0, thisClip.length - thisClip.length / 10);
                float thisDuration = Random.Range(minClipDuration, maxClipDuration);
                thisDuration = Mathf.Clamp(thisDuration, 0, thisClip.length - thisStart);

                thisSource.time = thisStart;

                AudioDict[thisClip] = (thisSource, true);
                StartCoroutine(PlaySource(thisSource, thisStart, thisDuration));
            }

            float thisWait = Random.Range(minPlayCooldown, maxPlayCooldown);
            yield return new WaitForSeconds(thisWait);
        }
    }

    private IEnumerator PlaySource(AudioSource thisSource, float start, float duration)
    {
        print("play start: " + thisSource.clip.name);
        thisSource.time = start;
        thisSource.volume = 0;
        thisSource.Play();
        yield return StartCoroutine(FadeAudio(thisSource, fadeDuration, baseVolume));
        yield return new WaitForSeconds(duration - fadeDuration*2);
        yield return StartCoroutine(FadeAudio(thisSource, fadeDuration, 0));

        thisSource.Stop();
        AudioDict[thisSource.clip] = (thisSource, false);
        print("play end: " + thisSource.clip.name);
        yield break;
    }

    private IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            t = t * t * (3f - 2f * t);
            audioSource.volume = Mathf.Lerp(start, targetVolume, t);
            currentTime += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
        yield break;
    }
}
