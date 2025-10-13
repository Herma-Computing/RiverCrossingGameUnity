using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Level2RiverController : MonoBehaviour
{
    // Characters

    public GameObject Dad;
    public GameObject Mom;
    public GameObject Boy;
    public GameObject Girl;
    public GameObject Boat;

    // Buttons

    public Button GoButton;
    public Button WinPlayAgain;
    public Button WinMainMenuButton;
    public Button PauseContinue;
    public Button PauseMainMenuButton;
    public Button PauseRestart;
    public Button SoundOn;
    public Button SoundOff;
    public Button PauseButton;
    public Button StartLevel2Button;

    // Positions

    public Vector3 DadRightSidePosition = new Vector3((float)3.52, (float)1.098, 0);
    public Vector3 DadLeftSidePosition = new Vector3((float)-3.52, (float)1.098, 0);
    public Vector3 MomRightSidePosition = new Vector3((float)6.323, (float)1.154, 0);
    public Vector3 MomLeftSidePosition = new Vector3((float)-6.323, (float)1.154, 0);
    public Vector3 BoyRightSidePosition = new Vector3((float)4.44, (float)0.69, 0);
    public Vector3 BoyLeftSidePosition = new Vector3((float)-4.44, (float)0.69, 0);
    public Vector3 GirlRightSidePosition = new Vector3((float)5.341617, (float)0.774, 0);
    public Vector3 GirlLeftSidePosition = new Vector3((float)-5.341617, (float)0.774, 0);
    public Vector3 BoatCarryBigRightSidePosition = new Vector3((float)1.13, (float)0.95, 0);
    public Vector3 BoatCarryBigLeftSidePosition = new Vector3((float)-1.13, (float)0.95, 0);
    public Vector3 BoatCarryBoyRightSidePosition = new Vector3((float)0.7, (float)0.54, 0);
    public Vector3 BoatCarryBoyLeftSidePosition = new Vector3((float)-0.7, (float)0.54, 0); 
    public Vector3 BoatCarryGirlRightSidePosition = new Vector3((float)1.56, (float)0.56, 0);
    public Vector3 BoatCarryGirlLeftSidePosition = new Vector3((float)-1.56, (float)0.56, 0);
    public Vector3 BoatRightSidePosition = new Vector3((float)1.182167, (float)0.1985318, 0);
    public Vector3 BoatLeftSidePosition = new Vector3((float)-1.182167, (float)0.1985318, 0);

    // panels

    public GameObject WinState;
    public GameObject Pause;
    public GameObject HowToPlay;

    // Sound

    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] AudioSource JumpSound;

    // List

    public List<char> ONBoat = new List<char>();

    //Timer And Score
    
    public TextMeshProUGUI timerText;
    public GameObject nostar1;
    public GameObject nostar2;
    public GameObject nostar3;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public int starScore;
    public float levelTime = 60f;
    public float timeRemaining;
    public bool isTimerRunning = true;

    void Start()
    {
        GoButton.onClick.AddListener(MoveBoat);
        WinPlayAgain.onClick.AddListener(ResetGame);
        WinMainMenuButton.onClick.AddListener(MainMenu);
        PauseMainMenuButton.onClick.AddListener(MainMenu);
        PauseRestart.onClick.AddListener(ResetGame);
        SoundOn.onClick.AddListener(Soundon);
        SoundOff.onClick.AddListener(Soundoff);
        PauseContinue.onClick.AddListener(Continue);
        PauseButton.onClick.AddListener(PauseMenu);
        StartLevel2Button.onClick.AddListener(StartLevel2);
        timeRemaining = levelTime;
        UpdateTimerUI();
    }
    public void UpdateTimerUI()
  {
    int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
  }

    public void StopTimer()
    {
        isTimerRunning = false;
    }


    void MoveDad()
    {
        JumpSound.Play();

        if (Dad.transform.position == DadRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            if (!CanBoard('D'))
                return;  // Boat full or invalid, ignore input

            Dad.transform.position = BoatCarryBigRightSidePosition;
            ONBoat.Add('D');

        }

        else if (Dad.transform.position == BoatCarryBigRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            Dad.transform.position = DadRightSidePosition;
            ONBoat.Remove('D');
        }

        else if (Dad.transform.position == BoatCarryBigLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            Dad.transform.position = DadLeftSidePosition;
            ONBoat.Remove('D');
        }

        else if (Dad.transform.position == DadLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            if (!CanBoard('D'))
                return;  // Boat full or invalid, ignore input

            Dad.transform.position = BoatCarryBigLeftSidePosition;
            ONBoat.Add('D');

        }
    }

     void MoveMom()
    {
        JumpSound.Play();

        if (Mom.transform.position == MomRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            
            if (!CanBoard('M'))
                return;  // Boat full or invalid, ignore input

            Mom.transform.position = BoatCarryBigRightSidePosition;
            ONBoat.Add('M');
        }

        else if (Mom.transform.position == BoatCarryBigRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            Mom.transform.position = MomRightSidePosition;
            ONBoat.Remove('M');
        }

        else if (Mom.transform.position == BoatCarryBigLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
         
            Mom.transform.position = MomLeftSidePosition;
            ONBoat.Remove('M');
        }

        else if (Mom.transform.position == MomLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            if (!CanBoard('M'))
                return;  // Boat full or invalid, ignore input

            Mom.transform.position = BoatCarryBigLeftSidePosition;
            ONBoat.Add('M');
        }
        
    }

     void MoveBoy()
    {
        JumpSound.Play();

        if (Boy.transform.position == BoyRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            if (!CanBoard('B'))
                return;  // Boat full or invalid, ignore input

            Boy.transform.position = BoatCarryBoyRightSidePosition;
            ONBoat.Add('B');
        }
        else if (Boy.transform.position == BoatCarryBoyRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            ONBoat.Remove('B');
            Boy.transform.position = BoyRightSidePosition;

        }
        else if (Boy.transform.position == BoatCarryBoyLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            Boy.transform.position = BoyLeftSidePosition;
            ONBoat.Remove('B');
        }
        else if (Boy.transform.position == BoyLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            if (!CanBoard('B'))
                return;  // Boat full or invalid, ignore input

            ONBoat.Add('B');
            Boy.transform.position = BoatCarryBoyLeftSidePosition;
        }
        }

    void MoveGirl()
    {
        JumpSound.Play();

        if (Girl.transform.position == GirlRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            if (!CanBoard('G'))
                return;  // Boat full or invalid, ignore input

            ONBoat.Add('G');
            Girl.transform.position = BoatCarryGirlRightSidePosition;

        }
        else if (Girl.transform.position == BoatCarryGirlRightSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            ONBoat.Remove('G');
            Girl.transform.position = GirlRightSidePosition;

        }
        else if (Girl.transform.position == BoatCarryGirlLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            ONBoat.Remove('G');
            Girl.transform.position = GirlLeftSidePosition;
        }
        else if (Girl.transform.position == GirlLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            if (!CanBoard('G'))
                return;  // Boat full or invalid, ignore input

            ONBoat.Add('G');
            Girl.transform.position = BoatCarryGirlLeftSidePosition;
        }
        }

    void MoveBoat()
    {
        JumpSound.Play();

        if (Boat.transform.position == BoatRightSidePosition)
        {
            if ((Dad.transform.position == BoatCarryBigRightSidePosition))
            {
                Dad.transform.position = BoatCarryBigLeftSidePosition;
            }
            else if ((Mom.transform.position == BoatCarryBigRightSidePosition))
            {
                Mom.transform.position = BoatCarryBigLeftSidePosition;
            }
            else if (Boy.transform.position == BoatCarryBoyRightSidePosition && Girl.transform.position == BoatCarryGirlRightSidePosition)
            {
                Boy.transform.position = BoatCarryBoyLeftSidePosition;
                Girl.transform.position = BoatCarryGirlLeftSidePosition;
            }
            else if ((Boy.transform.position == BoatCarryBoyRightSidePosition))
            {
                Boy.transform.position = BoatCarryBoyLeftSidePosition;
            }
            else if ((Girl.transform.position == BoatCarryGirlRightSidePosition))
            {
                Girl.transform.position = BoatCarryGirlLeftSidePosition;
            }

            Boat.transform.position = BoatLeftSidePosition;
            Boat.transform.localScale = new Vector3((float)-1.535864, (float)1.535864, 0);
        }
        else if (Boat.transform.position == BoatLeftSidePosition)
        {
            if ((Dad.transform.position == BoatCarryBigLeftSidePosition))
            {
                Dad.transform.position = BoatCarryBigRightSidePosition;
            }
            else if ((Mom.transform.position == BoatCarryBigLeftSidePosition))
            {
                Mom.transform.position = BoatCarryBigRightSidePosition;
            }
            else if (Boy.transform.position == BoatCarryBoyLeftSidePosition && Girl.transform.position == BoatCarryGirlLeftSidePosition)
            {
                Boy.transform.position = BoatCarryBoyRightSidePosition;
                Girl.transform.position = BoatCarryGirlRightSidePosition;
            }
            else if ((Boy.transform.position == BoatCarryBoyLeftSidePosition))
            {
                Boy.transform.position = BoatCarryBoyRightSidePosition;
            }
            else if ((Girl.transform.position == BoatCarryGirlLeftSidePosition))
            {
                Girl.transform.position = BoatCarryGirlRightSidePosition;
            }

            Boat.transform.position = BoatRightSidePosition;
            Boat.transform.localScale = new Vector3((float)1.535864, (float)1.535864, 0);
        }
    }
    public void ResetGame()
    {
        SceneManager.LoadSceneAsync(2);
        WinState.SetActive(false);
        ButtonClick.Play();
    }


    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        WinState.SetActive(false);
        ButtonClick.Play();
    }
    public void StartLevel2()
    {
        HowToPlay.SetActive(false);
        ButtonClick.Play();
    }

    bool CanBoard(char character)
    {
        int bigCount = ONBoat.Count(c => c == 'D' || c == 'M');  // Big humans
        int smallCount = ONBoat.Count(c => c == 'B' || c == 'G'); // Small humans

        if (character == 'D' || character == 'M')
        {
            // Big human can board only if boat is empty
            return ONBoat.Count == 0;
        }
        else // small human
        {
            // Small human can board only if there is room (max 2 smalls) and no big humans on board
            return bigCount == 0 && smallCount < 2;
        }
    }

    void Update()
    {
        if (Boy.transform.position == BoyLeftSidePosition && Girl.transform.position == GirlLeftSidePosition && Dad.transform.position == DadLeftSidePosition && Mom.transform.position == MomLeftSidePosition)
        
        {
            WinState.SetActive(true);
            StopTimer();
            CalculateStars();
        }

        if (ONBoat.Count == 0)

        {
            GoButton.interactable = false;
        }

        else

        {
            GoButton.interactable = true;
        }

        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0; // clamp to zero
                isTimerRunning = false;
                UpdateTimerUI();
            }
            else
            {
                UpdateTimerUI();
            }
        }

        // Detect mouse clicks
        if (Input.GetMouseButtonDown(0)) // 0 = left click / tap
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == Dad)
                {
                    MoveDad();
                }
                else if (hit.collider.gameObject == Mom)
                {
                    MoveMom();
                }
                else if (hit.collider.gameObject == Boy)
                {
                    MoveBoy();
                }
                else if (hit.collider.gameObject == Girl)
                {
                    MoveGirl();
                }
            }
        }
    }

    public void Soundon()
    {
        Music.Play();
    }

    public void Soundoff()
    {
        Music.Stop();
    }
    public void Continue()
    {
        Pause.SetActive(false);
        ButtonClick.Play();
    }
    public void PauseMenu()
    {
        Pause.SetActive(true);
        ButtonClick.Play();
    }
        void CalculateStars()
{
    // Define thresholds for stars
    if (timeRemaining >= levelTime * 0.7f) // 70% or more time remaining
    {
        star1.SetActive(true);
        star2.SetActive(true);
        star3.SetActive(true);
        nostar1.SetActive(false);
        nostar2.SetActive(false);
        nostar3.SetActive(false);
        starScore = 3;
    }
    else if (timeRemaining >= levelTime * 0.4f) // Between 40% and 70% time remaining
    {
        star1.SetActive(true);
        star2.SetActive(true);
        star3.SetActive(false);
        nostar1.SetActive(false);
        nostar2.SetActive(false);
        starScore = 2;
    }
    else // Less than 40% time remaining
    {
        star1.SetActive(true);
        star2.SetActive(false);
        star3.SetActive(false);
        nostar1.SetActive(false);

        starScore = 1;
    }
}
}
