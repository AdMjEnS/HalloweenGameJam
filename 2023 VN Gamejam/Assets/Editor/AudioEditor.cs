using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SoundManager))]
public class AudioEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        SoundManager audioScript = (SoundManager)target;

        //EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundAudio"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duringLinesVoice"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ct"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSelector"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("lineBeepBoops"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("linesAudio"));

        if (audioScript.linesAudio.Length != audioScript.ct.GameLines.Length)
        {
            Array.Resize(ref audioScript.linesAudio, audioScript.ct.GameLines.Length);
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }*/
    
}
