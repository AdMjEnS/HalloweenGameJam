using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameObject manager;
    public GameObject listText;
    public SoundManager soundManager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
        soundManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
    }

    public void CollectObject()
    {
        soundManager.itemAudio.clip = soundManager.itemGet;
        soundManager.itemAudio.Play();
        manager.GetComponent<CutsceneTracker>().count--;
        Destroy(listText);
        Destroy(this.gameObject);
        //Need to add actual functionality later
    }
}

