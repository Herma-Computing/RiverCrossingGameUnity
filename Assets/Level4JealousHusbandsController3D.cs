using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Level4JealousHusbandsController3D : MonoBehaviour
{
    // Characters (3 versions each - assign these in Inspector)
    [Header("Couple 1 - Husband 1")]
    public GameObject Husband1Left;
    public GameObject Husband1OnBoat;
    public GameObject Husband1Right;

    [Header("Couple 1 - Wife 1")]
    public GameObject Wife1Left;
    public GameObject Wife1OnBoat;
    public GameObject Wife1Right;

    [Header("Couple 2 - Husband 2")]
    public GameObject Husband2Left;
    public GameObject Husband2OnBoat;
    public GameObject Husband2Right;

    [Header("Couple 2 - Wife 2")]
    public GameObject Wife2Left;
    public GameObject Wife2OnBoat;
    public GameObject Wife2Right;

    [Header("Couple 3 - Husband 3")]
    public GameObject Husband3Left;
    public GameObject Husband3OnBoat;
    public GameObject Husband3Right;

    [Header("Couple 3 - Wife 3")]
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
    public Button PauseContinue;
    public Button PauseMainMenuButton;
    public Button PauseRestart;
    public Button SoundOn;
    public Button SoundOff;
    public Button PauseButton;
    public Button StartLevel4Button;

    // Boat Positions
    [Header("Boat Positions")]
    public GameObject BoatLeftPosition;
    public GameObject BoatRightPosition;

    // Panels
    public GameObject WinState;
    public GameObject Pause;
    public GameObject HowToPlay;
    public GameObject GameOverPanel; // New panel for rule violation

    // UI Text for error messages
    public TextMeshProUGUI ErrorMessageText;

    // Sound
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] AudioSource JumpSound;
    [SerializeField] AudioSource BoatMoveSound;
    [SerializeField] AudioSource ErrorSound;

    // Game Variables
    public List<char> ONBoat = new List<char>();
    public Slider progressSlider;
    public GameObject[] stars;
    public GameObject[] nostars;
    public int starScore;
    public int moveCount = 0;
    public TextMeshProUGUI moveCounterText;

    private bool isBoatMoving = false;
    private bool isBoatOnRightSide = true;

    // Character codes: H1=Husband1, W1=Wife1, H2=Husband2, W2=Wife2, H3=Husband3, W3=Wife3

    void Start()
    {
        SetInitialPositions();

        // Button listeners
        GoButton.onClick.AddListener(MoveBoat);
        WinPlayAgain.onClick.AddListener(ResetGame);
        WinMainMenuButton.onClick.AddListener(MainMenu);
        PauseMainMenuButton.onClick.AddListener(MainMenu);
        PauseRestart.onClick.AddListener(ResetGame);
        SoundOn.onClick.AddListener(() => Music.Play());
        SoundOff.onClick.AddListener(() => Music.Stop());
        PauseContinue.onClick.AddListener(() => Pause.SetActive(false));
        PauseButton.onClick.AddListener(() => Pause.SetActive(true));
        StartLevel4Button.onClick.AddListener(() => HowToPlay.SetActive(false));

        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        UpdateProgress();
        UpdateMoveCounter();
    }

    void SetInitialPositions()
    {
        // Set all characters to right side initially
        SetCharacterState("H1", CharacterState.Right);
        SetCharacterState("W1", CharacterState.Right);
        SetCharacterState("H2", CharacterState.Right);
        SetCharacterState("W2", CharacterState.Right);
        SetCharacterState("H3", CharacterState.Right);
        SetCharacterState("W3", CharacterState.Right);

        // Stop all particle effects
        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

        ONBoat.Clear();
        isBoatOnRightSide = true;
        moveCount = 0;
    }

    enum CharacterState { Left, Boat, Right }

    void SetCharacterState(string person, CharacterState state)
    {
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

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

    (GameObject, GameObject, GameObject) GetCharacterVersions(string person)
    {
        switch (person)
        {
            case "H1": return (Husband1Left, Husband1OnBoat, Husband1Right);
            case "W1": return (Wife1Left, Wife1OnBoat, Wife1Right);
            case "H2": return (Husband2Left, Husband2OnBoat, Husband2Right);
            case "W2": return (Wife2Left, Wife2OnBoat, Wife2Right);
            case "H3": return (Husband3Left, Husband3OnBoat, Husband3Right);
            case "W3": return (Wife3Left, Wife3OnBoat, Wife3Right);
            default: return (null, null, null);
        }
    }

    CharacterState GetCharacterCurrentState(string person)
    {
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

        if (left != null && left.activeInHierarchy) return CharacterState.Left;
        if (boat != null && boat.activeInHierarchy) return CharacterState.Boat;
        if (right != null && right.activeInHierarchy) return CharacterState.Right;

        return CharacterState.Right;
    }

    void MovePerson(string person)
    {
        if (isBoatMoving) return;

        JumpSound.Play();
        CharacterState currentState = GetCharacterCurrentState(person);

        switch (currentState)
        {
            case CharacterState.Right when isBoatOnRightSide:
                if (CanBoard(person))
                {
                    SetCharacterState(person, CharacterState.Boat);
                    ONBoat.Add(person[0]); // Store first character (H or W)
                }
                else
                {
                    ShowError("Boat is full! Maximum 2 people.");
                }
                break;

            case CharacterState.Boat when isBoatOnRightSide:
                SetCharacterState(person, CharacterState.Right);
                ONBoat.Remove(person[0]);
                break;

            case CharacterState.Left when !isBoatOnRightSide:
                if (CanBoard(person))
                {
                    SetCharacterState(person, CharacterState.Boat);
                    ONBoat.Add(person[0]);
                }
                else
                {
                    ShowError("Boat is full! Maximum 2 people.");
                }
                break;

            case CharacterState.Boat when !isBoatOnRightSide:
                SetCharacterState(person, CharacterState.Left);
                ONBoat.Remove(person[0]);
                break;
        }

        UpdateProgress();
    }

    void MoveBoat()
    {
        if (isBoatMoving || ONBoat.Count == 0) return;

        // Check if this move would cause a violation BEFORE moving
        if (WouldCauseViolation())
        {
            ShowError("Invalid move! A wife would be left with another husband!");
            if (ErrorSound != null) ErrorSound.Play();
            return;
        }

        JumpSound.Play();
        if (BoatMoveSound != null) BoatMoveSound.Play();
        moveCount++;
        UpdateMoveCounter();
        StartCoroutine(MoveBoatSmoothly());
    }

    bool WouldCauseViolation()
    {
        // Simulate what the bank would look like after the boat leaves
        // If boat is on right, check what right side will look like
        // If boat is on left, check what left side will look like

        List<string> remainingOnCurrentBank = new List<string>();
        string[] allPeople = { "H1", "W1", "H2", "W2", "H3", "W3" };

        foreach (string person in allPeople)
        {
            CharacterState state = GetCharacterCurrentState(person);

            // Person is on current bank but NOT on boat
            if (isBoatOnRightSide && state == CharacterState.Right && !ONBoat.Contains(person[0]))
            {
                remainingOnCurrentBank.Add(person);
            }
            else if (!isBoatOnRightSide && state == CharacterState.Left && !ONBoat.Contains(person[0]))
            {
                remainingOnCurrentBank.Add(person);
            }
        }

        // Check if remaining people on current bank violate the rule
        return CheckBankForViolation(remainingOnCurrentBank);
    }

    bool CheckBankForViolation(List<string> peopleOnBank)
    {
        if (peopleOnBank.Count == 0) return false; // Empty bank is safe

        // Check each wife on this bank
        for (int i = 1; i <= 3; i++)
        {
            string wife = "W" + i;
            string husband = "H" + i;

            if (peopleOnBank.Contains(wife))
            {
                bool hasOwnHusband = peopleOnBank.Contains(husband);

                if (!hasOwnHusband)
                {
                    // Check if there's any other husband on the bank
                    for (int j = 1; j <= 3; j++)
                    {
                        if (j != i && peopleOnBank.Contains("H" + j))
                        {
                            return true; // Violation!
                        }
                    }
                }
            }
        }

        return false;
    }

    IEnumerator MoveBoatSmoothly()
    {
        isBoatMoving = true;

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

        if (BoatMoveSound != null) BoatMoveSound.Stop();

        UpdateProgress();
    }

    bool CanBoard(string person)
    {
        return ONBoat.Count < 2; // Boat capacity is 2
    }

    void ShowError(string message)
    {
        if (ErrorMessageText != null)
        {
            ErrorMessageText.text = message;
            StartCoroutine(ClearErrorMessage());
        }
    }

    IEnumerator ClearErrorMessage()
    {
        yield return new WaitForSeconds(2f);
        if (ErrorMessageText != null)
            ErrorMessageText.text = "";
    }

    void UpdateProgress()
    {
        int charactersOnLeft = 0;
        string[] allPeople = { "H1", "W1", "H2", "W2", "H3", "W3" };

        foreach (string person in allPeople)
        {
            if (GetCharacterCurrentState(person) == CharacterState.Left)
                charactersOnLeft++;
        }

        if (progressSlider != null)
            progressSlider.value = charactersOnLeft / 6f;

        GoButton.interactable = ONBoat.Count > 0 && !isBoatMoving;

        if (charactersOnLeft == 6)
        {
            WinState.SetActive(true);
            CalculateStars();
        }
    }

    void UpdateMoveCounter()
    {
        if (moveCounterText != null)
            moveCounterText.text = "Moves: " + moveCount;
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
                    MovePerson("H1");
                // Check Wife 1
                else if (clickedObject == Wife1Left || clickedObject == Wife1OnBoat || clickedObject == Wife1Right)
                    MovePerson("W1");
                // Check Husband 2
                else if (clickedObject == Husband2Left || clickedObject == Husband2OnBoat || clickedObject == Husband2Right)
                    MovePerson("H2");
                // Check Wife 2
                else if (clickedObject == Wife2Left || clickedObject == Wife2OnBoat || clickedObject == Wife2Right)
                    MovePerson("W2");
                // Check Husband 3
                else if (clickedObject == Husband3Left || clickedObject == Husband3OnBoat || clickedObject == Husband3Right)
                    MovePerson("H3");
                // Check Wife 3
                else if (clickedObject == Wife3Left || clickedObject == Wife3OnBoat || clickedObject == Wife3Right)
                    MovePerson("W3");
            }
        }
    }

    public void ResetGame()
    {
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
        // Award stars based on number of moves
        // Optimal solution is around 11-13 moves
        int starsEarned = 3;

        if (moveCount > 15) starsEarned = 2;
        if (moveCount > 20) starsEarned = 1;

        for (int i = 0; i < stars.Length; i++)
        {
            if (i < starsEarned)
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
        starScore = starsEarned;
    }
}