using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Level1RiverController : MonoBehaviour
{
    // Characters
    public GameObject Sheep;
    public GameObject Wolf;
    public GameObject Cabbage;
    public GameObject Boat;

    // Buttons

    public Button GoButton;
    public Button CabbagePlayAgain;
    public Button CabbageMainMenuButton;
    public Button WinPlayAgain;
    public Button WinMainMenuButton;
    public Button WolfPlayAgain;
    public Button WolfMainMenuButton;
    public Button PauseContinue;
    public Button PauseMainMenuButton;
    public Button PauseRestart;
    public Button SoundOn;
    public Button SoundOff;
    public Button PauseButton;
    public Button NextLevelButton;
    public Button StartLevel1Button;

    // Positions

    public Vector3 CabbageRightSidePosition = new Vector3((float)3.389479, (float)0.6573207, 0);
    public Vector3 CabbageLeftSidePosition = new Vector3((float)-3.389479, (float)0.6573207, 0);
    public Vector3 SheepRightSidePosition = new Vector3((float)5.232215, (float)0.6930565, 0);
    public Vector3 SheepLeftSidePosition = new Vector3((float)-5.232215, (float)0.6930565, 0);
    public Vector3 WolfRightSidePosition = new Vector3((float)6.975685, (float)0.8031513, 0);
    public Vector3 WolfLeftSidePosition = new Vector3((float)-6.975685, (float)0.8031513, 0);
    public Vector3 BoatCarryRightSidePosition = new Vector3((float)1.517, (float)0.654, 0);
    public Vector3 BoatCarryLeftSidePosition = new Vector3((float)-1.517, (float)0.654, 0);
    public Vector3 BoatRightSidePosition = new Vector3((float)1.182167, (float)0.1985318, 0);
    public Vector3 BoatLeftSidePosition = new Vector3((float)-1.182167, (float)0.1985318, 0);

    // Panels

    public GameObject WolfEatSheep;
    public GameObject CabbageEatSheep;
    public GameObject WinState;
    public GameObject Pause;
    public GameObject HowToPlay;

    // Sounds

    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] AudioSource JumpSound;

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

    private bool isBoatOccupied = false;

    void Start()
    {
        GoButton.onClick.AddListener(MoveBoat);
        CabbagePlayAgain.onClick.AddListener(ResetGame);
        CabbageMainMenuButton.onClick.AddListener(MainMenu);
        WinPlayAgain.onClick.AddListener(ResetGame);
        WinMainMenuButton.onClick.AddListener(MainMenu);
        WolfPlayAgain.onClick.AddListener(ResetGame);
        WolfMainMenuButton.onClick.AddListener(MainMenu);
        PauseMainMenuButton.onClick.AddListener(MainMenu);
        PauseRestart.onClick.AddListener(ResetGame);
        SoundOn.onClick.AddListener(Soundon);
        SoundOff.onClick.AddListener(Soundoff);
        PauseContinue.onClick.AddListener(Continue);
        PauseButton.onClick.AddListener(PauseMenu);
        NextLevelButton.onClick.AddListener(NextLevel);
        StartLevel1Button.onClick.AddListener(StartLevel1);
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

    void MoveSheep()
    {
        JumpSound.Play();

        // If boat already has something and it's not the Sheep, don't let Sheep move
        if (isBoatOccupied && Sheep.transform.position != BoatCarryRightSidePosition && Sheep.transform.position != BoatCarryLeftSidePosition)
            return;

        if (Sheep.transform.position == SheepRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Sheep.transform.position = BoatCarryRightSidePosition;
        else if (Sheep.transform.position == BoatCarryRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Sheep.transform.position = SheepRightSidePosition;
        else if (Sheep.transform.position == BoatCarryLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Sheep.transform.position = SheepLeftSidePosition;
        else if (Sheep.transform.position == SheepLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Sheep.transform.position = BoatCarryLeftSidePosition;

        // Update boat occupancy
        isBoatOccupied =
            Sheep.transform.position == BoatCarryRightSidePosition || Sheep.transform.position == BoatCarryLeftSidePosition;


    }

    void MoveWolf()
    {
        JumpSound.Play();

        // If boat already has something and it's not the wolf, don't let wolf move
        if (isBoatOccupied && Wolf.transform.position != BoatCarryRightSidePosition && Wolf.transform.position != BoatCarryLeftSidePosition)
            return;

        if (Wolf.transform.position == WolfRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Wolf.transform.position = BoatCarryRightSidePosition;
        else if (Wolf.transform.position == BoatCarryRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Wolf.transform.position = WolfRightSidePosition;
        else if (Wolf.transform.position == BoatCarryLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Wolf.transform.position = WolfLeftSidePosition;
        else if (Wolf.transform.position == WolfLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Wolf.transform.position = BoatCarryLeftSidePosition;

        // Update boat occupancy
        isBoatOccupied =
            Wolf.transform.position == BoatCarryRightSidePosition || Wolf.transform.position == BoatCarryLeftSidePosition;

    }

    void MoveCabbage()
    {
        JumpSound.Play();

        // If boat already has something and it's not the Cabbage, don't let Cabbage move
        if (isBoatOccupied && Cabbage.transform.position != BoatCarryRightSidePosition && Cabbage.transform.position != BoatCarryLeftSidePosition)
            return;

        if (Cabbage.transform.position == CabbageRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Cabbage.transform.position = BoatCarryRightSidePosition;
        else if (Cabbage.transform.position == BoatCarryRightSidePosition && Boat.transform.position == BoatRightSidePosition)
            Cabbage.transform.position = CabbageRightSidePosition;
        else if (Cabbage.transform.position == BoatCarryLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Cabbage.transform.position = CabbageLeftSidePosition;
        else if (Cabbage.transform.position == CabbageLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
            Cabbage.transform.position = BoatCarryLeftSidePosition;

        // Update boat occupancy
        isBoatOccupied =
            Cabbage.transform.position == BoatCarryRightSidePosition || Cabbage.transform.position == BoatCarryLeftSidePosition;

    }
    void MoveBoat()
    {
        JumpSound.Play();

        if (Boat.transform.position == BoatRightSidePosition)
        {
            if ((Sheep.transform.position == BoatCarryRightSidePosition))
            {
                Sheep.transform.position = BoatCarryLeftSidePosition;
                Sheep.transform.localScale = new Vector3((float)-2.256043, (float)2.256043, 0);
            }
            else if ((Wolf.transform.position == BoatCarryRightSidePosition))
            {
                Wolf.transform.position = BoatCarryLeftSidePosition;
                Wolf.transform.localScale = new Vector3((float)-1.804834, (float)1.804834, 0);
            }
            else if ((Cabbage.transform.position == BoatCarryRightSidePosition))
            {
                Cabbage.transform.position = BoatCarryLeftSidePosition;
                Cabbage.transform.localScale = new Vector3((float)-1.353626, (float)1.353626, 0);
            }
            Boat.transform.position = BoatLeftSidePosition;
            Boat.transform.localScale = new Vector3((float)-1.535864, (float)1.535864, 0);
        }
        else if (Boat.transform.position == BoatLeftSidePosition)
        {
            if ((Sheep.transform.position == BoatCarryLeftSidePosition))
            {
                Sheep.transform.position = BoatCarryRightSidePosition;
                Sheep.transform.localScale = new Vector3((float)2.256043, (float)2.256043, 0);
            }
            else if ((Wolf.transform.position == BoatCarryLeftSidePosition))
            {
                Wolf.transform.position = BoatCarryRightSidePosition;
                Wolf.transform.localScale = new Vector3((float)1.804834, (float)1.804834, 0);
            }
            else if ((Cabbage.transform.position == BoatCarryLeftSidePosition))
            {
                Cabbage.transform.position = BoatCarryRightSidePosition;
                Cabbage.transform.localScale = new Vector3((float)1.353626, (float)1.353626, 0);
            }
            Boat.transform.position = BoatRightSidePosition;
            Boat.transform.localScale = new Vector3((float)1.535864, (float)1.535864, 0);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadSceneAsync(1);
        WolfEatSheep.SetActive(false);
        CabbageEatSheep.SetActive(false);
        WinState.SetActive(false);
        ButtonClick.Play();
    }


    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        WolfEatSheep.SetActive(false);
        CabbageEatSheep.SetActive(false);
        WinState.SetActive(false);
        ButtonClick.Play();
    }
    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(2);
        WinState.SetActive(false);
        ButtonClick.Play();
    }
    public void StartLevel1()
    {
        HowToPlay.SetActive(false);
        ButtonClick.Play();
    }

    void Update()
    {
        if (Sheep.transform.position == SheepRightSidePosition && Wolf.transform.position == WolfRightSidePosition && Cabbage.transform.position == BoatCarryLeftSidePosition)
        {
            WolfEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepRightSidePosition && Wolf.transform.position == WolfRightSidePosition && Cabbage.transform.position == CabbageLeftSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            WolfEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepRightSidePosition && Wolf.transform.position == BoatCarryLeftSidePosition && Cabbage.transform.position == CabbageRightSidePosition)
        {
            CabbageEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepLeftSidePosition && Wolf.transform.position == WolfLeftSidePosition && Boat.transform.position == BoatRightSidePosition)
        {
            WolfEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepLeftSidePosition && Boat.transform.position == BoatRightSidePosition && Cabbage.transform.position == CabbageLeftSidePosition)
        {
            CabbageEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepRightSidePosition && Wolf.transform.position == WolfLeftSidePosition && Cabbage.transform.position == CabbageRightSidePosition && Boat.transform.position == BoatLeftSidePosition)
        {
            CabbageEatSheep.SetActive(true);
            StopTimer();
        }

        if (Sheep.transform.position == SheepLeftSidePosition && Wolf.transform.position == WolfLeftSidePosition && Cabbage.transform.position == CabbageLeftSidePosition)
        {
            WinState.SetActive(true);
            StopTimer();
            CalculateStars();
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
                if (hit.collider.gameObject == Sheep)
                {
                    MoveSheep();
                }
                else if (hit.collider.gameObject == Wolf)
                {
                    MoveWolf();
                }
                else if (hit.collider.gameObject == Cabbage)
                {
                    MoveCabbage();
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