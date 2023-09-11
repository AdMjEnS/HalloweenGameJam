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
        Rotate,             // Rotates a gameObject sprite on an axis so there facing the opposite direction
        Move,               // Moves a gameObject from where they are on the screen to another spot on the screen
        Backgound,          // Changes the background image
        ChangePose          // Swaps either the gameObject or sprite of something to something else, likely used for when we want to have a character change there body pose mide conversation
    }

    public SceneActions[] performableActions;

    public int numberOfTrees;

    public string[] actionToPreform;

    public List<string> Lines;

    public List<int> linesOreintation;

    public SceneText[] GameLines;
    public SceneText[] GameNames;

    public GameObject transitionOverlay;
    public Image transitionPicture;
    public Text displayText;
    public Text continueText;
    public Text nameText;

    public string[] saveRandomIntroText;
    public SceneText[] saveRandomPreText;
    public Sprite[] saveRandomImages;

    [Serializable]
    public class SceneText
    {
        public List<string> sceneText;
    }

    [Serializable]
    public class SceneActions
    {
        public VN_Actions[] Actions;
    }

    public void Awake()
    {
        StartCoroutine(BegginingOfGameText());
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

    public IEnumerator BegginingOfGameText()
    {
        transitionOverlay.SetActive(true);
        continueText.enabled = false;
        if (true)
        {
            for (int i = 0; i < GameLines.Length; i++)
            {
                for (int j = 0; j < GameLines[i].sceneText.Count; j++)
                {
                    yield return StartCoroutine(LoadTextDialogue(GameLines[i].sceneText[j], GameNames[i].sceneText[j]));
                    //sm.transitionAudio.clip = sm.savingRandomStartAudio[i];
                    //sm.transitionAudio.Play();

                }
            }
        }
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
}
