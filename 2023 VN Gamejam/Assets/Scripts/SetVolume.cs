using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    private Slider slider;

    public string sliderName;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(sliderName);
        mixer.SetFloat(sliderName, Mathf.Log10(slider.value) * 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        //slider = GetComponent<Slider>();

        //slider.value = PlayerPrefs.GetFloat(sliderName);
        SetLevel(PlayerPrefs.GetFloat(sliderName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLevel(float value)
    {
        mixer.SetFloat(sliderName, Mathf.Log10(value) * 20);

        PlayerPrefs.SetFloat(sliderName, value);
    }    
}
