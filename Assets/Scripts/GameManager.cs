using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] CinemachineVirtualCamera cinemachine;
    [SerializeField] float shakeAmplitude = 5f;
    [SerializeField] float shakeDuration = 1f;
    CinemachineBasicMultiChannelPerlin cBMCP;

    public static GameManager instance;

    private void Awake()
    {
        if (!instance) instance = this;
    }

    private void Start()
    {
        cBMCP = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartCameraShake()
    {
        StartCoroutine(CameraShake());
    }

    IEnumerator CameraShake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            cBMCP.m_AmplitudeGain = Mathf.Lerp(shakeAmplitude, 0f, elapsedTime / shakeDuration);
            yield return null;
        }

        cBMCP.m_AmplitudeGain = 0f;
    }
}
