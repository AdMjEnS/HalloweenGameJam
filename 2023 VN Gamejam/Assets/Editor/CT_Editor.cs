using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CutsceneTracker))]
public class CT_Editor : Editor
{
    // The different actions we can have be preformed in the game
    /*public enum VN_Actions
    {
        Line,
        Appear,
        Remove,
        Shake,
        Rotate,
        Move,
        Backgound
    }*/

    // The enum fields that will determine what variables to display in the Inspector
    //public VN_Actions[] preformableActions;

    Vector2 scrollPos = Vector2.zero;
    
    /// <summary>
    /// The function that makes the custom editor work
    /// </summary>
    public override void OnInspectorGUI()
    {
        //Gets the variables from the version of CutsceneTracker.cs its attached to
        CutsceneTracker ct_Script = (CutsceneTracker)target;

        //Displays the Scene actions that will be taking place
        EditorGUILayout.PropertyField(serializedObject.FindProperty("performableActions"), true);

        var CT_Actions = ct_Script.performableActions;
        var serialCT_Actions = serializedObject.FindProperty("performableActions");

        int numOfLinesPerScene = 0;

        if (CT_Actions.Length > 0)
        {
            ct_Script.linesOreintation.Clear();
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
                                CharacterAppear();
                                break;

                            case CutsceneTracker.VN_Actions.Backgound:
                                SwitchBackground();
                                break;

                            case CutsceneTracker.VN_Actions.Move:
                                MoveCharacter();
                                break;

                            case CutsceneTracker.VN_Actions.Remove:
                                RemoveCharacter();
                                break;

                            case CutsceneTracker.VN_Actions.Rotate:
                                RotateCharacter();
                                break;

                            case CutsceneTracker.VN_Actions.Shake:
                                ShakeObject();
                                break;
                        }
                        // Then call the respective functions down here, this way the exact amount of each string value will be called
                        //NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    }
                    NumOfInputLines(ct_Script, numOfLinesPerScene, i);
                    numOfLinesPerScene = 0;
                }                
            }
        }

        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionOverlay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionPicture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("displayText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("continueText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nameText"));
        serializedObject.ApplyModifiedProperties();
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
        yield return null;
    }

    void NumOfInputLines(CutsceneTracker ct_Script, int linesPerScene, int whichScene)
    {
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("GameLines"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("GameNames"));

        try
        {
            if (ct_Script.GameLines[whichScene] == null)
            {
                Array.Resize(ref ct_Script.GameLines, ct_Script.GameLines.Length + 1);
            }
        }
        catch (Exception)
        {
            Array.Resize(ref ct_Script.GameLines, ct_Script.GameLines.Length + 1);
        }

        List<string> SceneText = ct_Script.GameLines[whichScene].sceneText;
        List<string> NameText = ct_Script.GameNames[whichScene].sceneText;

        if (SceneText == null) // TODO Fic
        {
            SceneText.Add("");
            NameText.Add("");
        }

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
        }
        EditorGUILayout.BeginHorizontal();
        //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(100), GUILayout.Width(400));
        //scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("GameNames").GetArrayElementAtIndex(whichScene), new GUIContent("N " + whichScene));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("GameLines").GetArrayElementAtIndex(whichScene), new GUIContent("S " + whichScene));
        //EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
    }

    void CharacterAppear()
    {

    }

    void SwitchBackground()
    {

    }

    void MoveCharacter()
    {

    }

    void RemoveCharacter()
    {

    }

    void RotateCharacter()
    {

    }

    void ShakeObject()
    {

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
