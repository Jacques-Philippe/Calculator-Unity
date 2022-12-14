using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    public List<Sound> sounds;

    protected void Awake()
    {
        this.sounds.ForEach(s =>
        {
            if (s != null)
            {
                s.Source = this.gameObject.AddComponent<AudioSource>();
                s.Source.clip = s.Clip;
                s.Source.volume = s.Volume;
                s.Source.pitch = s.Pitch;
            }
        });
    }

    public void PlaySound(string soundName)
    {
        var sound = this.sounds.First(s => s.Name == soundName);
        if (sound == null)
        {
            throw new System.Exception($"Sound {soundName} was not found in the list of available sounds");
        }
        //Debug.Log($"Playing sound {sound.Name}");
        sound.Source.PlayOneShot(sound.Clip);
    }
}
