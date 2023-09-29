using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatReaction : MonoBehaviour
{
    public Sprite catTurnHeadSprite;
    public Sprite catNetrualSprite;
    public Button Cat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void catClicked()
    {
        var currentCatSprite = GetComponent<Sprite>();
        currentCatSprite = catTurnHeadSprite;
        StartCoroutine(catMeow(currentCatSprite));
    }

    IEnumerator catMeow(Sprite currentCatSprite)
    {
        yield return new WaitForSecondsRealtime(2);
        currentCatSprite = catNetrualSprite;
    }
}
