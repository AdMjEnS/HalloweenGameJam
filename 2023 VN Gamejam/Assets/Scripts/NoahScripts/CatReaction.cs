using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatReaction : MonoBehaviour
{
    private AudioSource petSoundSorce;
    private Image sourceImage;
    public Sprite petTurnHeadSprite;
    public Sprite petNetrualSprite;
    //public Button Pet;

    // Start is called before the first frame update
    void Start()
    {
        petSoundSorce = GetComponent<AudioSource>();
        sourceImage = GetComponent<Image>();
    }

    public void petClicked()
    {
        sourceImage.sprite = petNetrualSprite;
        StartCoroutine(waitForEndOfNoice());
    }

    IEnumerator waitForEndOfNoice()
    {
        if (!petSoundSorce.isPlaying)
        {
            petSoundSorce.Play();
            while (petSoundSorce.isPlaying)
            {
                yield return 0;
            }
            sourceImage.sprite = petTurnHeadSprite;
        }
    }
}
