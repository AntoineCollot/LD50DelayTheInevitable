using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource = null;
    public AudioSource sfxSource = null;

    public Clip[] clips;

    //0 : MantisAttack
    //1 : Moskito Bite
    //2 : TurnOnLight
    //3 : Mantis Spooked
    [System.Serializable]
    public class Clip
    {
        public AudioClip clip;
        [Range(0, 1)] public float volume;
    }

    public static SoundManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void ToggleMusicMute()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
        else
            musicSource.Play();
    }

    public static void PlaySound(int id)
    {
        if (Instance == null)
            return;
        Instance.sfxSource.PlayOneShot(Instance.clips[id].clip, Instance.clips[id].volume);
    }
}
