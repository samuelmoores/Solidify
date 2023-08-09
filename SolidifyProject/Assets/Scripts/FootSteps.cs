using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footsteps;
    AudioSource footstepsSource;
    // Start is called before the first frame update
    void Start()
    {
        footstepsSource = GetComponent<AudioSource>();
    }

    private void Step()
    {
        AudioClip clip = footsteps[UnityEngine.Random.Range(0, footsteps.Length)]; 
        footstepsSource.PlayOneShot(clip);
    }

}
