using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneTracker : MonoBehaviour
{
    public enum VN_Actions
    {
        Line,               // Writes the lines that will be said along with the name of who's saying it
        Appear,             // Makes a gameObject sprite appear either in frame instantly or from off screen and move on screen
        Remove,             // Makes a gameObject sprite dissapear either in frame instantly or move off screen then disapear
        Shake,              // Shakes a gameObject ***For example play the yarn example VN to see what I mean
        Mirror,             // Rotates a gameObject sprite on an axis so there facing the opposite direction
        Move,               // Moves a gameObject from where they are on the screen to another spot on the screen
        Backgound,          // Changes the background image
        ChangePose,          // Swaps either the gameObject or sprite of something to something else, likely used for when we want to have a character change there body pose mide conversation
        Choice,
        PNC                  //Starts the Point and Click gameplay
    }

    public SceneActions[] performableActions;

    public int numberOfTrees;

    public string[] sceneNames;

    public List<string> Lines;

    public List<int> linesOreintation;

    public SceneText[] GameLines;
    public SceneText[] GameNames;

    public SceneObjects[] charactersToAppear;
    public SceneObjects[] charactersToDisapear;
    public SceneObjects[] charactersToMirror;

    public SceneObjects[] objectsToShake;
    public SceneNumbers[] intensityOfShake;

    public SceneObjects[] charactersToMove;
    public SceneLocations[] locationsToMove;
    public SceneNumbers[] speedtoMove;

    public SceneSprite[] Backgrounds;

    public SceneObjects[] currentPose;
    public SceneObjects[] newPoseWithObj;
    public SceneSprite[] newPose;

    public NestedDecitionText[] choices;
    public NestedBranchingScene[] scenesToGoTo;
    private List<int> decisionsMade = new List<int>();

    public GameObject transitionOverlay;
    public GameObject[] PNCOverlay;
    public Image backgroundImage;
    public Text displayText;
    public Text continueText;
    public Text nameText;
    public ChoicePanal panel;

    public string[] saveRandomIntroText;
    public SceneText[] saveRandomPreText;
    public Sprite[] saveRandomImages;

    public GameObject[] items;
    public int count;
    public int numCompleted = 0;
    public GameObject currentPNC;

    private int currentScene = 0;
    private bool branchingScene = false;

    private int numOfLinesPerScene = 0;
    private int charactersAppearing = 0;
    private int numBackgrounds = 0;
    private int numMovedCharacter = 0;
    private int numRemovedChars = 0;
    private int numRotatingCharacters = 0;
    private int shakeObjects = 0;
    private int numOfPosChanges = 0;
    private int numOfChoices = 0;

    [Serializable]
    public class SceneText
    {
        [TextArea(1, 5)]
        public List<string> sceneText = new List<string>();
    }

    [Serializable]
    public class NestedDecitionText
    {
        public SceneText[] nestedDecitions;
    }

    [Serializable]
    public class SceneNumbers
    {
        public List<float> sceneNumbers = new List<float>();       
    }

    [Serializable]
    public class NestedBranchingScene
    {
        public SceneNumbers[] nestedSceneBranching;
    }

    [Serializable]
    public class SceneObjects
    {
        public List<GameObject> sceneObject = new List<GameObject>();
    }

    [Serializable]
    public class SceneLocations
    {
        public List<Vector3> sceneLocations = new List<Vector3>();
    }

    [Serializable]
    public class SceneImages
    {
        public List<Image> sceneImages = new List<Image>();
    }

    [Serializable]
    public class SceneSprite
    {
        public List<Sprite> sceneSprites = new List<Sprite>();
    }

    [Serializable]
    public class SceneActions
    {
        public string SceneName;
        public VN_Actions[] Actions;
    }

    public void Awake()
    {
        StartCoroutine(VisualNovelSceneCurator(1));
    }

    public void Update()
    {
        // Have 
    }

    IEnumerator WaitForUpKey(KeyCode key)
    {
        while (!Input.GetKeyUp(key))
        {
            yield return null;
        }
    }

    IEnumerator WaitForDownKey(KeyCode key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }

    public IEnumerator VisualNovelSceneCurator(int sceneToInitiate)
    {
        transitionOverlay.SetActive(true);
        continueText.enabled = false;
        currentScene = sceneToInitiate;
        for (int scene = currentScene; scene < performableActions.Length; scene++)
        {
            for (int i = 0; i < performableActions[scene].Actions.Length; i++)
            {
                if (branchingScene == true)
                {
                    i--;
                    CountersReset();
                    branchingScene = false;
                }

                switch (performableActions[scene].Actions[i])
                {
                    case VN_Actions.Line:
                        yield return LoadTextDialogue(GameLines[scene].sceneText[numOfLinesPerScene], GameNames[scene].sceneText[numOfLinesPerScene]);
                        numOfLinesPerScene++;
                        break;

                    case VN_Actions.Appear:
                        Appear(charactersToAppear[scene].sceneObject[charactersAppearing]);
                        charactersAppearing++;
                        break;

                    case VN_Actions.Backgound:
                        Backgound(Backgrounds[scene].sceneSprites[numBackgrounds]);
                        numBackgrounds++;
                        break;

                    case VN_Actions.Move:
                        StartCoroutine(MoveLerp(charactersToMove[scene].sceneObject[numMovedCharacter], locationsToMove[scene].sceneLocations[numMovedCharacter], speedtoMove[scene].sceneNumbers[numMovedCharacter])); 
                        numMovedCharacter++;
                        break;

                    case VN_Actions.Remove:
                        Remove(charactersToDisapear[scene].sceneObject[numRemovedChars]);
                        numRemovedChars++;
                        break;

                    case VN_Actions.Mirror:
                        Mirror(charactersToMirror[scene].sceneObject[numRotatingCharacters]);
                        numRotatingCharacters++;
                        break;

                    case VN_Actions.Shake:
                        shakeObjects++;
                        break;

                    case VN_Actions.ChangePose:
                        ChangeSprite(currentPose[scene].sceneObject[numOfPosChanges], newPoseWithObj[scene].sceneObject[numOfPosChanges]);
                        numOfPosChanges++;
                        break;

                    case VN_Actions.PNC:
                        StartCoroutine(RunPNC());
                        break;

                    case VN_Actions.Choice:
                        yield return Choice(choices[scene].nestedDecitions[numOfChoices].sceneText, scenesToGoTo[scene].nestedSceneBranching[numOfChoices].sceneNumbers);
                        numOfChoices++;
                        if (branchingScene == true)
                        {
                            i = 0;
                        }
                        break;

                }
            }
            CountersReset();
            Debug.Log("VN Scene Complete");
        }
    }

    IEnumerator WaitForMidTextSkip()
    {
        while (!continueText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                continueText.enabled = true;
            }
            yield return null;
        }
    }


    IEnumerator LoadTextDialogue(string text, string name)
    {
        nameText.text = name;
        StartCoroutine(WaitForMidTextSkip());
        for (int i = 0; i < text.Length; i++)
        {
            displayText.text = text.Substring(0, i);

            yield return new WaitForSecondsRealtime(0.03f);

            if (continueText.enabled)
            {
                break;
            }
        }
        displayText.text = text;
        
        /*while (sm.transitionAudio.isPlaying)
        {
            yield return 0;
        }*/

      
        continueText.enabled = true;
        yield return StartCoroutine(WaitForDownKey(KeyCode.Return));
        continueText.enabled = false;
        yield return null;
    }


    void Appear(GameObject obj)
    {
        obj.SetActive(true);
    }


    void Remove(GameObject obj)
    {
        obj.SetActive(false);
    }

    void Shake()
    {
        
    }

    void Mirror(GameObject objToMirror)
    {
        var trans = objToMirror.transform;
        trans.rotation = new Quaternion(trans.rotation.x, trans.rotation.y + 180, trans.rotation.z, trans.rotation.w);
    }

    void Move(GameObject obj, Vector3 endPos, float timeToMove)
    {
        //StartCoroutine(MoveLerp(obj, startPos, endPos, timeToMove));
    }

    IEnumerator MoveLerp(GameObject obj, Vector3 endPos, float timeToMove)
    {
        float elapsedTime = 0;
        while (elapsedTime <= timeToMove/3)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void Backgound(Sprite img)
    {
        ChangePose(backgroundImage, img);
    }

    void ChangeSprite(GameObject pose1, GameObject pose2)
    {
        //ChangePose(obj.GetComponent<Image>(), img);
        ChangePoseWithObject(pose1, pose2);
    }

    void ChangePose(Image obj, Sprite img)
    {
        obj.sprite = img;
    }

    void ChangePoseWithObject(GameObject pose1, GameObject pose2)
    {
        pose1.SetActive(false);
        pose2.SetActive(true);
    }

    IEnumerator RunPNC()
    {
        
        transitionOverlay.SetActive(false);

        currentPNC = PNCOverlay[numCompleted];
        currentPNC.SetActive(true);

        items = GameObject.FindGameObjectsWithTag("Item");
        count = items.Length;
       
        while (count > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }

        transitionOverlay.SetActive(true);
        currentPNC.SetActive(false);
        numCompleted++;
       
        StopCoroutine(RunPNC());

    }

    IEnumerator Choice(List<string> choices, List<float> outcomes)
    {
        string[] arrayChoices = choices.ToArray();

        panel.Show("Choose Wisely", arrayChoices);

        while (panel.isWaitingOnUserChoice)
        {
            yield return null;
        }

        var dicision = panel.lastDecision;

        decisionsMade.Add(dicision.answerIndex);

        Debug.Log(outcomes[dicision.answerIndex]);
        Debug.Log(currentScene);
        if (outcomes[dicision.answerIndex] != currentScene)
        {
            currentScene = (int)outcomes[dicision.answerIndex];
            branchingScene = true;
            Debug.Log("Entering New Scene");
        }

        Debug.Log($"Made choice {dicision.answerIndex} '{dicision.choices[dicision.answerIndex]}'");
    }


    private void CountersReset()
    {
        numOfLinesPerScene = 0;
        charactersAppearing = 0;
        numBackgrounds = 0;
        numMovedCharacter = 0;
        numRemovedChars = 0;
        numRotatingCharacters = 0;
        shakeObjects = 0;
        numOfPosChanges = 0;
        numOfChoices = 0;
    }
}
