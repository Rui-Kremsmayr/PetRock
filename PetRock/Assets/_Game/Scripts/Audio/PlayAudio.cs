using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    
    [SerializeField] SoundInfoSO _soundInfo;
    [SerializeField] bool _playOnAwake = false;


    void Start()
    {
        if (_playOnAwake)
            AudioManagerWithPool.Instance.Play(_soundInfo, transform.position);
    }

    public void PlaySound() => AudioManagerWithPool.Instance.Play(_soundInfo, transform.position);


}
