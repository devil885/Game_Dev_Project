using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    InputSystemActions playerControls;

    public GameState currentState;
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stats Display")]
    public Text currentHealthDisplay;
    public Text currentHealthRegenDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentStrengthDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentPickUpRangeDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text timeSurvivedDisplay;
    public Text levelReachedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public Text stopwatchDisplay;




    public bool isGameOver = false;
    public bool isChoosingUpgrade = false;

    public GameObject playerObject;

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("EXTRA " + this + " DELETED");
            Destroy(gameObject);
        }

        playerControls = new InputSystemActions();
        DisableScreens();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!isChoosingUpgrade) 
                {
                    isChoosingUpgrade = true;
                    Time.timeScale = 0f;
                    Debug.Log("Upgrades Shown");
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("game is resumed");
        }
    }

    public void CheckForPauseAndResume()
    {
        bool isPauseKeyPressed = playerControls.Player.Pause.triggered;
        if (isPauseKeyPressed)
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);

    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;

    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen weapons or passive items data lists have different length");
            return;
        }

        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch() 
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        if (stopwatchTime >= timeLimit) 
        {
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateStopwatchDisplay() 
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    public void StartLevelUp() 
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp() 
    {
        isChoosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
