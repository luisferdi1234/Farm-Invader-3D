using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GadgetsMenu : MonoBehaviour
{
    //Text Objects
    [SerializeField] TextMeshProUGUI AppleText;
    [SerializeField] TextMeshProUGUI EMPText;
    [SerializeField] TextMeshProUGUI HoloBushText;
    [SerializeField] TextMeshProUGUI DopplegangerText;

    //Item Objects
    [SerializeField] GameObject menuApple;
    [SerializeField] GameObject menuEmp;
    [SerializeField] GameObject menuHoloBush;
    [SerializeField] GameObject menuDoppleganger;

    private void Start()
    {
        //Update Apple Text
        if (PlayerPrefs.HasKey("Level1"))
        {
            AppleText.text = $"Cosmic Apple: \nUse the apple to lure cows to it's location";
            menuApple.transform.localPosition = new Vector3(470, 40, 0);
        }
        else
        {
            AppleText.text = "???";
        }
        //Update EMP Text
        if (PlayerPrefs.HasKey("Level9"))
        {
            EMPText.text = $"EMP: Duration 5s\nUse the EMP to turn off all flashlights in the level";
            menuEmp.transform.localPosition = new Vector3(550, 30, 0);
        }
        else
        {
            EMPText.text = "???";
        }
        
        //Update Holo Bush Text
        if (PlayerPrefs.HasKey("Level14"))
        {
            HoloBushText.text = $"Holo Bush:\r\nUse to create a bush at your location";
            menuHoloBush.transform.localPosition = new Vector3(470, 0, 0);
        }
        else
        {
            HoloBushText.text = "???";
        }

        //Update Doppleganger Text
        if (PlayerPrefs.HasKey("Level17"))
        {
            DopplegangerText.text = $"Doppleganger Decoy: Duration 5s\r\nUse to lure away farmers and dogs. (Does not affect score)";
            menuDoppleganger.transform.localPosition = new Vector3(710, -40, 0);
        }
        else
        {
            DopplegangerText.text = "???";
        }
    }
}
