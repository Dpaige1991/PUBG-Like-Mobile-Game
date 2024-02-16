using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Footsteps Sources")]
    private AudioClip[] footstepsSound;

    private AudioClip GetRandomFootStep()
    {
        return footstepsSound[Random.Range(0, footstepsSound.Length)];
    }

    private void Step()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }
}
