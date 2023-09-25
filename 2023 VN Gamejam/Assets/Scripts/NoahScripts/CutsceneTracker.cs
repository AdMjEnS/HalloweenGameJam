﻿using System;
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

    public SceneImages[] currentPose;
    public SceneSprite[] newPose;

    public NestedDecitionText[] choices;
    public NestedBranchingScene[] scenesToGoTo;
    private List<int> decisionsMade = new List<int>();

    public GameObject transitionOverlay;
    public GameObject[] PNCOverlay;
    public Image transitionPicture;
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
        public VN_Actions[] Actions;
    }

    public void Awake()
    {
        StartCoroutine(VisualNovelSceneCurator(2));
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
        if (true)
        {
            for (int i = 0; i < performableActions[currentScene].Actions.Length; i++)
            {
                if (branchingScene == true)
                {
                    i--;
                    CountersReset();
                    branchingScene = false;
                }

                switch (performableActions[currentScene].Actions[i])
                {
                    case VN_Actions.Line:
                        yield return LoadTextDialogue(GameLines[currentScene].sceneText[numOfLinesPerScene], GameNames[currentScene].sceneText[numOfLinesPerScene]);
                        numOfLinesPerScene++;
                        break;

                    case VN_Actions.Appear:
                        Appear(charactersToAppear[currentScene].sceneObject[charactersAppearing]);
                        charactersAppearing++;
                        break;

                    case VN_Actions.Backgound:
                        Backgound(Backgrounds[currentScene].sceneSprites[numBackgrounds]);
                        numBackgrounds++;
                        break;

                    case VN_Actions.Move:
                        Move(charactersToMove[currentScene].sceneObject[numMovedCharacter], locationsToMove[currentScene].sceneLocations[numMovedCharacter], speedtoMove[currentScene].sceneNumbers[numMovedCharacter]);
                        numMovedCharacter++;
                        break;

                    case VN_Actions.Remove:
                        Remove(charactersToDisapear[currentScene].sceneObject[numRemovedChars]);
                        numRemovedChars++;
                        break;

                    case VN_Actions.Mirror:
                        Mirror(charactersToMirror[currentScene].sceneObject[numRotatingCharacters]);
                        numRotatingCharacters++;
                        break;

                    case VN_Actions.Shake:
                        shakeObjects++;
                        break;

                    case VN_Actions.ChangePose:
                        ChangePose(currentPose[currentScene].sceneImages[numOfLinesPerScene], newPose[currentScene].sceneSprites[numOfLinesPerScene]);
                        numOfPosChanges++;
                        break;

                    case VN_Actions.PNC:
                        StartCoroutine(RunPNC());
                        break;

                    case VN_Actions.Choice:
                        yield return Choice(choices[currentScene].nestedDecitions[numOfChoices].sceneText, scenesToGoTo[currentScene].nestedSceneBranching[numOfChoices].sceneNumbers);
                        numOfChoices++;
                        if (branchingScene == true)
                        {
                            i = 0;
                        }
                        break;

                }
            }
        }
        CountersReset();
        Debug.Log("VN Scene Complete");
    }

    IEnumerator LoadTextDialogue(string text, string name)
    {
        nameText.text = name;
        for (int i = 0; i < text.Length; i++)
        {
            displayText.text = text.Substring(0, i);

            yield return new WaitForSecondsRealtime(0.03f);
        }
        displayText.text = text;
        
        /*while (sm.transitionAudio.isPlaying)
        {
            yield return 0;
        }*/

      
        continueText.enabled = true;
        yield return StartCoroutine(WaitForDownKey(KeyCode.Return));
        continueText.enabled = false;
    }


    void Appear(GameObject obj)
    {
        obj.SetActive(true);
    }


    void Remove(GameObject obj)
    {
        obj.SetActive(false);
    }
    /*
    IEnumerator Appear(GameObject obj, Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        float elapsedTime = 0;
        while (elapsedTime <= timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    } */

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

    IEnumerator MoveLerp(GameObject obj, Vector3 startPos, Vector3 endPos, float timeToMove)
    {
        transform.position = startPos;
        float elapsedTime = 0;
        while (elapsedTime <= timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    [SerializeField]
    Image backgroundImage;
    void Backgound(Sprite img)
    {
        ChangePose(backgroundImage, img);
    }
    void ChangePose(Image obj, Sprite img)
    {
        obj.sprite = img;
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
