using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadHandTracker : MonoBehaviour
{
    public GameObject _camera;
    public float rotationOffset;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject debugUISample;
    public Canvas canvas;

    private bool didInitializeRotation;
    private Quaternion initialRotation;
    private DebugUISample sample;
    private Text text;
    private float timeDistracted = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // get initial rotation
        didInitializeRotation = false;

        // get ui script to access meditation start
        sample = debugUISample.GetComponent<DebugUISample>();

        // get text component
        text = canvas.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // start the script only if it is meditation time
        if (sample.getIsMeditationTime())
        {
            // initialize the rotation before continuing
            if (!didInitializeRotation)
            {
                // display text to calibrate the head position
                text.text = "Press A to calibrate your initial position.";

                // calibrate upon pressing 'A' and start monitoring
                if (OVRInput.Get(OVRInput.Button.One))
                {
                    didInitializeRotation = true;
                    initialRotation = _camera.transform.rotation;
                    text.text = "";
                }
            }
            // start monitoring the user
            else
            {
                // notify the user if the user has moved their head too much
                Quaternion current = _camera.transform.rotation;
                if (current.x > initialRotation.x + rotationOffset ||
                    current.x < initialRotation.x - rotationOffset ||
                    current.y > initialRotation.y + rotationOffset ||
                    current.y < initialRotation.y - rotationOffset ||
                    current.z > initialRotation.z + rotationOffset ||
                    current.z < initialRotation.z - rotationOffset )
                {
                    text.text = "Please do not move too much";
                    // increment distracted time
                    timeDistracted += Time.deltaTime;
                } else
                {
                    text.text = "";
                }
            }
        }
        // not meditation means no need to initilize rotation
        else
        {
            // if end of meditation, display seconds not concentrated as result
            if (didInitializeRotation)
            {
                int timeDistractedInInt = (int) timeDistracted;
                text.text = "You were not focused for " + timeDistractedInInt + " seconds";
                timeDistracted = 0.0f;
                didInitializeRotation = false;
            }
        }
        
    }
}
