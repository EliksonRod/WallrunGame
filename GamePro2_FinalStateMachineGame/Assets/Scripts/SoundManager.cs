using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    //The numbers are the index of the sound in the soundList array
    Player_Jumping = 0,
    Player_Footsteps = 1,
}
public enum SoundSource
{
    Player = 0,
    Monster = 1,
}
[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    [SerializeField] private AudioSource[] audioSource;

    private void Awake()
    {
        instance = this;
    }

    public static void PlaySound(SoundSource source, SoundType sound, float volume = 1, float pitch = 1)
    {
        instance.audioSource[(int)source].pitch = pitch;
        instance.audioSource[(int)source].PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
