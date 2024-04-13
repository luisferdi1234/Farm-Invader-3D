using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GadgetsMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI EMPText;
    [SerializeField] TextMeshProUGUI HoloBushText;
    [SerializeField] TextMeshProUGUI DopplegangerText;

    private void Start()
    {
        //Update EMP Text
        if (PlayerPrefs.HasKey("Level9"))
        {
            EMPText.text = $"EMP: Duration 5s\nUse the EMP to turn off all flashlights in the level";
        }
        else
        {
            EMPText.text = "???";
        }
        
        //Update Holo Bush Text
        if (PlayerPrefs.HasKey("Level14"))
        {
            HoloBushText.text = $"Holo Bush:\r\nUse to create a bush at your location";
        }
        else
        {
            HoloBushText.text = "???";
        }

        //Update Doppleganger Text
        if (PlayerPrefs.HasKey("Level17"))
        {
            DopplegangerText.text = $"Doppleganger Decoy: Duration 5s\r\nUse to lure away farmers and dogs. (Does not affect score)";
        }
        else
        {
            DopplegangerText.text = "???";
        }
    }
}
