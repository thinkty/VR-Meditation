using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

// Show off all the Debug UI components.
public class DebugUISample : MonoBehaviour
{
    public GameObject focusPoint;
    public int meditationTime;
    public AudioSource audioGuide;
    public AudioSource[] environment;

    bool inMenu;
    private Text sliderText;
    private Slider sliderPrefab;
    private Text sliderText2;
    private Slider sliderPrefab2;
    private Text timer;
    private Text session;
    private bool isMeditationTime;
    DateTime oldDate;
    DateTime currentDate;

    public bool getIsMeditationTime()
    {
        return isMeditationTime;
    }


    void Start ()
    {
        // option
        isMeditationTime = false;
        var manageSession = DebugUIBuilder.instance.AddButton("Start Session", ToggleMeditationSession);
        session = manageSession.GetComponentInChildren<Button>().GetComponentInChildren<Text>();
        DebugUIBuilder.instance.AddDivider();

        DebugUIBuilder.instance.AddLabel("Session Configuration");
        focusPoint.SetActive(false);
        DebugUIBuilder.instance.AddToggle("Toggle Focus Point", TogglePressed, false);

        // Guide Volume
        var sliderPrefab = DebugUIBuilder.instance.AddSlider("Guide Volume", 0.0f, 100.0f, SliderPressed, true);
        var textElementsInSlider = sliderPrefab.GetComponentsInChildren<Text>();
        textElementsInSlider[0].text = "Guide Volume";
        Assert.AreEqual(textElementsInSlider.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        sliderText = textElementsInSlider[1];
        Assert.IsNotNull(sliderText, "No text component on slider prefab");
        this.sliderPrefab = sliderPrefab.GetComponentInChildren<Slider>();
        sliderText.text = this.sliderPrefab.value.ToString();
        audioGuide.volume = this.sliderPrefab.value / 100.0f;

        // Environment Volume
        var sliderPrefab2 = DebugUIBuilder.instance.AddSlider("Environment Volume", 0.0f, 100.0f, EnvironemntSliderPressed, true);
        var textElementsInSlider2 = sliderPrefab2.GetComponentsInChildren<Text>();
        textElementsInSlider2[0].text = "Environment Volume";
        Assert.AreEqual(textElementsInSlider2.Length, 2, "Slider prefab format requires 2 text components (label + value)");
        sliderText2 = textElementsInSlider2[1];
        Assert.IsNotNull(sliderText2, "No text component on slider prefab");
        this.sliderPrefab2 = sliderPrefab2.GetComponentInChildren<Slider>();
        sliderText2.text = this.sliderPrefab2.value.ToString();


        // Time setting
        DebugUIBuilder.instance.AddToggle("Time", TimeTogglePressed, false);

        // Buttons to increase, decrease timer time
        DebugUIBuilder.instance.AddButton("Add 1 min", IncreaseMeditationTime);
        DebugUIBuilder.instance.AddButton("Sub 1 min", DecreaseMeditationTime);

        // Set meditation timer
        var labelPrefab = DebugUIBuilder.instance.AddLabel("");
        timer = labelPrefab.GetComponentInChildren<Text>();
        timer.text = "Timer : " + meditationTime + " min.";

        DebugUIBuilder.instance.Show();
        inMenu = true;
	}

    public void TogglePressed(Toggle t)
    {
        focusPoint.SetActive(t.isOn);
        Debug.Log("Focus Button Toggle pressed. Is on? "+t.isOn);
    }
    public void EnvironemntSliderPressed(float f)
    {
        // control sound of environment
        Debug.Log("Slider: " + f);
        sliderText2.text = f.ToString();
        // iterate through all the environment audio sources and change volume
        for (int i = 0; i < environment.Length; i++)
        {
            environment[i].volume = this.sliderPrefab2.value / 100.0f;
        }
    }
    public void SliderPressed(float f)
    {
        // control sound of audio guide
        sliderText.text = f.ToString();
        audioGuide.volume = this.sliderPrefab.value / 100.0f;
    }
    public void TimeTogglePressed(Toggle t)
    {
        // TODO: Set day or night
    }
    public void IncreaseMeditationTime()
    {
        // Increase meditation time by 1 minute
        this.meditationTime++;
        timer.text = "Timer : " + meditationTime + " min.";
    }
    public void DecreaseMeditationTime()
    {
        // Decrease meditation time by 1 minute
        this.meditationTime--;
        timer.text = "Timer : " + meditationTime + " min.";
    }

    void ToggleMeditationSession()
    {
        isMeditationTime = !isMeditationTime;
        if (isMeditationTime)
        {
            // update label
            session.text = "Stop Session";

            // let it update meditation timer
            isMeditationTime = true;
            oldDate = System.DateTime.Now;

            // start audio guide
            audioGuide.Play();
        }
        else
        {
            // update label
            session.text = "Start Session";

            isMeditationTime = false;
            timer.text = "Timer : " + meditationTime + " min.";

            // stop audio guide
            audioGuide.Stop();
        }
    }

    void Update()
    {
        // update the meditation timer
        if (isMeditationTime)
        {
            currentDate = System.DateTime.Now;
            int minutes = currentDate.Minute - oldDate.Minute; // minutes passed

            // meditation done
            if (meditationTime - minutes <= 0)
            {
                // turn of meditation
                ToggleMeditationSession();
            }
            else
            {
                timer.text = "Timer : " + (meditationTime - minutes) + " min.";
            }
        }

    }


}
