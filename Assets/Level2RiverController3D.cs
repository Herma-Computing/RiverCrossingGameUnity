using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Level2RiverController3D : MonoBehaviour
{
    // Characters (3 versions each - assign these in Inspector)
    [Header("Dad Versions")]
    public GameObject DadLeft;
    public GameObject DadOnBoat;
    public GameObject DadRight;

    [Header("Mom Versions")]
    public GameObject MomLeft;
    public GameObject MomOnBoat;
    public GameObject MomRight;

    [Header("Boy Versions")]
    public GameObject BoyLeft;
    public GameObject BoyOnBoat;
    public GameObject BoyRight;

    [Header("Girl Versions")]
    public GameObject GirlLeft;
    public GameObject GirlOnBoat;
    public GameObject GirlRight;

    [Header("Boat")]
    public GameObject Boat;
    public Transform BoatCharactersParent; // Parent object for boat characters (optional - if not set, will use Boat)

    [Header("Particle Effects")]
    public ParticleSystem BoatParticleEffect1; // First particle effect (e.g., water splash)
    public ParticleSystem BoatParticleEffect2; // Second particle effect (e.g., bubbles)
    public ParticleSystem BoatParticleEffect3; // Third particle effect (e.g., mist/foam)

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
    public Button StartLevel2Button;

    // Boat Positions (drag GameObjects here in inspector)
    [Header("Boat Positions")]
    public GameObject BoatLeftPosition;
    public GameObject BoatRightPosition;

    // Panels
    public GameObject WinState;
    public GameObject Pause;
    public GameObject HowToPlay;

    // Sound
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] AudioSource JumpSound;
    [SerializeField] AudioSource BoatMoveSound;

    // Game Variables
    public List<char> ONBoat = new List<char>();
    public Slider progressSlider;
    public GameObject[] stars;
    public GameObject[] nostars;
    public int starScore;

    private bool isBoatMoving = false;
    private bool isBoatOnRightSide = true;

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
        StartLevel2Button.onClick.AddListener(() => HowToPlay.SetActive(false));

        UpdateProgress();
    }

    void SetInitialPositions()
    {
        // Set all characters to right side initially
        SetCharacterState('D', CharacterState.Right);
        SetCharacterState('M', CharacterState.Right);
        SetCharacterState('B', CharacterState.Right);
        SetCharacterState('G', CharacterState.Right);

        // Boat keeps its initial position and rotation from Inspector
        // No changes to boat transform at start!

        // Make sure all particle effects are off at start
        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

        ONBoat.Clear();
        isBoatOnRightSide = true;
    }

    enum CharacterState { Left, Boat, Right }

    void SetCharacterState(char person, CharacterState state)
    {
        // Get all three versions of the character
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

        // Deactivate all versions
        left.SetActive(false);
        boat.SetActive(false);
        right.SetActive(false);

        // Activate the correct version
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

    (GameObject, GameObject, GameObject) GetCharacterVersions(char person)
    {
        switch (person)
        {
            case 'D': return (DadLeft, DadOnBoat, DadRight);
            case 'M': return (MomLeft, MomOnBoat, MomRight);
            case 'B': return (BoyLeft, BoyOnBoat, BoyRight);
            case 'G': return (GirlLeft, GirlOnBoat, GirlRight);
            default: return (null, null, null);
        }
    }

    CharacterState GetCharacterCurrentState(char person)
    {
        (GameObject left, GameObject boat, GameObject right) = GetCharacterVersions(person);

        if (left.activeInHierarchy) return CharacterState.Left;
        if (boat.activeInHierarchy) return CharacterState.Boat;
        if (right.activeInHierarchy) return CharacterState.Right;

        return CharacterState.Right; // Default fallback
    }

    void MovePerson(char person)
    {
        if (isBoatMoving) return;

        JumpSound.Play();
        CharacterState currentState = GetCharacterCurrentState(person);

        switch (currentState)
        {
            case CharacterState.Right when isBoatOnRightSide:
                // Move from right side to boat
                if (CanBoard(person))
                {
                    SetCharacterState(person, CharacterState.Boat);
                    ONBoat.Add(person);
                }
                break;

            case CharacterState.Boat when isBoatOnRightSide:
                // Move from boat back to right side
                SetCharacterState(person, CharacterState.Right);
                ONBoat.Remove(person);
                break;

            case CharacterState.Left when !isBoatOnRightSide:
                // Move from left side to boat
                if (CanBoard(person))
                {
                    SetCharacterState(person, CharacterState.Boat);
                    ONBoat.Add(person);
                }
                break;

            case CharacterState.Boat when !isBoatOnRightSide:
                // Move from boat to left side
                SetCharacterState(person, CharacterState.Left);
                ONBoat.Remove(person);
                break;
        }

        UpdateProgress();
    }

    void MoveBoat()
    {
        if (isBoatMoving || ONBoat.Count == 0) return;

        JumpSound.Play();
        if (BoatMoveSound != null) BoatMoveSound.Play();
        StartCoroutine(MoveBoatSmoothly());
    }

    IEnumerator MoveBoatSmoothly()
    {
        isBoatMoving = true;

        // Turn ON all particle effects when boat starts moving
        if (BoatParticleEffect1 != null) BoatParticleEffect1.Play();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Play();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Play();

        GameObject targetPosition = isBoatOnRightSide ? BoatLeftPosition : BoatRightPosition;
        Vector3 targetPos = targetPosition.transform.position;
        Vector3 startPos = Boat.transform.position;

        // Set rotation based on direction
        // Going Left (Right to Left): Face Left (-90 on Y axis)
        // Going Right (Left to Right): Face Right (90 on Y axis)
        Quaternion targetRotation = isBoatOnRightSide ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
        Boat.transform.rotation = targetRotation;

        // Also rotate the boat characters if they are children of the boat
        // (They will automatically rotate with the boat if they're children)
        // If you need manual rotation, uncomment below:
        /*
        if (BoatCharactersParent != null)
        {
            BoatCharactersParent.rotation = targetRotation;
        }
        */

        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime * 0.5f; // 2 second duration
            Boat.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Boat.transform.position = targetPos;
        isBoatOnRightSide = !isBoatOnRightSide;

        isBoatMoving = false;

        // Turn OFF all particle effects when boat stops
        if (BoatParticleEffect1 != null) BoatParticleEffect1.Stop();
        if (BoatParticleEffect2 != null) BoatParticleEffect2.Stop();
        if (BoatParticleEffect3 != null) BoatParticleEffect3.Stop();

        if (BoatMoveSound != null) BoatMoveSound.Stop();

        UpdateProgress();
    }

    bool CanBoard(char person)
    {
        int bigCount = ONBoat.Count(c => c == 'D' || c == 'M');
        int smallCount = ONBoat.Count(c => c == 'B' || c == 'G');

        if (person == 'D' || person == 'M')
        {
            return ONBoat.Count == 0; // Adults travel alone
        }
        else
        {
            return bigCount == 0 && smallCount < 2; // Max 2 children, no adults
        }
    }

    void UpdateProgress()
    {
        int charactersOnLeft = 0;

        if (GetCharacterCurrentState('D') == CharacterState.Left) charactersOnLeft++;
        if (GetCharacterCurrentState('M') == CharacterState.Left) charactersOnLeft++;
        if (GetCharacterCurrentState('B') == CharacterState.Left) charactersOnLeft++;
        if (GetCharacterCurrentState('G') == CharacterState.Left) charactersOnLeft++;

        if (progressSlider != null)
            progressSlider.value = charactersOnLeft / 4f;

        GoButton.interactable = ONBoat.Count > 0 && !isBoatMoving;

        if (charactersOnLeft == 4)
        {
            WinState.SetActive(true);
            CalculateStars();
        }
    }

    void Update()
    {
        // Click directly on 3D characters to move them
        if (Input.GetMouseButtonDown(0) && !isBoatMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked on: " + clickedObject.name); // Debug line to see what you're clicking

                // Check all character versions
                if (clickedObject == DadLeft || clickedObject == DadOnBoat || clickedObject == DadRight)
                    MovePerson('D');
                else if (clickedObject == MomLeft || clickedObject == MomOnBoat || clickedObject == MomRight)
                    MovePerson('M');
                else if (clickedObject == BoyLeft || clickedObject == BoyOnBoat || clickedObject == BoyRight)
                    MovePerson('B');
                else if (clickedObject == GirlLeft || clickedObject == GirlOnBoat || clickedObject == GirlRight)
                    MovePerson('G');
            }
            else
            {
                Debug.Log("No collider hit!"); // Debug line if raycast misses
            }
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadSceneAsync(2);
        ButtonClick.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        ButtonClick.Play();
    }

    void CalculateStars()
    {
        // Give 3 stars for completion
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(true);
            if (i < nostars.Length) nostars[i].SetActive(false);
        }
        starScore = 3;
    }
}