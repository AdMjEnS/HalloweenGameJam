using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;

[CustomEditor(typeof(CutsceneTracker))]
public class CT_Editor : Editor
{
    //Initializating the different scroll views
    Vector2 sceneScrollPos = Vector2.zero;
    Vector2 LineScrollPos = Vector2.zero;
    Vector2 MoveScrollPos = Vector2.zero;
    Vector2 ShakeScrollPos = Vector2.zero;
    Vector2 PoseScrollPos = Vector2.zero;

    int numOfLinesPerScene = 0;
    int charactersAppearing = 0;
    int numBackgrounds = 0;
    int numMovedCharacter = 0;
    int numRemovedChars = 0;
    int numRotatingCharacters = 0;
    int shakeObjects = 0;
    int numOfPosChanges = 0;
    int numOfChoices = 0;

    private ReorderableList list;

    public void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("performableActions"));
        list.onSelectCallback = (ReorderableList l) =>
        {
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index);
        };
    }

    /// <summary>
    /// The function that makes the custom editor work
    /// </summary>
    public override void OnInspectorGUI()
    {
        //Gets the variables from the version of CutsceneTracker.cs its attached to
        CutsceneTracker ct_Script = (CutsceneTracker)target;

        //Displays the Scene actions that will be taking place
        sceneScrollPos = EditorGUILayout.BeginScrollView(sceneScrollPos, GUILayout.Height(300));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("performableActions"), true);
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Decrease Scenes list by 1"))
        {
            Array.Resize(ref ct_Script.performableActions, ct_Script.performableActions.Length - 1);
        }

        EditorGUILayout.Space();

        var CT_Actions = ct_Script.performableActions;
        var serialCT_Actions = serializedObject.FindProperty("performableActions");

        if (CT_Actions.Length > 0)
        {
            if (ct_Script.sceneNames.Length != CT_Actions.Length)
            {
                Array.Resize(ref ct_Script.sceneNames, ct_Script.performableActions.Length);
            }

            for (int i = 0; i < CT_Actions.Length; i++)
            {
                if (serialCT_Actions.GetArrayElementAtIndex(i).isExpanded)
                {
                    for (int j = 0; j < CT_Actions[i].Actions.Length; j++)
                    {
                        switch (CT_Actions[i].Actions[j])
                        {
                            case CutsceneTracker.VN_Actions.Line:
                                numOfLinesPerScene++;
                                break;

                            case CutsceneTracker.VN_Actions.Appear:
                                charactersAppearing++;
                                break;

                            case CutsceneTracker.VN_Actions.Backgound:
                                numBackgrounds++;
                                break;

                            case CutsceneTracker.VN_Actions.Move:
                                numMovedCharacter++;
                                break;

                            case CutsceneTracker.VN_Actions.Remove:
                                numRemovedChars++;
                                break;

                            case CutsceneTracker.VN_Actions.Mirror:
                                numRotatingCharacters++;
                                break;

                            case CutsceneTracker.VN_Actions.Shake:
                                shakeObjects++;
                                break;

                            case CutsceneTracker.VN_Actions.ChangePose:
                                numOfPosChanges++;
                                break;

                            case CutsceneTracker.VN_Actions.Choice:
                                numOfChoices++;
                                break;
                        }
                        // Then call the respective functions down here, this way the exact amount of each string value will be called
                        //NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    }
                    NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    CharacterAppear(ct_Script, charactersAppearing, i);
                    SwitchBackground(ct_Script, numBackgrounds, i);
                    MoveCharacter(ct_Script, numMovedCharacter, i);
                    RemoveCharacter(ct_Script, numRemovedChars, i);
                    RotateCharacter(ct_Script, numRotatingCharacters, i);
                    ShakeObject(ct_Script, shakeObjects, i);
                    ChangeSprite(ct_Script, numOfPosChanges, i);
                    MakeChoice(ct_Script, numOfChoices, i);

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
        }

        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionOverlay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("displayText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("continueText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nameText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PNCOverlay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("panel"));

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
        if (list.isExpanded)
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(1f); ;
    }

    void NumOfInputLines(CutsceneTracker ct_Script, int linesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("GameLines"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("GameNames"));

        if (ct_Script.GameLines.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.GameLines, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.GameNames, ct_Script.performableActions.Length);
        }

        var SceneText = ct_Script.GameLines[whichScene].sceneText;
        var NameText = ct_Script.GameNames[whichScene].sceneText;

        if (linesPerScene != 0 || SceneText.Count != 0 || NameText.Count != 0)
        {
            if (SceneText.Count != linesPerScene || NameText.Count != linesPerScene || SceneText.Count != NameText.Count)
            {
                if (SceneText.Count != NameText.Count)
                {
                    if (SceneText.Count < linesPerScene)
                    {
                        SceneText.Add(null);
                    }
                    else if (SceneText.Count > linesPerScene)
                    {
                        SceneText.RemoveAt(SceneText.Count - 1);
                    }
                    else if (NameText.Count < linesPerScene)
                    {
                        NameText.Add(null);
                    }
                    else if (NameText.Count > linesPerScene)
                    {
                        NameText.RemoveAt(NameText.Count - 1);
                    }
                }

                if (SceneText.Count < linesPerScene || NameText.Count < linesPerScene)
                {
                    SceneText.Add("");
                    NameText.Add("");
                }
                else if (SceneText.Count > linesPerScene || NameText.Count > linesPerScene)
                {
                    SceneText.RemoveAt(SceneText.Count - 1);
                    NameText.RemoveAt(NameText.Count - 1);
                }
            }

            //actionScrollPos = EditorGUILayout.BeginScrollView(actionScrollPos, true, true, GUILayout.Width(400));
            LineScrollPos = EditorGUILayout.BeginScrollView(LineScrollPos, true, true, GUILayout.Height(200));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GameNames").GetArrayElementAtIndex(whichScene), new GUIContent("Name " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GameLines").GetArrayElementAtIndex(whichScene), new GUIContent("Line " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    void CharacterAppear(CutsceneTracker ct_Script, int charactersPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToAppear"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.charactersToAppear.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.charactersToAppear, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var StartPos = ct_Script.charactersToAppear[whichScene].sceneObject;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (charactersPerScene != 0 || StartPos.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (StartPos.Count != charactersPerScene)
            {
                if (StartPos.Count < charactersPerScene)
                {
                    StartPos.Add(ct_Script.transitionOverlay);
                }
                else if (StartPos.Count > charactersPerScene)
                {
                    StartPos.RemoveAt(StartPos.Count - 1);
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToAppear").GetArrayElementAtIndex(whichScene), new GUIContent("Appear " + whichScene));
            EditorGUILayout.EndHorizontal();
        }
    }

    void SwitchBackground(CutsceneTracker ct_Script, int backgroundsPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Backgrounds"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.Backgrounds.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.Backgrounds, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var NewBackground = ct_Script.Backgrounds[whichScene].sceneSprites;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (backgroundsPerScene != 0 || NewBackground.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (NewBackground.Count != backgroundsPerScene)
            {
                if (NewBackground.Count < backgroundsPerScene)
                {
                    NewBackground.Add(ct_Script.backgroundImage.sprite);
                }
                else if (NewBackground.Count > backgroundsPerScene)
                {
                    NewBackground.RemoveAt(NewBackground.Count - 1);
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Backgrounds").GetArrayElementAtIndex(whichScene), new GUIContent("Background " + whichScene));
            EditorGUILayout.EndHorizontal();
        }
    }

    void MoveCharacter(CutsceneTracker ct_Script, int movmentsPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToMove"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("locationsToMove"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("speedtoMove"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.charactersToMove.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.charactersToMove, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.locationsToMove, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.speedtoMove, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var charToMove = ct_Script.charactersToMove[whichScene].sceneObject;
        var locateMove = ct_Script.locationsToMove[whichScene].sceneLocations;
        var moveSpeed = ct_Script.speedtoMove[whichScene].sceneNumbers;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (movmentsPerScene != 0 || charToMove.Count != 0 || locateMove.Count != 0 || moveSpeed.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (charToMove.Count != movmentsPerScene || locateMove.Count != movmentsPerScene || moveSpeed.Count != movmentsPerScene)
            {
                if (charToMove.Count != locateMove.Count || charToMove.Count != moveSpeed.Count)
                {
                    if (charToMove.Count < movmentsPerScene)
                    {
                        charToMove.Add(null);
                    }
                    else if (charToMove.Count > movmentsPerScene)
                    {
                        charToMove.RemoveAt(charToMove.Count - 1);
                    }
                    else if (locateMove.Count < movmentsPerScene)
                    {
                        locateMove.Add(new Vector3());
                    }
                    else if (locateMove.Count > movmentsPerScene)
                    {
                        locateMove.RemoveAt(locateMove.Count - 1);
                    }
                    else if (moveSpeed.Count < movmentsPerScene)
                    {
                        moveSpeed.Add(0);
                    }
                    else if (moveSpeed.Count > movmentsPerScene)
                    {
                        moveSpeed.RemoveAt(moveSpeed.Count - 1);
                    }
                }

                if (charToMove.Count < movmentsPerScene || locateMove.Count < movmentsPerScene || moveSpeed.Count < movmentsPerScene)
                {
                    charToMove.Add(ct_Script.transitionOverlay);
                    locateMove.Add(new Vector3());
                    moveSpeed.Add(0);
                }
                else if (charToMove.Count > movmentsPerScene || locateMove.Count > movmentsPerScene || moveSpeed.Count > movmentsPerScene)
                {
                    charToMove.RemoveAt(charToMove.Count - 1);
                    locateMove.RemoveAt(locateMove.Count - 1);
                    moveSpeed.RemoveAt(moveSpeed.Count - 1);
                }
            }

            MoveScrollPos = EditorGUILayout.BeginScrollView(MoveScrollPos, true, true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speedtoMove").GetArrayElementAtIndex(whichScene), new GUIContent("Speed " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToMove").GetArrayElementAtIndex(whichScene), new GUIContent("Characters " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("locationsToMove").GetArrayElementAtIndex(whichScene), new GUIContent("Locations " + whichScene));
            EditorGUILayout.EndScrollView();
        }
    }

    void RemoveCharacter(CutsceneTracker ct_Script, int removePerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToDisapear"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.charactersToDisapear.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.charactersToDisapear, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var DissapearObjs = ct_Script.charactersToDisapear[whichScene].sceneObject;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (removePerScene != 0 || DissapearObjs.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (DissapearObjs.Count != removePerScene)
            {
                if (DissapearObjs.Count < removePerScene)
                {
                    DissapearObjs.Add(ct_Script.transitionOverlay);
                }
                else if (DissapearObjs.Count > removePerScene)
                {
                    DissapearObjs.RemoveAt(DissapearObjs.Count - 1);
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToDisapear").GetArrayElementAtIndex(whichScene), new GUIContent("Remove " + whichScene));
            EditorGUILayout.EndHorizontal();
        }
    }

    void RotateCharacter(CutsceneTracker ct_Script, int rotatesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToMirror"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.charactersToMirror.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.charactersToMirror, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var MirrorObjs = ct_Script.charactersToMirror[whichScene].sceneObject;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (rotatesPerScene != 0 || MirrorObjs.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (MirrorObjs.Count != rotatesPerScene)
            {
                if (MirrorObjs.Count < rotatesPerScene)
                {
                    MirrorObjs.Add(ct_Script.transitionOverlay);
                }
                else if (MirrorObjs.Count > rotatesPerScene)
                {
                    MirrorObjs.RemoveAt(MirrorObjs.Count - 1);
                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("charactersToMirror").GetArrayElementAtIndex(whichScene), new GUIContent("Mirror " + whichScene));
            EditorGUILayout.EndHorizontal();
        }
    }

    void ShakeObject(CutsceneTracker ct_Script, int shakesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("objectsToShake"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("intensityOfShake"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.objectsToShake.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.objectsToShake, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.intensityOfShake, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var StartPos = ct_Script.objectsToShake[whichScene].sceneObject;
        var NewPos = ct_Script.intensityOfShake[whichScene].sceneNumbers;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (shakesPerScene != 0 || StartPos.Count != 0 || NewPos.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (StartPos.Count != shakesPerScene || NewPos.Count != shakesPerScene)
            {
                if (StartPos.Count < shakesPerScene || NewPos.Count < shakesPerScene)
                {
                    StartPos.Add(ct_Script.transitionOverlay);
                    NewPos.Add(0);
                }
                else if (StartPos.Count > shakesPerScene || NewPos.Count > shakesPerScene)
                {
                    StartPos.RemoveAt(StartPos.Count - 1);
                    NewPos.RemoveAt(NewPos.Count - 1);
                }
            }

            ShakeScrollPos = EditorGUILayout.BeginScrollView(ShakeScrollPos, true, true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("objectsToShake").GetArrayElementAtIndex(whichScene), new GUIContent("Objects " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("intensityOfShake").GetArrayElementAtIndex(whichScene), new GUIContent("Intensity " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    void ChangeSprite(CutsceneTracker ct_Script, int posesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("currentPose"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("newPose"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.currentPose.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.currentPose, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.newPose, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var StartPos = ct_Script.currentPose[whichScene].sceneObject;
        var NewPos = ct_Script.newPose[whichScene].sceneSprites;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (posesPerScene != 0 || StartPos.Count != 0 || NewPos.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (StartPos.Count != posesPerScene || NewPos.Count != posesPerScene || StartPos.Count != NewPos.Count)
            {
                if (StartPos.Count != NewPos.Count)
                {
                    if (StartPos.Count < posesPerScene)
                    {
                        StartPos.Add(null);
                    }
                    else if (StartPos.Count > posesPerScene)
                    {
                        StartPos.RemoveAt(StartPos.Count - 1);
                    }
                    else if (NewPos.Count < posesPerScene)
                    {
                        NewPos.Add(null);
                    }
                    else if (NewPos.Count > posesPerScene)
                    {
                        NewPos.RemoveAt(NewPos.Count - 1);
                    }
                }

                if (StartPos.Count < posesPerScene || NewPos.Count < posesPerScene)
                {
                    StartPos.Add(null);
                    NewPos.Add(null);
                }
                else if (StartPos.Count > posesPerScene || NewPos.Count > posesPerScene)
                {
                    StartPos.RemoveAt(StartPos.Count - 1);
                    NewPos.RemoveAt(NewPos.Count - 1);
                }
            }

            PoseScrollPos = EditorGUILayout.BeginScrollView(PoseScrollPos, true, true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentPose").GetArrayElementAtIndex(whichScene), new GUIContent("Current " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("newPose").GetArrayElementAtIndex(whichScene), new GUIContent("New " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    void MakeChoice(CutsceneTracker ct_Script, int choicesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("choices"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("scenesToGoTo"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.choices.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.choices, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.scenesToGoTo, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var numDecisions = ct_Script.choices[whichScene].nestedDecitions;
        var SceneTransitions = ct_Script.scenesToGoTo[whichScene].nestedSceneBranching;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (choicesPerScene != 0 || numDecisions.Length != 0 || SceneTransitions.Length != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (numDecisions.Length != choicesPerScene || SceneTransitions.Length != choicesPerScene)
            {
                numDecisions = new CutsceneTracker.SceneText[choicesPerScene];
                SceneTransitions = new CutsceneTracker.SceneNumbers[choicesPerScene];
            }

            PoseScrollPos = EditorGUILayout.BeginScrollView(PoseScrollPos, true, true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("choices").GetArrayElementAtIndex(whichScene), new GUIContent("Choices " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scenesToGoTo").GetArrayElementAtIndex(whichScene), new GUIContent("Scene " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    /*void DisplayText(int lineNumber, int x, int y,  CutsceneTracker ct_Script)
    {
        SerializedProperty VN_Lines = serializedObject.FindProperty("Lines");

        SerializedProperty sceneLines = serializedObject.FindProperty("saveRandomPreText");

        if (ct_Script.Lines[ct_Script.Lines.Count-1] != "")
        {
            ct_Script.Lines.Add("");
        }

        //EditorGUILayout.PropertyField(VN_Lines);

        int c = (((x + y) * (x + y + 1)) / 2) + y;

        if (!ct_Script.linesOreintation.Contains(c))
        {
            ct_Script.linesOreintation.Add(c);
        }

        EditorGUILayout.PropertyField(VN_Lines.GetArrayElementAtIndex(lineNumber), new GUIContent(c + " gameObject"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("linesOreintation"));
    }*/
}
