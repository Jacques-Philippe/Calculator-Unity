using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    /// <summary>
    /// The name associated to the sound
    /// </summary>
    [SerializeField]
    public string Name;
    /// <summary>
    /// The sound file the sound is expected to play
    /// </summary>
    public AudioClip Clip;

    /// <summary>
    /// The volume of the sound
    /// </summary>
    [Range(0f, 1f)]
    public float Volume;

    /// <summary>
    /// The sound's pitch
    /// </summary>
    [Range(0f, 2f)]
    public float Pitch;

    /// <summary>
    /// The audiosource property to which we map all our custom valies, i.e. Name, Volume, Pitch, etc.
    /// </summary>
    [HideInInspector]
    public AudioSource Source;
}
