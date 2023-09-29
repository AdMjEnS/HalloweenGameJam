using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image viewImage1, viewImage2;
    public List<Image> bGImages = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }

    private void ScrollBackgroundImages()
    {
        
    }
}
