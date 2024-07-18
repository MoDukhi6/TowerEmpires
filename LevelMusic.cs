using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [Header("-----Audio Source-----")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFX;
    
    [Header("-----Audio Clip-----")]
    public AudioClip background;
    public AudioClip endPoint;
    public AudioClip death;
    public AudioClip startWave;

    //UI Abilities
    public AudioClip thunder;
    public AudioClip ice;
    public AudioClip fire;

    //Tower Shoot
    public AudioClip archedShoot;
    public AudioClip shadowShoot;
    public AudioClip magicShoot;
    public AudioClip stoneShoot;

    //Bullet Impact
    public AudioClip stoneImpact;

    //Tower Set
    public AudioClip archedSet1;
    public AudioClip shadowSet1;
    public AudioClip magicSet1;
    public AudioClip stoneSet1;

    public AudioClip archedSet2;
    public AudioClip shadowSet2;
    public AudioClip magicSet2;
    public AudioClip stoneSet2;

    public AudioClip archedSet3;//Not Done Yet
    public AudioClip shadowSet3;//Not Done Yet
    public AudioClip magicSet3;//Not Done Yet
    public AudioClip stoneSet3;//Not Done Yet

    public AudioClip archedSet4;//Not Done Yet
    public AudioClip shadowSet4;//Not Done Yet
    public AudioClip magicSet4;//Not Done Yet
    public AudioClip stoneSet4;//Not Done Yet

    public AudioClip archedSet5;//Not Done Yet
    public AudioClip shadowSet5;//Not Done Yet
    public AudioClip magicSet5;//Not Done Yet
    public AudioClip stoneSet5;//Not Done Yet

    //Win & Lose levels
    public AudioClip Win1Star;
    public AudioClip Win2Stars;
    public AudioClip Win3Stars;
    public AudioClip Lose;


    public void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true; // Set loop property to true

        // Set the volume to 65% (0.65f)
        musicSource.volume = 0.30f;

        musicSource.Play();
    }


    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void PlaySFXModified(AudioClip clip, float volume = 1.0f)
    {
        SFX.PlayOneShot(clip, volume);
    }


}
