using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIntro : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI topText;
    [SerializeField] TextMeshProUGUI bottomText;
    [SerializeField] List<GameObject> thingsToLookAt = new List<GameObject>();

    private int listIndex = 0;
    private float lookTimer = 0;
    private float maxLookTime = 5f;
    private bool introDone;
    private float positionDamping = 7f;

    GameObject playerUI;
    GameObject pauseCanvas;

    //Cinemachine camera
    CinemachineVirtualCamera vcam;

    //Alien Controller
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerProgressManager.Instance == null)
        {
            introDone = true;
        }
        else
        {
            introDone = PlayerProgressManager.Instance.introPlayed;
        }
        if (!introDone)
        {
            //Sets camera
            vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            vcam.Follow = thingsToLookAt[listIndex].transform;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = positionDamping;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = positionDamping;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = positionDamping;

            //Updates top and bottom text
            ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            topText.text = scoreManager.levelName;
            bottomText.text = $"Obtainable Cows: {scoreManager.maxAmountOfCows}";

            //Turns off player UI
            playerUI = GameObject.Find("PlayerCanvas");
            playerUI.SetActive(false);

            //Turns off pause canvas
            pauseCanvas = GameObject.Find("Pause Canvas");
            pauseCanvas.SetActive(false);

            //Grabs player controller
            playerController = GameObject.Find("Alien").GetComponent<PlayerController>();
        }
        else
        {
            //Sets camera
            vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
            vcam.Follow = thingsToLookAt[thingsToLookAt.Count - 1].transform;

            //Gives player movement back
            playerController = GameObject.Find("Alien").GetComponent<PlayerController>();
            playerController.canMove = true;

            //Sets camera speed to normal
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1f;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = 1f;

            //Turns off black bars
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!introDone)
        {
            if (listIndex == 0)
            {
                maxLookTime = .75f;
            }
            else if (listIndex == thingsToLookAt.Count - 1)
            {
                maxLookTime = 2f;
            }
            else
            {
                maxLookTime = 5f;
            }
            if (lookTimer < maxLookTime && listIndex < thingsToLookAt.Count)
            {
                lookTimer += Time.deltaTime;
                if (lookTimer >= maxLookTime)
                {
                    listIndex++;
                    lookTimer = 0;
                    if (listIndex < thingsToLookAt.Count)
                    {
                        vcam.Follow = thingsToLookAt[listIndex].transform;
                    }
                }
            }
            else if (!introDone && listIndex >= thingsToLookAt.Count)
            {
                introDone = true;
                PlayerProgressManager.Instance.introPlayed = true;

                //Turns on canvases
                playerUI.SetActive(true);
                pauseCanvas.SetActive(true);

                //Gives player control back
                playerController.canMove = true;

                //Turns off black bars
                gameObject.SetActive(false);

                //Makes camera move like normal again
                vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1f;
                vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
                vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = 1f;
            }
        }
    }
}
