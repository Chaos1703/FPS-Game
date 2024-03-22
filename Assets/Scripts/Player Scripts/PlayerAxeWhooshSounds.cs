using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAxeWhooshSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] whooshSounds;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayWhooshSound(){
        audioSource.clip = whooshSounds[Random.Range(0, whooshSounds.Length)];
        audioSource.Play();
    }
}
