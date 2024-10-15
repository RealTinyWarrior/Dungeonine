using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Controls Music and SFX volumes
public class MasterVolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);

        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        PlayerPrefs.Save();
    }
}
