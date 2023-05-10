using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class SimpleCameraShake : MonoBehaviour
{

    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private Cowboy cowboy;
    // Use this for initialization
    void Start()
    {
        cowboy = GetComponent<Cowboy>();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Replace with your trigger
        if (Input.GetButton("Fire1")&&cowboy.startShake)
        {
            ShakeElapsedTime = ShakeDuration;
        }
        /*if (cowboy.startShake)
        {
            ShakeElapsedTime = ShakeDuration;
        }*/

        // If the Cinemachine componet is not set, avoid update
          // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
        if (ShakeElapsedTime <= 0)
        {
            // If Camera Shake effect is over, reset variables
            virtualCameraNoise.m_AmplitudeGain = 0.64f;
            virtualCameraNoise.m_FrequencyGain = 0.15f;
            ShakeElapsedTime = 0f;
        }
    }
}