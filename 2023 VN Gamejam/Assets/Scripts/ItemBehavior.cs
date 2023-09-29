using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameObject manager;
    public GameObject listText;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void CollectObject()
    {
        manager.GetComponent<CutsceneTracker>().count--;
        Destroy(listText);
        Destroy(this.gameObject);
        //Need to add actual functionality later
    }
}
