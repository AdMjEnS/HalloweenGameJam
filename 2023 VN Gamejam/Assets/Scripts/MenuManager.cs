using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public Image viewImage1, viewImage2;
    public List<GameObject> allPanels = new List<GameObject>();
    public List<Button> allButtons = new List<Button>();
    public AudioMixer audioMixer;

    //public float scrollSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        SetAllPanelsOff();

        foreach (Button buttons in allButtons)
        {
            buttons.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScrollBackgroundImages();
    }

    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();

    }

    public void ActivatePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
        SetAllButtonsDisabled();
    }

    private void ScrollBackgroundImages()
    {
        //viewImage1.transform.position = viewImage1.transform.position - new Vector3((viewImage1.transform.position.x * scrollSpeed), viewImage1.transform.position.y);
    }

    private void SetAllPanelsOff()
    {
        foreach(GameObject panel in allPanels)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private void SetAllButtonsDisabled()
    {
        foreach(Button buttons in allButtons)
        {
            buttons.interactable = !buttons.interactable;
        }
    }
}
