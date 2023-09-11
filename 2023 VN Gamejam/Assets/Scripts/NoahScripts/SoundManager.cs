using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] fastMusic;
    public AudioClip[] slowMusic;
    public AudioClip[] savingRandomGameAudio;
    public AudioClip[] savingFamilyGameAudio;
    public AudioClip[] relaxingNatureGameAudio;
    public AudioClip[] relaxingBreathingGameAudio;
    public AudioClip[] savoringGameAudio;
    public AudioClip[] paridoxicalIntentionsGameAudio;
    public AudioClip[] TimeSeneitiveGameAudio;
    public AudioSource transitionAudio;
    public AudioSource duringGameVoice;
    public GameManager gm;

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

    public AudioClip[] savingRandomStartAudio;
    public PreAudio[] savingRandomPreAudio;
    public PostAudio[] savingRandomPostAudio;
    public AudioClip[] savingRandomEndAudio;
    public AudioClip[] savingFamilyStartAudio;
    public PreAudio[] savingFamilyPreAudio;
    public PostAudio[] savingFamilyPostAudio;
    public AudioClip[] savingFamilyEndAudio;
    public AudioClip[] relaxingNatureStartAudio;
    public PreAudio[] relaxingNaturePreAudio;
    public PostAudio[] relaxingNaturePostAudio;
    public AudioClip[] relaxingNatureEndAudio;
    public AudioClip[] relaxingBreathingStartAudio;
    public PreAudio[] relaxingBreatingPreAudio;
    public PostAudio[] relaxingBreathingPostAudio;
    public AudioClip[] relaxingBreathingEndAudio;
    public AudioClip[] savoringStartAudio;
    public PreAudio[] savoringPreAudio;
    public PostAudio[] savoringPostAudio;
    public AudioClip[] savoringEndAudio;
    public AudioClip[] paridoxStartAudio;
    public PreAudio[] paridoxPreAudio;
    public PostAudio[] paridoxPostAudio;
    public AudioClip[] paridoxEndAudio;
    public AudioClip[] noReasonStartAudio;
    public PreAudio[] noReasonPreAudio;
    public PostAudio[] noReasonPostAudio;
    public AudioClip[] noReasonEndAudio;
}
