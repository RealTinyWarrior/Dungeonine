using UnityEngine;
using UnityEngine.Rendering;
using URPGlitch.Runtime.AnalogGlitch;

public class FloorGlitchIntensity : MonoBehaviour
{
    [Range(0, 1)]
    public float jitter;
    [Range(0, 1)]
    public float verticalJump;
    [Range(0, 1)]
    public float horizontalJump;
    [Range(0, 1)]
    public float colorDrift;
    public Volume volume;

    void Start()
    {
        if (volume.profile.TryGet<AnalogGlitchVolume>(out var volumeGlitch))
        {
            volumeGlitch.scanLineJitter.value = jitter;
            volumeGlitch.verticalJump.value = verticalJump;
            volumeGlitch.horizontalShake.value = horizontalJump;
            volumeGlitch.colorDrift.value = colorDrift;
        }
    }
}