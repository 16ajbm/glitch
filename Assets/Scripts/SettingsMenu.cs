using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Start()
    {
        volumeSlider.value = AudioManager.GetVolume(audioMixer);
    }

    public void OnVolumeChange(float volume)
    {
        AudioManager.SetVolume(audioMixer, volume);
    }
}
