using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private int previousSound;
    [SerializeField] List<AudioClip> chaseSounds;
    [SerializeField] List<AudioClip> returnSounds;
    [SerializeField] List<AudioClip> cowSounds;

    Dictionary<string, List<AudioClip>> audioClipsDictionary = new Dictionary<string, List<AudioClip>>();

    [SerializeField] AudioSource cowSource;
    [SerializeField] AudioSource farmerSource;

    public void Start()
    {
        instance = this;
        //Adds audio clip lists to the dictionary
        audioClipsDictionary.Add("chaseSounds", chaseSounds);
        audioClipsDictionary.Add("returnSounds", returnSounds);
        audioClipsDictionary.Add("cowSounds", cowSounds);
    }

    /// <summary>
    /// Grabs audio from dictionary and plays a random sound
    /// </summary>
    /// <param name="listName"></param>
    public void PlayRandomAudioClip(string listName)
    {
        if (audioClipsDictionary.ContainsKey(listName) && audioClipsDictionary[listName].Count > 0)
        {
            if (listName == "cowSounds")
            {
                PlaySound(cowSource, listName);
            }
            else
            {
                PlaySound(farmerSource, listName);
            }
        }
        else
        {
            Debug.LogError($"AudioManager: No audio clips found in the list {listName}");
        }
    }

    public void PlaySound(AudioSource audioSource, string listName)
    {
        int selection = Random.Range(0, audioClipsDictionary[listName].Count - 1);

        // Exclude the previous sound index
        selection = (selection >= previousSound) ? selection + 1 : selection;

        // Update the previous sound index
        previousSound = selection;

        //Play the sound
        AudioClip randomClip = audioClipsDictionary[listName][selection];
        audioSource.PlayOneShot(randomClip);
    }
}
