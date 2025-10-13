//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using TMPro;
//using System;

//public class CannibalsMissionariesController : MonoBehaviour
//{
//    // Characters (3 versions each - assign these in Inspector)
//    [Header("Missionary 1 Versions")]
//    public GameObject Missionary1Left;
//    public GameObject Missionary1OnBoat;
//    public GameObject Missionary1Right;

//    [Header("Missionary 2 Versions")]
//    public GameObject Missionary2Left;
//    public GameObject Missionary2OnBoat;
//    public GameObject Missionary2Right;

//    [Header("Missionary 3 Versions")]
//    public GameObject Missionary3Left;
//    public GameObject Missionary3OnBoat;
//    public GameObject Missionary3Right;

//    [Header("Cannibal 1 Versions")]
//    public GameObject Cannibal1Left;
//    public GameObject Cannibal1OnBoat;
//    public GameObject Cannibal1Right;

//    [Header("Cannibal 2 Versions")]
//    public GameObject Cannibal2Left;
//    public GameObject Cannibal2OnBoat;
//    public GameObject Cannibal2Right;

//    [Header("Cannibal 3 Versions")]
//    public GameObject Cannibal3Left;
//    public GameObject Cannibal3OnBoat;
//    public GameObject Cannibal3Right;

//    [Header("Boat")]
//    public GameObject Boat;
//    public Transform BoatCharactersParent;

//    [Header("Particle Effects")]
//    public ParticleSystem BoatParticleEffect1;
//    public ParticleSystem BoatParticleEffect2;
//    public ParticleSystem BoatParticleEffect3;

//    // Game Buttons
//    public Button GoButton;
//    public Button WinPlayAgain;
//    public Button WinMainMenuButton;
//    public Button LosePlayAgain;
//    public Button LoseMainMenuButton;
//    public Button PauseContinue;
//    public Button PauseMainMenuButton;
//    public Button PauseRestart;
//    public Button SoundOn;
//    public Button SoundOff;
//    public Button PauseButton;
//    public Button StartLevelButton;

//    // Boat Positions
//    [Header("Boat Positions")]
//    public GameObject BoatLeftPosition;
//    public GameObject BoatRightPosition;

//    // Panels
//    public GameObject WinState;
//    public GameObject LoseState;
//    public GameObject Pause;
//    public GameObject HowToPlay;

//    // UI Text
//    public TextMeshProUGUI ErrorText;
//    public TextMeshProUGUI MovesCountText;

//    // Sound
//    [SerializeField] AudioSource Music;
//    [SerializeField] AudioSource ButtonClick;
//    [SerializeField] AudioSource JumpSound;
//    [SerializeField] AudioSource WinSound;
//    [SerializeField] AudioSource LoseSound;

//    // Game Variables
//    public List<char> ONBoat = new List<char>();
//    public Slider progressSlider;
//    public GameObject[] stars;
//    public GameObject[] nostars;
//    public int starScore;

//    private bool isBoatMoving = false;
//    private bool isBoatOnRightSide = true;
//    private int moveCount = 0;

//    void Start()
//    {
//        SetInitialPositions();

//        // Button listeners
//        GoButton.onClick.AddListener(MoveBoat);
//        WinPlayAgain.onClick.AddListener(ResetGame);
//        WinMainMenuButton.onClick.AddListener(MainMenu);
//        LosePlayAgain.onClick.AddListener(ResetGame);
//        LoseMainMenuButton.onClick.AddListener(MainMenu);
//        PauseMainMenuButton.onClick.AddListener(MainMenu);
//        PauseRestart.onClick.AddListener(ResetGame);
//        SoundOn.onClick.AddListener(() => Music.Play());
//        SoundOff.onClick.AddListener(() => Music.Stop());
//        PauseContinue.onClick.AddListener(() => Pause.SetActive(false));
//        PauseButton.onClick.AddListener(() => Pause.SetActive(true));
//        StartLevelButton.onClick.AddListener(() => HowToPlay.SetActive(false));

//        if (ErrorText != null) ErrorText.text = "";
//        UpdateProgress();
//        UpdateMovesCount();
//    }

//    void SetInitialPositions()
//    {
//        // Set all characters to right side initially (starting shore)
//        SetCharacterState('1', CharacterState.Right); // Missionary 1
//        SetCharacterState('2', CharacterState.Right); // Missionary 2
//        SetCharacterState('3', CharacterState.Right); // Missionary 3
//        SetCharacterState('4', CharacterState.Right); // Cannibal 1
//        SetCharacterState('5', CharacterState.Right); // Cannibal 2
//        SetCharacterState('6', CharacterState.Right); // Cannibal 3

//        // Turn off particle effects
//        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
//        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
//        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

//        ONBoat.Clear();
//        isBoatOnRightSide = true;
//    }

//    enum CharacterState { Left, Boat, Right }

//    void SetCharacterState(char person, CharacterState state)
//    {
//        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

//        if (left == null) return;

//        left.SetActive(false);
//        boat.SetActive(false);
//        right.SetActive(false);

//        switch (state)
//        {
//            case CharacterState.Left:
//                left.SetActive(true);
//                break;
//            case CharacterState.Boat:
//                boat.SetActive(true);
//                break;
//            case CharacterState.Right:
//                right.SetActive(true);
//                break;
//        }
//    }

//    (GameObject, GameObject, GameObject) GetCharacterVersions(char person)
//    {
//        switch (person)
//        {
//            case '1': return (Missionary1Left, Missionary1OnBoat, Missionary1Right);
//            case '2': return (Missionary2Left, Missionary2OnBoat, Missionary2Right);
//            case '3': return (Missionary3Left, Missionary3OnBoat, Missionary3Right);
//            case '4': return (Cannibal1Left, Cannibal1OnBoat, Cannibal1Right);
//            case '5': return (Cannibal2Left, Cannibal2OnBoat, Cannibal2Right);
//            case '6': return (Cannibal3Left, Cannibal3OnBoat, Cannibal3Right);
//            default: return (null, null, null);
//        }
//    }

//    CharacterState GetCharacterCurrentState(char person)
//    {
//        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

//        if (left == null) return CharacterState.Right;
//        if (left.activeInHierarchy) return CharacterState.Left;
//        if (boat.activeInHierarchy) return CharacterState.Boat;
//        if (right.activeInHierarchy) return CharacterState.Right;

//        return CharacterState.Right;
//    }

//    bool IsMissionary(char person)
//    {
//        return person == '1' || person == '2' || person == '3';
//    }

//    void MovePerson(char person)
//    {
//        if (isBoatMoving) return;

//        JumpSound.Play();
//        CharacterState currentState = GetCharacterCurrentState(person);

//        switch (currentState)
//        {
//            case CharacterState.Right when isBoatOnRightSide:
//                if (CanBoard(person))
//                {
//                    SetCharacterState(person, CharacterState.Boat);
//                    ONBoat.Add(person);
//                }
//                break;

//            case CharacterState.Boat when isBoatOnRightSide:
//                SetCharacterState(person, CharacterState.Right);
//                ONBoat.Remove(person);
//                break;

//            case CharacterState.Left when !isBoatOnRightSide:
//                if (CanBoard(person))
//                {
//                    SetCharacterState(person, CharacterState.Boat);
//                    ONBoat.Add(person);
//                }
//                break;

//            case CharacterState.Boat when !isBoatOnRightSide:
//                SetCharacterState(person, CharacterState.Left);
//                ONBoat.Remove(person);
//                break;
//        }

//        UpdateProgress();
//    }

//    void MoveBoat()
//    {
//        if (isBoatMoving || ONBoat.Count == 0) return;

//        // Check if the move will cause a lose condition
//        if (!IsValidMove()) return;

//        JumpSound.Play();
//        StartCoroutine(MoveBoatSmoothly());
//    }

//    bool IsValidMove()
//    {
//        // Simulate the move and check both sides
//        int missionariesLeft = 0, cannibalsLeft = 0;
//        int missionariesRight = 0, cannibalsRight = 0;

//        // Count current positions
//        for (char c = '1'; c <= '6'; c++)
//        {
//            CharacterState state = GetCharacterCurrentState(c);
//            bool isMiss = IsMissionary(c);

//            if (state == CharacterState.Left)
//            {
//                if (isMiss) missionariesLeft++;
//                else cannibalsLeft++;
//            }
//            else if (state == CharacterState.Right)
//            {
//                if (isMiss) missionariesRight++;
//                else cannibalsRight++;
//            }
//        }

//        // Simulate boat arrival at destination
//        foreach (char person in ONBoat)
//        {
//            bool isMiss = IsMissionary(person);
//            if (isBoatOnRightSide) // Moving to left
//            {
//                if (isMiss) missionariesLeft++;
//                else cannibalsLeft++;
//            }
//            else // Moving to right
//            {
//                if (isMiss) missionariesRight++;
//                else cannibalsRight++;
//            }
//        }

//        // Check validity: missionaries must not be outnumbered (unless there are 0 missionaries)
//        bool leftValid = missionariesLeft == 0 || missionariesLeft >= cannibalsLeft;
//        bool rightValid = missionariesRight == 0 || missionariesRight >= cannibalsRight;

//        return leftValid && rightValid;
//    }

//    IEnumerator MoveBoatSmoothly()
//    {
//        isBoatMoving = true;
//        moveCount++;
//        UpdateMovesCount();

//        if (BoatParticleEffect1 != null) BoatParticleEffect1.Play();
//        if (BoatParticleEffect2 != null) BoatParticleEffect2.Play();
//        if (BoatParticleEffect3 != null) BoatParticleEffect3.Play();

//        GameObject targetPosition = isBoatOnRightSide ? BoatLeftPosition : BoatRightPosition;
//        Vector3 targetPos = targetPosition.transform.position;
//        Vector3 startPos = Boat.transform.position;

//        Quaternion targetRotation = isBoatOnRightSide ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
//        Boat.transform.rotation = targetRotation;

//        float t = 0f;
//        while (t <= 1f)
//        {
//            t += Time.deltaTime * 0.5f;
//            Boat.transform.position = Vector3.Lerp(startPos, targetPos, t);
//            yield return null;
//        }

//        Boat.transform.position = targetPos;
//        isBoatOnRightSide = !isBoatOnRightSide;
//        isBoatMoving = false;

//        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
//        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
//        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

//        UpdateProgress();

//        // Check for lose condition after boat arrives
//        if (!CheckCurrentStateValid())
//        {
//            yield return new WaitForSeconds(0.5f);
//            GameOver();
//        }
//    }

//    bool CheckCurrentStateValid()
//    {
//        int missionariesLeft = 0, cannibalsLeft = 0;
//        int missionariesRight = 0, cannibalsRight = 0;

//        for (char c = '1'; c <= '6'; c++)
//        {
//            CharacterState state = GetCharacterCurrentState(c);
//            bool isMiss = IsMissionary(c);

//            if (state == CharacterState.Left)
//            {
//                if (isMiss) missionariesLeft++;
//                else cannibalsLeft++;
//            }
//            else if (state == CharacterState.Right)
//            {
//                if (isMiss) missionariesRight++;
//                else cannibalsRight++;
//            }
//        }

//        bool leftValid = missionariesLeft == 0 || missionariesLeft >= cannibalsLeft;
//        bool rightValid = missionariesRight == 0 || missionariesRight >= cannibalsRight;

//        return leftValid && rightValid;
//    }

//    bool CanBoard(char person)
//    {
//        return ONBoat.Count < 2; // Max 2 people on boat
//    }

//    void UpdateProgress()
//    {
//        int charactersOnLeft = 0;

//        for (char c = '1'; c <= '6'; c++)
//        {
//            if (GetCharacterCurrentState(c) == CharacterState.Left)
//                charactersOnLeft++;
//        }

//        if (progressSlider != null)
//            progressSlider.value = charactersOnLeft / 6f;

//        GoButton.interactable = ONBoat.Count > 0 && !isBoatMoving;

//        if (charactersOnLeft == 6)
//        {
//            WinGame();
//        }
//    }

//    void UpdateMovesCount()
//    {
//        if (MovesCountText != null)
//        {
//            MovesCountText.text = "Moves: " + moveCount;
//        }
//    }

//    void WinGame()
//    {
//        if (WinSound != null) WinSound.Play();
//        WinState.SetActive(true);
//        CalculateStars();
//    }

//    void GameOver()
//    {
//        if (LoseSound != null) LoseSound.Play();
//        if (LoseState != null) LoseState.SetActive(true);
//    }

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0) && !isBoatMoving)
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit))
//            {
//                GameObject clickedObject = hit.collider.gameObject;

//                // Check all missionary versions
//                if (clickedObject == Missionary1Left || clickedObject == Missionary1OnBoat || clickedObject == Missionary1Right)
//                    MovePerson('1');
//                else if (clickedObject == Missionary2Left || clickedObject == Missionary2OnBoat || clickedObject == Missionary2Right)
//                    MovePerson('2');
//                else if (clickedObject == Missionary3Left || clickedObject == Missionary3OnBoat || clickedObject == Missionary3Right)
//                    MovePerson('3');
//                // Check all cannibal versions
//                else if (clickedObject == Cannibal1Left || clickedObject == Cannibal1OnBoat || clickedObject == Cannibal1Right)
//                    MovePerson('4');
//                else if (clickedObject == Cannibal2Left || clickedObject == Cannibal2OnBoat || clickedObject == Cannibal2Right)
//                    MovePerson('5');
//                else if (clickedObject == Cannibal3Left || clickedObject == Cannibal3OnBoat || clickedObject == Cannibal3Right)
//                    MovePerson('6');
//            }
//        }
//    }

//    public void ResetGame()
//    {
//        moveCount = 0;
//        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
//        ButtonClick.Play();
//    }

//    public void MainMenu()
//    {
//        SceneManager.LoadSceneAsync(0);
//        ButtonClick.Play();
//    }

//    void CalculateStars()
//    {
//        // Give 3 stars for completion
//        for (int i = 0; i < stars.Length; i++)
//        {
//            stars[i].SetActive(true);
//            if (i < nostars.Length) nostars[i].SetActive(false);
//        }
//        starScore = 3;
//    }
//}