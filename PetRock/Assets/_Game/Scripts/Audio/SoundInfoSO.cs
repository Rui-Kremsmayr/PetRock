using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundInfo", menuName = "Audio/Sound Info", order = 0)]
public class SoundInfoSO : ScriptableObject
{
    [SerializeField] AudioClip[] _clips;
    [SerializeField] AudioMixerGroup _mixerGroup;
    [SerializeField] float _volume = 1;
    [SerializeField] Vector2 _minMaxPitch = new(0.9f, 1.1f);
    [SerializeField] bool _is3D = false;
    [SerializeField] bool _loop = false;


    public AudioMixerGroup MixerGroup => _mixerGroup;
    public float Volume => _volume;
    public bool Is3D => _is3D;
    public bool Loop => _loop;
    
    public AudioClip GetRandomClip() => _clips[Random.Range(0, _clips.Length)];
    public float GetRandomPitch() => Random.Range(_minMaxPitch.x, _minMaxPitch.y);
}


