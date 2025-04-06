using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Awake()
    {
        AudioManager.GetVolume(audioMixer);
    }

    public static float GetVolume(AudioMixer mixer)
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        mixer.SetFloat("volume", savedVolume);
        return savedVolume;
    }

    public static void SetVolume(AudioMixer mixer, float volume)
    {
        mixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
