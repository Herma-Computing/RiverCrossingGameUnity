using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class Level1RiverController3D : MonoBehaviour
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

    // Position References (assign these in the inspector)
    public GameObject CabbageRightSide;
    public GameObject CabbageLeftSide;
    public GameObject SheepRightSide;
    public GameObject SheepLeftSide;
    public GameObject WolfRightSide;
    public GameObject WolfLeftSide;
    public GameObject BoatCarryRightSide;
    public GameObject BoatCarryLeftSide;
    public GameObject BoatRightSide;
    public GameObject BoatLeftSide;

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
    [SerializeField] AudioSource BoatMoveSound;

    // Progress Bar
    public Slider progressSlider;

    // Boat movement parameters
    public float boatAnimationDuration = 2.0f;
    private bool isBoatMoving = false;
    private bool isBoatOccupied = false;
    private Vector3 boatTargetPosition;
    private GameObject currentPassenger = null;

    // Animation
    private Animator boatAnimator;
    private bool isBoatOnRightSide = true;

    void Start()
    {
        // Get the boat's animator
        boatAnimator = Boat.GetComponent<Animator>();

        // Set initial positions
        SetInitialPositions();

        // Button listeners
        GoButton.onClick.AddListener(StartBoatMovement);
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

        UpdateProgressBar();

        // GoButton is always interactable (boat can move without passenger)
        GoButton.interactable = true;
    }

    void SetInitialPositions()
    {
        // Set all characters to their right side positions
        Sheep.transform.position = SheepRightSide.transform.position;
        Wolf.transform.position = WolfRightSide.transform.position;
        Cabbage.transform.position = CabbageRightSide.transform.position;
        Boat.transform.position = BoatRightSide.transform.position;

        // Set initial scales (facing right)
        Sheep.transform.localScale = new Vector3(
            Mathf.Abs(Sheep.transform.localScale.x),
            Sheep.transform.localScale.y,
            Sheep.transform.localScale.z
        );

        Wolf.transform.localScale = new Vector3(
            Mathf.Abs(Wolf.transform.localScale.x),
            Wolf.transform.localScale.y,
            Wolf.transform.localScale.z
        );

        Cabbage.transform.localScale = new Vector3(
            Mathf.Abs(Cabbage.transform.localScale.x),
            Cabbage.transform.localScale.y,
            Cabbage.transform.localScale.z
        );

        // Reset boat occupancy
        isBoatOccupied = false;
        currentPassenger = null;
        isBoatOnRightSide = true;

        // Set boat to idle animation
        if (boatAnimator != null)
        {
            boatAnimator.SetBool("IsMoving", false);
            boatAnimator.SetFloat("MoveDirection", 1f);
        }

        // Set boat rotation to face right
        Boat.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public void UpdateProgressBar()
    {
        if (progressSlider == null) return;

        int charactersOnLeft = 0;

        // Check if Sheep is on left side
        if (Vector3.Distance(Sheep.transform.position, SheepLeftSide.transform.position) < 0.1f)
            charactersOnLeft++;

        // Check if Wolf is on left side
        if (Vector3.Distance(Wolf.transform.position, WolfLeftSide.transform.position) < 0.1f)
            charactersOnLeft++;

        // Check if Cabbage is on left side
        if (Vector3.Distance(Cabbage.transform.position, CabbageLeftSide.transform.position) < 0.1f)
            charactersOnLeft++;

        // Update slider value (0 to 1)
        // 3 characters need to cross, so divide by 3
        progressSlider.value = charactersOnLeft / 3f;
    }

    void MoveSheep()
    {
        if (isBoatMoving) return;

        JumpSound.Play();

        // If boat already has something and it's not the Sheep, don't let Sheep move
        if (isBoatOccupied && currentPassenger != Sheep)
            return;

        // Check if boat is on right side
        if (Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 0.1f)
        {
            // Boat is on right side
            if (Vector3.Distance(Sheep.transform.position, SheepRightSide.transform.position) < 0.1f)
            {
                // Sheep is on right side, move to boat
                Sheep.transform.position = BoatCarryRightSide.transform.position;
                Sheep.transform.localRotation = Quaternion.Euler(0, -177.101f, 0);
                currentPassenger = Sheep;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Sheep.transform.position, BoatCarryRightSide.transform.position) < 0.1f)
            {
                // Sheep is on boat on right side, move back to right side
                Sheep.transform.position = SheepRightSide.transform.position;
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }
        else if (Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f)
        {
            // Boat is on left side
            if (Vector3.Distance(Sheep.transform.position, SheepLeftSide.transform.position) < 0.1f)
            {
                // Sheep is on left side, move to boat
                Sheep.transform.position = BoatCarryLeftSide.transform.position;
                Sheep.transform.localRotation = Quaternion.Euler(0, -177.101f, 0);
                currentPassenger = Sheep;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Sheep.transform.position, BoatCarryLeftSide.transform.position) < 0.1f)
            {
                // Sheep is on boat on left side, move back to left side
                Sheep.transform.position = SheepLeftSide.transform.position;
                Sheep.transform.localRotation = Quaternion.Euler(0, -76.188f, 0);
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }

        // Update progress bar after moving
        UpdateProgressBar();

        // Check game conditions after moving sheep
        CheckGameConditions();
    }

    void MoveWolf()
    {
        if (isBoatMoving) return;

        JumpSound.Play();

        // If boat already has something and it's not the wolf, don't let wolf move
        if (isBoatOccupied && currentPassenger != Wolf)
            return;

        // Check if boat is on right side
        if (Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 0.1f)
        {
            // Boat is on right side
            if (Vector3.Distance(Wolf.transform.position, WolfRightSide.transform.position) < 0.1f)
            {
                // Wolf is on right side, move to boat
                Wolf.transform.position = BoatCarryRightSide.transform.position;
                Wolf.transform.localRotation = Quaternion.Euler(0, -183.323f, 0);
                currentPassenger = Wolf;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Wolf.transform.position, BoatCarryRightSide.transform.position) < 0.1f)
            {
                // Wolf is on boat on right side, move back to right side
                Wolf.transform.position = WolfRightSide.transform.position;
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }
        else if (Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f)
        {
            // Boat is on left side
            if (Vector3.Distance(Wolf.transform.position, WolfLeftSide.transform.position) < 0.1f)
            {
                // Wolf is on left side, move to boat
                Wolf.transform.position = BoatCarryLeftSide.transform.position;
                Wolf.transform.localRotation = Quaternion.Euler(0, -183.323f, 0);
                currentPassenger = Wolf;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Wolf.transform.position, BoatCarryLeftSide.transform.position) < 0.1f)
            {
                // Wolf is on boat on left side, move back to left side
                Wolf.transform.position = WolfLeftSide.transform.position;
                Wolf.transform.localRotation = Quaternion.Euler(0, -82.41f, 0);
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }

        // Update progress bar after moving
        UpdateProgressBar();

        // Check game conditions after moving wolf
        CheckGameConditions();
    }

    void MoveCabbage()
    {
        if (isBoatMoving) return;

        JumpSound.Play();

        // If boat already has something and it's not the Cabbage, don't let Cabbage move
        if (isBoatOccupied && currentPassenger != Cabbage)
            return;

        // Check if boat is on right side
        if (Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 1f)
        {
            // Boat is on right side
            if (Vector3.Distance(Cabbage.transform.position, CabbageRightSide.transform.position) < 1f)
            {
                // Cabbage is on right side, move to boat
                Cabbage.transform.position = BoatCarryRightSide.transform.position;
                currentPassenger = Cabbage;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Cabbage.transform.position, BoatCarryRightSide.transform.position) < 1f)
            {
                // Cabbage is on boat on right side, move back to right side
                Cabbage.transform.position = CabbageRightSide.transform.position;
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }
        else if (Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f)
        {
            // Boat is on left side
            if (Vector3.Distance(Cabbage.transform.position, CabbageLeftSide.transform.position) < 0.1f)
            {
                // Cabbage is on left side, move to boat
                Cabbage.transform.position = BoatCarryLeftSide.transform.position;
                currentPassenger = Cabbage;
                isBoatOccupied = true;
            }
            else if (Vector3.Distance(Cabbage.transform.position, BoatCarryLeftSide.transform.position) < 0.1f)
            {
                // Cabbage is on boat on left side, move back to left side
                Cabbage.transform.position = CabbageLeftSide.transform.position;
                currentPassenger = null;
                isBoatOccupied = false;
            }
        }

        // Update progress bar after moving
        UpdateProgressBar();

        // Check game conditions after moving cabbage
        CheckGameConditions();
    }

    void StartBoatMovement()
    {
        if (isBoatMoving) return;

        // Check if moving the boat would cause a game over condition
        if (CheckGameOverBeforeMove())
        {
            // If moving would cause a game over, show it immediately
            return;
        }

        JumpSound.Play();
        BoatMoveSound.Play();
        StartCoroutine(MoveBoatSmoothly());
    }

    bool CheckGameOverBeforeMove()
    {
        // Check if moving the boat would leave wolf and sheep alone
        bool isBoatOnRight = Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 0.1f;

        if (isBoatOnRight)
        {
            // Boat is on right, check if moving would leave wolf and sheep alone on right
            bool sheepOnRight = Vector3.Distance(Sheep.transform.position, SheepRightSide.transform.position) < 0.1f;
            bool wolfOnRight = Vector3.Distance(Wolf.transform.position, WolfRightSide.transform.position) < 0.1f;
            bool cabbageOnRight = Vector3.Distance(Cabbage.transform.position, CabbageRightSide.transform.position) < 0.1f;

            // If passenger is on boat, they won't be left behind
            if (currentPassenger == Sheep) sheepOnRight = false;
            if (currentPassenger == Wolf) wolfOnRight = false;
            if (currentPassenger == Cabbage) cabbageOnRight = false;

            // Check if wolf and sheep would be left alone
            if (sheepOnRight && wolfOnRight && !cabbageOnRight)
            {
                WolfEatSheep.SetActive(true);
                return true;
            }

            // Check if sheep and cabbage would be left alone
            if (sheepOnRight && cabbageOnRight && !wolfOnRight)
            {
                CabbageEatSheep.SetActive(true);
                return true;
            }
        }
        else
        {
            // Boat is on left, check if moving would leave wolf and sheep alone on left
            bool sheepOnLeft = Vector3.Distance(Sheep.transform.position, SheepLeftSide.transform.position) < 0.1f;
            bool wolfOnLeft = Vector3.Distance(Wolf.transform.position, WolfLeftSide.transform.position) < 0.1f;
            bool cabbageOnLeft = Vector3.Distance(Cabbage.transform.position, CabbageLeftSide.transform.position) < 0.1f;

            // If passenger is on boat, they won't be left behind
            if (currentPassenger == Sheep) sheepOnLeft = false;
            if (currentPassenger == Wolf) wolfOnLeft = false;
            if (currentPassenger == Cabbage) cabbageOnLeft = false;

            // Check if wolf and sheep would be left alone
            if (sheepOnLeft && wolfOnLeft && !cabbageOnLeft)
            {
                WolfEatSheep.SetActive(true);
                return true;
            }

            // Check if sheep and cabbage would be left alone
            if (sheepOnLeft && cabbageOnLeft && !wolfOnLeft)
            {
                CabbageEatSheep.SetActive(true);
                return true;
            }
        }

        return false;
    }

    IEnumerator MoveBoatSmoothly()
    {
        isBoatMoving = true;

        // Determine target position and set animation parameters
        bool movingToLeft = isBoatOnRightSide;
        float moveDirection = movingToLeft ? 1f : -1f;

        if (boatAnimator != null)
        {
            boatAnimator.SetBool("IsMoving", true);
            boatAnimator.SetFloat("MoveDirection", moveDirection);
        }

        boatTargetPosition = movingToLeft ? BoatLeftSide.transform.position : BoatRightSide.transform.position;

        // Flip characters to face the direction of movement
        if (currentPassenger != null)
        {
            currentPassenger.transform.localScale = new Vector3(
                movingToLeft ? -Mathf.Abs(currentPassenger.transform.localScale.x) : Mathf.Abs(currentPassenger.transform.localScale.x),
                currentPassenger.transform.localScale.y,
                currentPassenger.transform.localScale.z
            );
        }

        //Rotate boat based on direction
        if (movingToLeft)
        {
            Boat.transform.rotation = Quaternion.Euler(0, -90, 0); // Face left
        }
        else
        {
            Boat.transform.rotation = Quaternion.Euler(0, 90, 0); // Face right
        }

        // Smoothly move the boat to the target position
        Vector3 startPosition = Boat.transform.position;
        float journey = 0f;

        while (journey <= 1f)
        {
            journey += Time.deltaTime * (1f / boatAnimationDuration);
            Boat.transform.position = Vector3.Lerp(startPosition, boatTargetPosition, journey);

            // Move the passenger with the boat
            if (currentPassenger != null)
            {
                if (Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 0.1f)
                {
                    currentPassenger.transform.position = BoatCarryRightSide.transform.position;
                }
                else if (Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f)
                {
                    currentPassenger.transform.position = BoatCarryLeftSide.transform.position;
                }
                else
                {
                    // During movement, keep the passenger relative to the boat
                    Vector3 passengerOffset = BoatCarryRightSide.transform.position - BoatRightSide.transform.position;
                    currentPassenger.transform.position = Boat.transform.position + passengerOffset;
                }
            }

            yield return null;
        }

        // Ensure exact position at the end
        Boat.transform.position = boatTargetPosition;
        isBoatOnRightSide = !isBoatOnRightSide;

        // Set animation back to idle
        if (boatAnimator != null)
        {
            boatAnimator.SetBool("IsMoving", false);
        }

        isBoatMoving = false;
        BoatMoveSound.Stop();

        // Update progress bar after boat movement
        UpdateProgressBar();

        // Check game conditions after boat movement
        CheckGameConditions();
    }

    void CheckGameConditions()
    {
        // Don't check conditions if game is already over
        if (WolfEatSheep.activeSelf || CabbageEatSheep.activeSelf || WinState.activeSelf)
            return;

        bool isBoatOnRight = Vector3.Distance(Boat.transform.position, BoatRightSide.transform.position) < 0.1f;
        bool isBoatOnLeft = Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f;

        // Get which side each character is on
        bool sheepOnRight = Vector3.Distance(Sheep.transform.position, SheepRightSide.transform.position) < 0.1f ||
                            Vector3.Distance(Sheep.transform.position, BoatCarryRightSide.transform.position) < 0.1f;
        bool sheepOnLeft = Vector3.Distance(Sheep.transform.position, SheepLeftSide.transform.position) < 0.1f ||
                           Vector3.Distance(Sheep.transform.position, BoatCarryLeftSide.transform.position) < 0.1f;

        bool wolfOnRight = Vector3.Distance(Wolf.transform.position, WolfRightSide.transform.position) < 0.1f ||
                           Vector3.Distance(Wolf.transform.position, BoatCarryRightSide.transform.position) < 0.1f;
        bool wolfOnLeft = Vector3.Distance(Wolf.transform.position, WolfLeftSide.transform.position) < 0.1f ||
                          Vector3.Distance(Wolf.transform.position, BoatCarryLeftSide.transform.position) < 0.1f;

        bool cabbageOnRight = Vector3.Distance(Cabbage.transform.position, CabbageRightSide.transform.position) < 0.1f ||
                              Vector3.Distance(Cabbage.transform.position, BoatCarryRightSide.transform.position) < 0.1f;
        bool cabbageOnLeft = Vector3.Distance(Cabbage.transform.position, CabbageLeftSide.transform.position) < 0.1f ||
                             Vector3.Distance(Cabbage.transform.position, BoatCarryLeftSide.transform.position) < 0.1f;

        // Check if wolf eats sheep (when left alone together without farmer)
        if (isBoatOnRight)
        {
            // Boat is on right side, check left side for wolf and sheep alone
            if (sheepOnLeft && wolfOnLeft && !cabbageOnLeft)
            {
                // Wolf and sheep are alone on left side
                WolfEatSheep.SetActive(true);
                return;
            }
        }
        else if (isBoatOnLeft)
        {
            // Boat is on left side, check right side for wolf and sheep alone
            if (sheepOnRight && wolfOnRight && !cabbageOnRight)
            {
                // Wolf and sheep are alone on right side
                WolfEatSheep.SetActive(true);
                return;
            }
        }

        // Check if sheep eats cabbage (when left alone together without farmer)
        if (isBoatOnRight)
        {
            // Boat is on right side, check left side for sheep and cabbage alone
            if (sheepOnLeft && cabbageOnLeft && !wolfOnLeft)
            {
                // Sheep and cabbage are alone on left side
                CabbageEatSheep.SetActive(true);
                return;
            }
        }
        else if (isBoatOnLeft)
        {
            // Boat is on left side, check right side for sheep and cabbage alone
            if (sheepOnRight && cabbageOnRight && !wolfOnRight)
            {
                // Sheep and cabbage are alone on right side
                CabbageEatSheep.SetActive(true);
                return;
            }
        }

        // Check win condition
        if (Vector3.Distance(Sheep.transform.position, SheepLeftSide.transform.position) < 0.1f &&
            Vector3.Distance(Wolf.transform.position, WolfLeftSide.transform.position) < 0.1f &&
            Vector3.Distance(Cabbage.transform.position, CabbageLeftSide.transform.position) < 0.1f &&
            Vector3.Distance(Boat.transform.position, BoatLeftSide.transform.position) < 0.1f)
        {
            WinState.SetActive(true);
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
        // Detect mouse clicks in 3D
        if (Input.GetMouseButtonDown(0) && !isBoatMoving) // 0 = left click / tap
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
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
}