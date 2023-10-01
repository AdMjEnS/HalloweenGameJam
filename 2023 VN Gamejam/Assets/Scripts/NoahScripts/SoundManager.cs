using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource backgroundAudio;
    public AudioSource beepboopSource;
    public AudioSource duringLinesVoice;
    public AudioSource doorOpenAudio;
    public AudioSource doorCloseAudio;
    public AudioSource cardAudio;
    public CutsceneTracker ct;

    [Serializable]
    public class PreAudio
    {
        public AudioClip[] preAudio;
    }

    [Serializable]
    public class PostAudio
    {
        public AudioClip[] postAudio;
    }

    /*
     * 0 - No Sound
     * 1 - Bleep Bloop Speaking Sound
     * 2 - Voice Over Audio
     */

    [Range(0, 2)]
    public int audioSelector;
    public static int audioVersion;

    public AudioClip[] lineBeepBoops;
    public PreAudio[] linesAudio;
    public AudioClip[] cardDrawSounds;
    public AudioClip[] footSteps;
    public AudioClip[] backgroundMusic;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip cardsShuffle;
}
