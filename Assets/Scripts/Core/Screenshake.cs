using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class Screenshake : MonoBehaviour
{
    public static Screenshake Instance;
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance != null) {
            Debug.LogError($"There's more than one Screenshake! {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
