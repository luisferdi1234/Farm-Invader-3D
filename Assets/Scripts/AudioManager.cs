using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private int previousSound;
    //Farmer Sounds
    [SerializeField] List<AudioClip> chaseSounds;
    [SerializeField] List<AudioClip> returnSounds;
    [SerializeField] List<AudioClip> empSounds;

    //Cow Sounds
    [SerializeField] List<AudioClip> cowSounds;
    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] List<AudioClip> grassSounds;

    //Dog Sounds
    [SerializeField] List<AudioClip> dogGrowlSounds;
    [SerializeField] List<AudioClip> dogBarkSounds;

    Dictionary<string, List<AudioClip>> audioClipsListDictionary = new Dictionary<string, List<AudioClip>>();
    Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource cowSource;
    [SerializeField] AudioSource farmerSource;
    [SerializeField] AudioSource grassSource;
    [SerializeField] AudioSource alienCharge;
    [SerializeField] AudioSource dogSource;

    int selection;
    public void Start()
    {
        instance = this;
        //Adds audio clip lists to the dictionary
        audioClipsListDictionary.Add("chaseSounds", chaseSounds);
        audioClipsListDictionary.Add("returnSounds", returnSounds);
        audioClipsListDictionary.Add("empSounds", empSounds);
        audioClipsListDictionary.Add("cowSounds", cowSounds);
        audioClipsListDictionary.Add("grassSounds", grassSounds);
        audioClipsListDictionary.Add("dogGrowlSounds", dogGrowlSounds);
        audioClipsListDictionary.Add("dogBarkSounds", dogBarkSounds);
        foreach (AudioClip clip in audioClips)
        {
            audioClipDictionary.Add(clip.name, clip);
        }
    }

    /// <summary>
    /// Plays a specific sound
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="volume"></param>
    public void PlaySpecificSound(string soundName, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClipDictionary[soundName]);
    }

    /// <summary>
    /// Grabs audio from dictionary and plays a random sound
    /// </summary>
    /// <param name="listName"></param>
    public void PlayRandomAudioClip(string listName)
    {
        if (audioClipsListDictionary.ContainsKey(listName) && audioClipsListDictionary[listName].Count > 0)
        {
            if (listName == "chaseSounds" || listName == "returnSounds" || listName == "empSounds")
            {
                PlaySound(farmerSource, listName);
            }
            else if (listName == "cowSounds")
            {
                PlaySound(cowSource, listName);
            }
            else if (listName == "grassSounds")
            {
                PlaySound(grassSource, listName);
            }
            else if (listName == "dogGrowlSounds" || listName == "dogBarkSounds")
            {
                PlaySound(dogSource, listName);
            }
        }
        else
        {
            Debug.LogError($"AudioManager: No audio clips found in the list {listName}");
        }
    }

    /// <summary>
    /// Plays a random sound in the list
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="listName"></param>
    public void PlaySound(AudioSource audioSource, string listName)
    {
        selection = Random.Range(0, audioClipsListDictionary[listName].Count - 1);

        // Exclude the previous sound index
        selection = (selection >= previousSound) ? selection + 1 : selection;

        // Update the previous sound index
        previousSound = selection;

        //Play the sound
        AudioClip randomClip = audioClipsListDictionary[listName][selection];

        if (listName == "dogBarkSounds" || listName == "dogGrowlSounds" || listName == "cowSounds")
        {
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(randomClip);
        }
    }

    /// <summary>
    /// Plays the Alien Charge Sound
    /// </summary>
    public void PlayAlienCharge()
    {
        if (alienCharge.isPlaying)
        {
            alienCharge.UnPause();
        }
        else
        {
            alienCharge.Play();
        }
    }

    public void PauseAlienCharge()
    {
        alienCharge.Pause();
    }
}
