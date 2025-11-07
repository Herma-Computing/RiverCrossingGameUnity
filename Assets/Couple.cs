using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Couple : MonoBehaviour
{
    // Characters (3 versions each - assign these in Inspector)
    [Header("Husband 1 Versions")]
    public GameObject Husband1Left;
    public GameObject Husband1OnBoat;
    public GameObject Husband1Right;

    [Header("Wife 1 Versions")]
    public GameObject Wife1Left;
    public GameObject Wife1OnBoat;
    public GameObject Wife1Right;

    [Header("Husband 2 Versions")]
    public GameObject Husband2Left;
    public GameObject Husband2OnBoat;
    public GameObject Husband2Right;

    [Header("Wife 2 Versions")]
    public GameObject Wife2Left;
    public GameObject Wife2OnBoat;
    public GameObject Wife2Right;

    [Header("Husband 3 Versions")]
    public GameObject Husband3Left;
    public GameObject Husband3OnBoat;
    public GameObject Husband3Right;

    [Header("Wife 3 Versions")]
    public GameObject Wife3Left;
    public GameObject Wife3OnBoat;
    public GameObject Wife3Right;

    [Header("Boat")]
    public GameObject Boat;
    public Transform BoatCharactersParent;

    [Header("Particle Effects")]
    public ParticleSystem BoatParticleEffect1;
    public ParticleSystem BoatParticleEffect2;
    public ParticleSystem BoatParticleEffect3;

    // Game Buttons
    public Button GoButton;
    public Button WinPlayAgain;
    public Button WinMainMenuButton;
    public Button LosePlayAgain;
    public Button LoseMainMenuButton;
    public Button PauseContinue;
    public Button PauseMainMenuButton;
    public Button PauseRestart;
    public Button SoundOn;
    public Button SoundOff;
    public Button PauseButton;
    public Button StartLevelButton;

    // Boat Positions
    [Header("Boat Positions")]
    public GameObject BoatLeftPosition;
    public GameObject BoatRightPosition;

    // Panels
    public GameObject WinState;
    public GameObject LoseState;
    public GameObject Pause;
    public GameObject HowToPlay;

    // UI Text
    public TextMeshProUGUI ErrorText;
    public TextMeshProUGUI MovesCountText;

    // Sound
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] AudioSource JumpSound;
    [SerializeField] AudioSource WinSound;
    [SerializeField] AudioSource LoseSound;

    // Game Variables
    public List<char> ONBoat = new List<char>();
    public Slider progressSlider;
    public GameObject[] stars;
    public GameObject[] nostars;
    public int starScore;

    private bool isBoatMoving = false;
    private bool isBoatOnRightSide = true;
    private int moveCount = 0;

    void Start()
    {
        SetInitialPositions();

        // Button listeners
        GoButton.onClick.AddListener(MoveBoat);
        WinPlayAgain.onClick.AddListener(ResetGame);
        WinMainMenuButton.onClick.AddListener(MainMenu);
        LosePlayAgain.onClick.AddListener(ResetGame);
        LoseMainMenuButton.onClick.AddListener(MainMenu);
        PauseMainMenuButton.onClick.AddListener(MainMenu);
        PauseRestart.onClick.AddListener(ResetGame);
        SoundOn.onClick.AddListener(() => Music.Play());
        SoundOff.onClick.AddListener(() => Music.Stop());
        PauseContinue.onClick.AddListener(() => Pause.SetActive(false));
        PauseButton.onClick.AddListener(() => Pause.SetActive(true));
        StartLevelButton.onClick.AddListener(() => HowToPlay.SetActive(false));

        if (ErrorText != null) ErrorText.text = "";
        UpdateProgress();
        UpdateMovesCount();
    }

    void SetInitialPositions()
    {
        // Set all characters to right side initially (starting shore)
        SetCharacterState('H', 1, CharacterState.Right); // Husband 1
        SetCharacterState('W', 1, CharacterState.Right); // Wife 1
        SetCharacterState('H', 2, CharacterState.Right); // Husband 2
        SetCharacterState('W', 2, CharacterState.Right); // Wife 2
        SetCharacterState('H', 3, CharacterState.Right); // Husband 3
        SetCharacterState('W', 3, CharacterState.Right); // Wife 3

        // Turn off particle effects
        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

        ONBoat.Clear();
        isBoatOnRightSide = true;
    }

    enum CharacterState { Left, Boat, Right }

    void SetCharacterState(char type, int coupleNumber, CharacterState state)
    {
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(type, coupleNumber);

        if (left == null) return;

        left.SetActive(false);
        boat.SetActive(false);
        right.SetActive(false);

        switch (state)
        {
            case CharacterState.Left:
                left.SetActive(true);
                break;
            case CharacterState.Boat:
                boat.SetActive(true);
                break;
            case CharacterState.Right:
                right.SetActive(true);
                break;
        }
    }

    (GameObject, GameObject, GameObject) GetCharacterVersions(char type, int coupleNumber)
    {
        if (type == 'H') // Husband
        {
            switch (coupleNumber)
            {
                case 1: return (Husband1Left, Husband1OnBoat, Husband1Right);
                case 2: return (Husband2Left, Husband2OnBoat, Husband2Right);
                case 3: return (Husband3Left, Husband3OnBoat, Husband3Right);
            }
        }
        else if (type == 'W') // Wife
        {
            switch (coupleNumber)
            {
                case 1: return (Wife1Left, Wife1OnBoat, Wife1Right);
                case 2: return (Wife2Left, Wife2OnBoat, Wife2Right);
                case 3: return (Wife3Left, Wife3OnBoat, Wife3Right);
            }
        }
        return (null, null, null);
    }

    CharacterState GetCharacterCurrentState(char type, int coupleNumber)
    {
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(type, coupleNumber);

        if (left == null) return CharacterState.Right;
        if (left.activeInHierarchy) return CharacterState.Left;
        if (boat.activeInHierarchy) return CharacterState.Boat;
        if (right.activeInHierarchy) return CharacterState.Right;

        return CharacterState.Right;
    }

    void MovePerson(char type, int coupleNumber)
    {
        if (isBoatMoving) return;

        JumpSound.Play();
        CharacterState currentState = GetCharacterCurrentState(type, coupleNumber);

        switch (currentState)
        {
            case CharacterState.Right when isBoatOnRightSide:
                if (CanBoard())
                {
                    SetCharacterState(type, coupleNumber, CharacterState.Boat);
                    ONBoat.Add(type == 'H' ? (char)('0' + coupleNumber) : (char)('3' + coupleNumber));
                }
                break;

            case CharacterState.Boat when isBoatOnRightSide:
                SetCharacterState(type, coupleNumber, CharacterState.Right);
                ONBoat.Remove(type == 'H' ? (char)('0' + coupleNumber) : (char)('3' + coupleNumber));
                break;

            case CharacterState.Left when !isBoatOnRightSide:
                if (CanBoard())
                {
                    SetCharacterState(type, coupleNumber, CharacterState.Boat);
                    ONBoat.Add(type == 'H' ? (char)('0' + coupleNumber) : (char)('3' + coupleNumber));
                }
                break;

            case CharacterState.Boat when !isBoatOnRightSide:
                SetCharacterState(type, coupleNumber, CharacterState.Left);
                ONBoat.Remove(type == 'H' ? (char)('0' + coupleNumber) : (char)('3' + coupleNumber));
                break;
        }

        UpdateProgress();
    }

    void MoveBoat()
    {
        if (isBoatMoving || ONBoat.Count == 0) return;

        // Check if the move will cause a lose condition
        if (!IsValidMove())
        {
            if (ErrorText != null)
            {
                ErrorText.text = "Invalid move! A wife cannot be left with another husband!";
                StartCoroutine(ClearErrorText());
            }
            return;
        }

        JumpSound.Play();
        StartCoroutine(MoveBoatSmoothly());
    }

    IEnumerator ClearErrorText()
    {
        yield return new WaitForSeconds(2f);
        if (ErrorText != null) ErrorText.text = "";
    }

    bool IsValidMove()
    {
        // Check the state after the boat moves
        List<int> husbandsAtDestination = new List<int>();
        List<int> wivesAtDestination = new List<int>();
        List<int> husbandsAtOrigin = new List<int>();
        List<int> wivesAtOrigin = new List<int>();

        // Determine who will be at each location after the move
        for (int i = 1; i <= 3; i++)
        {
            CharacterState husbandState = GetCharacterCurrentState('H', i);
            CharacterState wifeState = GetCharacterCurrentState('W', i);

            bool husbandOnBoat = ONBoat.Contains((char)('0' + i));
            bool wifeOnBoat = ONBoat.Contains((char)('3' + i));

            // After boat moves
            if (isBoatOnRightSide) // Moving to left
            {
                // Destination (left side)
                if (husbandState == CharacterState.Left || husbandOnBoat)
                    husbandsAtDestination.Add(i);
                if (wifeState == CharacterState.Left || wifeOnBoat)
                    wivesAtDestination.Add(i);

                // Origin (right side)
                if (husbandState == CharacterState.Right && !husbandOnBoat)
                    husbandsAtOrigin.Add(i);
                if (wifeState == CharacterState.Right && !wifeOnBoat)
                    wivesAtOrigin.Add(i);
            }
            else // Moving to right
            {
                // Destination (right side)
                if (husbandState == CharacterState.Right || husbandOnBoat)
                    husbandsAtDestination.Add(i);
                if (wifeState == CharacterState.Right || wifeOnBoat)
                    wivesAtDestination.Add(i);

                // Origin (left side)
                if (husbandState == CharacterState.Left && !husbandOnBoat)
                    husbandsAtOrigin.Add(i);
                if (wifeState == CharacterState.Left && !wifeOnBoat)
                    wivesAtOrigin.Add(i);
            }
        }

        // Check if any wife is with a husband who is not her own
        return IsConfigurationValid(husbandsAtDestination, wivesAtDestination) &&
               IsConfigurationValid(husbandsAtOrigin, wivesAtOrigin);
    }

    bool IsConfigurationValid(List<int> husbands, List<int> wives)
    {
        // For each wife, check if she's alone with another husband
        foreach (int wife in wives)
        {
            // If her husband is not present
            if (!husbands.Contains(wife))
            {
                // If there are any other husbands present, it's invalid
                if (husbands.Count > 0)
                    return false;
            }
        }
        return true;
    }

    IEnumerator MoveBoatSmoothly()
    {
        isBoatMoving = true;
        moveCount++;
        UpdateMovesCount();

        if (BoatParticleEffect1 != null) BoatParticleEffect1.Play();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Play();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Play();

        GameObject targetPosition = isBoatOnRightSide ? BoatLeftPosition : BoatRightPosition;
        Vector3 targetPos = targetPosition.transform.position;
        Vector3 startPos = Boat.transform.position;

        Quaternion targetRotation = isBoatOnRightSide ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
        Boat.transform.rotation = targetRotation;

        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime * 0.5f;
            Boat.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Boat.transform.position = targetPos;
        isBoatOnRightSide = !isBoatOnRightSide;
        isBoatMoving = false;

        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

        UpdateProgress();

        // Check for lose condition after boat arrives
        if (!CheckCurrentStateValid())
        {
            yield return new WaitForSeconds(0.5f);
            GameOver();
        }
    }

    bool CheckCurrentStateValid()
    {
        List<int> husbandsLeft = new List<int>();
        List<int> wivesLeft = new List<int>();
        List<int> husbandsRight = new List<int>();
        List<int> wivesRight = new List<int>();

        for (int i = 1; i <= 3; i++)
        {
            CharacterState husbandState = GetCharacterCurrentState('H', i);
            CharacterState wifeState = GetCharacterCurrentState('W', i);

            if (husbandState == CharacterState.Left) husbandsLeft.Add(i);
            if (wifeState == CharacterState.Left) wivesLeft.Add(i);
            if (husbandState == CharacterState.Right) husbandsRight.Add(i);
            if (wifeState == CharacterState.Right) wivesRight.Add(i);
        }

        return IsConfigurationValid(husbandsLeft, wivesLeft) &&
               IsConfigurationValid(husbandsRight, wivesRight);
    }

    bool CanBoard()
    {
        return ONBoat.Count < 2; // Max 2 people on boat
    }

    void UpdateProgress()
    {
        int charactersOnLeft = 0;

        for (int i = 1; i <= 3; i++)
        {
            if (GetCharacterCurrentState('H', i) == CharacterState.Left)
                charactersOnLeft++;
            if (GetCharacterCurrentState('W', i) == CharacterState.Left)
                charactersOnLeft++;
        }

        if (progressSlider != null)
            progressSlider.value = charactersOnLeft / 6f;

        GoButton.interactable = ONBoat.Count > 0 && !isBoatMoving;

        if (charactersOnLeft == 6)
        {
            WinGame();
        }
    }

    void UpdateMovesCount()
    {
        if (MovesCountText != null)
        {
            MovesCountText.text = "Moves: " + moveCount;
        }
    }

    void WinGame()
    {
        if (WinSound != null) WinSound.Play();
        WinState.SetActive(true);
        CalculateStars();
    }

    void GameOver()
    {
        if (LoseSound != null) LoseSound.Play();
        if (LoseState != null) LoseState.SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isBoatMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked on: " + clickedObject.name);

                // Check Husband 1
                if (clickedObject == Husband1Left || clickedObject == Husband1OnBoat || clickedObject == Husband1Right)
                {
                    Debug.Log("Moving Husband 1");
                    MovePerson('H', 1);
                }
                // Check Wife 1
                else if (clickedObject == Wife1Left || clickedObject == Wife1OnBoat || clickedObject == Wife1Right)
                {
                    Debug.Log("Moving Wife 1");
                    MovePerson('W', 1);
                }
                // Check Husband 2
                else if (clickedObject == Husband2Left || clickedObject == Husband2OnBoat || clickedObject == Husband2Right)
                {
                    Debug.Log("Moving Husband 2");
                    MovePerson('H', 2);
                }
                // Check Wife 2
                else if (clickedObject == Wife2Left || clickedObject == Wife2OnBoat || clickedObject == Wife2Right)
                {
                    Debug.Log("Moving Wife 2");
                    MovePerson('W', 2);
                }
                // Check Husband 3
                else if (clickedObject == Husband3Left || clickedObject == Husband3OnBoat || clickedObject == Husband3Right)
                {
                    Debug.Log("Moving Husband 3");
                    MovePerson('H', 3);
                }
                // Check Wife 3
                else if (clickedObject == Wife3Left || clickedObject == Wife3OnBoat || clickedObject == Wife3Right)
                {
                    Debug.Log("Moving Wife 3");
                    MovePerson('W', 3);
                }
                else
                {
                    Debug.Log("Clicked object not recognized!");
                }
            }
            else
            {
                Debug.Log("Raycast hit nothing");
            }
        }
    }

    public void ResetGame()
    {
        moveCount = 0;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        ButtonClick.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        ButtonClick.Play();
    }

    void CalculateStars()
    {
        // Give stars based on move count
        if (moveCount <= 11) // Optimal solution
        {
            starScore = 3;
        }
        else if (moveCount <= 15)
        {
            starScore = 2;
        }
        else
        {
            starScore = 1;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starScore)
            {
                stars[i].SetActive(true);
                if (i < nostars.Length) nostars[i].SetActive(false);
            }
            else
            {
                stars[i].SetActive(false);
                if (i < nostars.Length) nostars[i].SetActive(true);
            }
        }
    }
}