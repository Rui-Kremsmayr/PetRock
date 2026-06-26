using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerSimple : MonoBehaviour
{
    public static AudioManagerSimple Instance;

    [SerializeField] AudioMixer _mixer;
    [SerializeField] AudioSource _audioSourcePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public AudioSource Play(SoundInfoSO soundInfo, Vector3 pos, ulong delay = 0)
    {
        if (soundInfo == null)
        {
            Debug.LogWarning("[AudioManager.cs] Cannot play the sound as the sound info is null.");
            return null;
        }

        AudioSource src = Instantiate(_audioSourcePrefab, pos, Quaternion.identity, transform);
        src.spatialBlend = soundInfo.Is3D ? 1 : 0;
        src.volume = soundInfo.Volume;
        src.loop = soundInfo.Loop;
        src.outputAudioMixerGroup = soundInfo.MixerGroup;
        src.pitch = soundInfo.GetRandomPitch();
        src.clip = soundInfo.GetRandomClip();

        src.Play(delay);

        if (!src.loop)
            Destroy(src.gameObject, src.clip.length + delay);

        return src;
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Pause(AudioSource audioSource, bool pause)
    {
        if (pause)
            audioSource.Pause();
        else
            audioSource.UnPause();
    }

    public void PauseAllAudio(bool pause)
    {
        AudioListener.pause = pause;

        // to set specific audio sources to keep playing (e.g. Pause Menu Music)
        // audioSource.ignoreListenerPause = true;
    }

    public void SetAudioMixerVolume(string exposedVolumeParam, float volumeSliderValue)
    {
        _mixer.SetFloat(exposedVolumeParam, Mathf.Log10(volumeSliderValue) *20);
    }

}



