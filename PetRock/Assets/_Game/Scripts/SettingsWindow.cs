using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    
    [SerializeField] GameObject _window;
    [SerializeField] InputActionReference _pauseKey;

    [Space]

    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _sfxVolumeSlider;
    [SerializeField] Toggle _muteToggle;


    void Start()
    {
        _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        _muteToggle.onValueChanged.AddListener(MuteSound);

        _pauseKey.action.performed += ToggleWindow;
    }

    void OnDestroy()
    {
        _pauseKey.action.performed -= ToggleWindow;
    }

    void ToggleWindow(InputAction.CallbackContext context) => _window.SetActive(!_window.activeInHierarchy);

    void SetMasterVolume(float volume) => AudioManagerWithPool.Instance.SetAudioMixerVolume("MasterVolume", volume);
    void SetMusicVolume(float volume) => AudioManagerWithPool.Instance.SetAudioMixerVolume("MusicVolume", volume);
    void SetSfxVolume(float volume) => AudioManagerWithPool.Instance.SetAudioMixerVolume("SfxVolume", volume);

    void MuteSound(bool mute) => AudioManagerWithPool.Instance.PauseAllAudio(mute);


}
