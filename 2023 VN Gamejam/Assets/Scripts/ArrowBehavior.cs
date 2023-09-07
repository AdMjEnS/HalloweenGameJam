using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowBehavior : MonoBehaviour
{
    //the current background
    public GameObject currentBackground;

    //sets the background to a new one based on direction clicked and chosen new background
    public void OnClick(GameObject NewBG)
    {
        NewBG.SetActive(true);
        currentBackground.SetActive(false);

    }
}
