using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //arrays of assets used for various versions, keep the numbers the same
    /*
     * 0 - Save a random person from an illness
     * 1 - Save a family member from an illness
     * 2 - Relaxation Through Nature
     * 3 - Relaxation Through Breathing
     * 4 - Savering
     * 5 - Paridoxical Intentions
     * 6 - No Reason
     */
    [Range(0, 6)]
    public int narrativeSelector;
    public static int narrativeVersion;

    //number used for various upgrade versions, keep the numbers the same
    /*
     * 0 - No Sound
     * 1 - Voice Over Sound
     * 2 - Voice Over With In Between Sound
     */


    [Range(0, 2)]
    public int soundSelector;
    public static int soundVersion;


    public static GameManager gameManager;
    public bool isPaused;
    public bool devMode; //ensure to turn this off for when making builds
    public GameObject workerIDScreen;
    public GameObject soundCheckScreen;
    public GameObject pauseScreen;
    public GameObject transitionScreen;
    public GameObject endScreen;
    public Text displayText;
    public int textToShow = 0;
    public InputField WorkerIDnumber;

    private int midGameDialogePicker = 0;

    public GameObject HintArrow;
    public GameObject[] StartPositions;
    public int activatorsActiveCount;
    public bool[] levelsPlayed;
    public bool[] levelsCompleted;
    public Text pauseScreenGlobalTimer;

    public float afkTime;
    public float max_afkTime = 15f;
    public float puzzleTime = 120;
    public float currentTime = 1200;
    public float realTime = 0;
    public float pauseTime = 0;
    public float puzzleMin = 0;
    public float puzzleSec = 0;

    [SerializeField] public string[] tutorialText;
    [SerializeField] const int timeToPlay = 15;
    [SerializeField] int minute = 0;
    [SerializeField] int second = 0;

    public Text showPuzzleTimer;
    public Text numActivatorsRemaining;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public KeyValuePair<int, int> TimeStamp()
    {
        KeyValuePair<int, int> pair = new KeyValuePair<int, int>(timeToPlay - minute, 60 - second);
        return pair;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            textToShow++;
            displayText.text = tutorialText[textToShow];
        }
        if (textToShow == 8)
        {
            transitionScreen.SetActive(false);
        }
        if (!workerIDScreen.activeInHierarchy && !soundCheckScreen.activeInHierarchy && !transitionScreen.activeInHierarchy)
        {
            if (puzzleTime == 120)
            {
                if (soundSelector == 2)
                {
                    StartCoroutine(midGameDialoge());
                }
            }
            //globalTime += Time.deltaTime;
            puzzleTime -= Time.deltaTime;
            currentTime -= Time.deltaTime;
            updateTimer(currentTime);
        }
        if (isPaused == true)
        {
            pauseTime += Time.unscaledDeltaTime;
        }
        if (isPaused != true)
        {
            realTime += Time.unscaledDeltaTime;
        }
        if (!isPaused && !workerIDScreen.activeInHierarchy && !soundCheckScreen.activeInHierarchy && !transitionScreen.activeInHierarchy)
        {
            afkTime += Time.deltaTime;
            if (afkTime > max_afkTime)
            {
                afkTime = 0;
                Pause();
                print("pausing from afk");
            }
        }
        else
        {
            afkTime = 0;
        }

        if (puzzleTime <= 0)
        {
            HintArrow.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

    }

    public IEnumerator midGameDialoge()
    {
        yield return new WaitForSecondsRealtime(4);

        if (midGameDialogePicker > 14)
        {
            if (narrativeSelector == 5)
            {
                midGameDialogePicker = 3;
            }
            else
            {
                midGameDialogePicker = 0;
            }
        }

        midGameDialogePicker++;
    }

    public bool IsPaused() { return isPaused; }
    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        isPaused = false;
        pauseScreen.SetActive(false);
    }

    public void PauseSwitch()
    {
        if (isPaused) Unpause();
        else Pause();
    }

    public void OnApplicationFocus(bool focus)
    {
        if (!isPaused && !Input.anyKey && !workerIDScreen.activeInHierarchy && !soundCheckScreen.activeInHierarchy && !transitionScreen.activeInHierarchy)
        {
            Pause();
        }
    }

    public void updateTimer(float currentTime)
    {
        currentTime += 1;

        minute = Mathf.FloorToInt(currentTime / 60);
        second = Mathf.FloorToInt(currentTime % 60);

        if (currentTime < (1800 - realTime))
        {
            pauseScreenGlobalTimer.text = string.Format("{0:00} : {1:00}", Mathf.FloorToInt((1800 - realTime) / 60), Mathf.FloorToInt((1800 - realTime) % 60));
        }
        else
        {
            pauseScreenGlobalTimer.text = string.Format("{0:00} : {1:00}", minute, second);
        }

        puzzleMin = Mathf.FloorToInt(puzzleTime / 60);
        puzzleSec = Mathf.FloorToInt(puzzleTime % 60);

        if (puzzleTime <= 0)
        {
            float puzzleTimeDisplay = 60 + puzzleTime;

            puzzleMin = Mathf.FloorToInt(puzzleTimeDisplay / 60);
            puzzleSec = Mathf.FloorToInt(puzzleTimeDisplay % 60);

            showPuzzleTimer.text = string.Format("{0:00} : {1:00}", puzzleMin, puzzleSec) + " overtime";
        }
        else
        {
            showPuzzleTimer.text = string.Format("{0:00} : {1:00}", puzzleMin, puzzleSec);
        }
    }
}
