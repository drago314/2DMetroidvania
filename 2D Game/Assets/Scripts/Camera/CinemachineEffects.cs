using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CinemachineEffects : MonoBehaviour
{
    public static CinemachineEffects instance;

    [SerializeField] private float punchEffectScale;
    [SerializeField] private float punchEffectTime;

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float defaultOrthographicSize;

    private float shakeTimer;

    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        defaultOrthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cBMCP =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cBMCP.m_AmplitudeGain = 0;
            }
        }
    }

    public void Punch()
    {
        float newSize = defaultOrthographicSize + punchEffectScale;
        float orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        Tween t = DOTween.To(() => orthographicSize, x => orthographicSize = x, newSize, punchEffectTime);
        t.OnUpdate(() =>
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
        });

        t.OnComplete(() => {
            Tween k = DOTween.To(() => orthographicSize, x => orthographicSize = x, defaultOrthographicSize, punchEffectTime);
            k.OnUpdate(() => {
                cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
            });
        });
    }

    public void Shake(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cBMCP =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cBMCP.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}
