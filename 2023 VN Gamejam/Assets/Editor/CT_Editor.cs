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
    Vector2 actionScrollPos = Vector2.zero;

    int numOfLinesPerScene = 0;
    int charactersAppearing = 0;
    int numBackgrounds = 0;
    int numMovedCharacter = 0;
    int numRemovedChars = 0;
    int numRotatingCharacters = 0;
    int shakeObjects = 0;
    int numOfPosChanges = 0;

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

        if (GUILayout.Button("Change Scene Name"))
        {
            EditorGUILayout.SelectableLabel("Change Scene Name");
        }

        EditorGUILayout.Space();

        EditorGUILayout.TextField("Input Scene Name Here");

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Don't touch the +- buttons for the lists bellow");

        EditorGUILayout.Space();

        var CT_Actions = ct_Script.performableActions;
        var serialCT_Actions = serializedObject.FindProperty("performableActions");

        if (CT_Actions.Length > 0)
        {
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
                        }
                        // Then call the respective functions down here, this way the exact amount of each string value will be called
                        //NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    }
                    NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    CharacterAppear(ct_Script, charactersAppearing, i);
                    SwitchBackground(ct_Script, charactersAppearing, i);
                    MoveCharacter(ct_Script, charactersAppearing, i);
                    RemoveCharacter(ct_Script, charactersAppearing, i);
                    RotateCharacter(ct_Script, charactersAppearing, i);
                    ShakeObject(ct_Script, charactersAppearing, i);
                    ChangePose(ct_Script, numOfPosChanges, i);

                    numOfLinesPerScene = 0;
                    charactersAppearing = 0;
                    numBackgrounds = 0;
                    numMovedCharacter = 0;
                    numRemovedChars = 0;
                    numRotatingCharacters = 0;
                    shakeObjects = 0;
                    numOfPosChanges = 0;
                }                
            }
        }

        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionOverlay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionPicture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("displayText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("continueText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nameText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PNCOverlay"));

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

        if (linesPerScene != 0 || SceneText.Count != 0)
        {
            if (SceneText.Count != linesPerScene)
            {
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
            actionScrollPos = EditorGUILayout.BeginScrollView(actionScrollPos, true, true);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GameNames").GetArrayElementAtIndex(whichScene), new GUIContent("N " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GameLines").GetArrayElementAtIndex(whichScene), new GUIContent("S " + whichScene));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }

    void CharacterAppear(CutsceneTracker ct_Script, int charactersPerScene, int whichScene)
    {

    }

    void SwitchBackground(CutsceneTracker ct_Script, int backgroundsPerScene, int whichScene)
    {

    }

    void MoveCharacter(CutsceneTracker ct_Script, int movmentsPerScene, int whichScene)
    {

    }

    void RemoveCharacter(CutsceneTracker ct_Script, int removePerScene, int whichScene)
    {

    }

    void RotateCharacter(CutsceneTracker ct_Script, int rotatesPerScene, int whichScene)
    {

    }

    void ShakeObject(CutsceneTracker ct_Script, int shakesPerScene, int whichScene)
    {

    }

    void ChangePose(CutsceneTracker ct_Script, int posesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("currentPos"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("newPos"));

        //Makes sure the length of Position arrays are the same as the Performable Action arrays
        if (ct_Script.currentPos.Length != ct_Script.performableActions.Length)
        {
            Array.Resize(ref ct_Script.currentPos, ct_Script.performableActions.Length);
            Array.Resize(ref ct_Script.newPos, ct_Script.performableActions.Length);
        }

        //Instanciates them into varables
        var StartPos = ct_Script.currentPos[whichScene].sceneObject;
        var NewPos = ct_Script.newPos[whichScene].sceneObject;

        //If there is any integer bigger then zero then it constantly checks to make sure that everything is displayed corectly
        if (posesPerScene != 0 || StartPos.Count != 0 || NewPos.Count != 0)
        {
            //If the number of pose changes in either array's don't match then this makes sure that they do
            if (StartPos.Count != posesPerScene || NewPos.Count != posesPerScene)
            {
                if (StartPos.Count < posesPerScene || NewPos.Count < posesPerScene)
                {
                    StartPos.Add(ct_Script.transitionOverlay);
                    NewPos.Add(ct_Script.transitionOverlay);
                }
                else if (StartPos.Count > posesPerScene || NewPos.Count > posesPerScene)
                {
                    StartPos.RemoveAt(StartPos.Count - 1);
                    NewPos.RemoveAt(NewPos.Count - 1);
                }
            }

            EditorGUILayout.LabelField("Don't touch the +- buttons for the lists bellow");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentPos").GetArrayElementAtIndex(whichScene), new GUIContent("C " + whichScene));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("newPos").GetArrayElementAtIndex(whichScene), new GUIContent("N " + whichScene));
            EditorGUILayout.EndHorizontal();
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
