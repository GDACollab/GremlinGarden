using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GremlinAudioController : MonoBehaviour
{
    private PauseController pauseController;
    private AudioSource[] sounds;

    public int chanceToPlay = 100;

    private void Awake()
    {
        pauseController = GameObject.FindWithTag("Manager").GetComponent<PauseController>();
        sounds = this.GetComponents<AudioSource>();
    }

    public void FixedUpdate()
    {
        if (pauseController.paused)
        {
            foreach (var component in this.GetComponents<AudioSource>())
            {
                if (component.isPlaying)
                    component.Pause();
            }
        }
        else
        {
            //Unpause any audio tracks that were paused
            foreach (var component in this.GetComponents<AudioSource>())
                component.UnPause();

            //Then, check if any audio is playing before playing a new sound
            if (!IsAudioPlaying())
                PlayIdle();
        }
    }

    public bool IsAudioPlaying()
    {
        foreach (var component in this.GetComponents<AudioSource>())
        {
            if (component.isPlaying) return true;
        }
        return false;
    }

    public void PlayIdle()
    {
        int chance = Random.Range(0, chanceToPlay);
        if (chance < 1)
            sounds[Random.Range(8, 11)].Play();
    }

    public void PlayThrow()
    {
        StopSounds();
        int chance = Random.Range(0, 100);
        if (chance < 45)
            sounds[0].Play();
        else if (chance < 90)
            sounds[1].Play();
        else
            sounds[2].Play();
    }

    public void PlayPet()
    {
        StopSounds();
        sounds[Random.Range(3, 5)].Play();
    }

    public void PlayEat()
    {
        StopSounds();
        sounds[Random.Range(5, 8)].Play();
    }

    public void StopSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
            sounds[i].Stop();
    }

}
