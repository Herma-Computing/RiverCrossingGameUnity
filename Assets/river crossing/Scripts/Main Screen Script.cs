using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreenScript : MonoBehaviour
{
    [Header("Navigation Buttons")]
    [SerializeField] private Button quitButton;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;


    [Header("Sound Buttons")]
    [SerializeField] private Button soundOnButton;
    [SerializeField] private Button soundOffButton;

    [Header("Audio")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource buttonClick;

    // Constants
    private const string SOUND_ENABLED_KEY = "SoundEnabled";
    private const float BUTTON_DELAY = 0.2f;

    private void Start()
    {
        // Navigation buttons
        quitButton.onClick.AddListener(QuitGame);
        level1Button.onClick.AddListener(Level1);
        level2Button.onClick.AddListener(Level2);
   

        // Sound buttons - DIRECT APPROACH
        soundOnButton.onClick.AddListener(() => {
            // Turn sound ON
            music.Play();
            buttonClick.Play();
            PlayerPrefs.SetInt(SOUND_ENABLED_KEY, 1);

            // Hide ON button, Show OFF button
            soundOnButton.gameObject.SetActive(false);
            soundOffButton.gameObject.SetActive(true);
        });

        soundOffButton.onClick.AddListener(() => {
            // Turn sound OFF
            music.Stop();
            buttonClick.Play();
            PlayerPrefs.SetInt(SOUND_ENABLED_KEY, 0);

            // Show ON button, Hide OFF button
            soundOnButton.gameObject.SetActive(true);
            soundOffButton.gameObject.SetActive(false);
        });

        // Set initial state
        bool soundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1;

        if (soundEnabled)
        {
            music.Play();
            soundOnButton.gameObject.SetActive(false);
            soundOffButton.gameObject.SetActive(true);
        }
        else
        {
            music.Stop();
            soundOnButton.gameObject.SetActive(true);
            soundOffButton.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        StartCoroutine(QuitWithDelay());
    }

    private IEnumerator QuitWithDelay()
    {
        if (PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1)
            buttonClick.Play();

        yield return new WaitForSeconds(BUTTON_DELAY);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Level1()
    {
        StartCoroutine(LoadSceneWithDelay(1));
    }

    public void Level2()
    {
        StartCoroutine(LoadSceneWithDelay(2));
    }


    private IEnumerator LoadSceneWithDelay(int sceneIndex)
    {
        if (PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1)
            buttonClick.Play();

        yield return new WaitForSeconds(BUTTON_DELAY);
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}