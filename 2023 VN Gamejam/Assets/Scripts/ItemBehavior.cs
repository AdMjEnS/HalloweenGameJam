using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameObject manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void CollectObject()
    {
        manager.GetComponent<CutsceneTracker>().count--;
        Destroy(this.gameObject);
        //Need to add actual functionality later
    }
}
