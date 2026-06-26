using UnityEngine;
using UnityEngine.Pool;

public class AudioPool
{

    ObjectPool<AudioSource> _pool;

    public AudioPool(int defaultCapacity = 20, int maxSize = 50)
    {
        _pool = new(
            createFunc: CreateItem,
            actionOnGet: OnGetItem,
            actionOnRelease: OnReleaseItem,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public AudioSource GetItem(Transform parent)
    {
        AudioSource source = _pool.Get();
        source.transform.SetParent(parent);
        return source;
    }

    public void ReleaseItem(AudioSource audioSource) => _pool.Release(audioSource);

    AudioSource CreateItem()
    {
        GameObject go = new("Audio Source");
        AudioSource src = go.AddComponent<AudioSource>();
        go.SetActive(false);

        return src;
    }

    void OnGetItem(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(true);
    }

    void OnReleaseItem(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(false);
    }

    void OnDestroyItem(AudioSource audioSource)
    {
        Object.Destroy(audioSource.gameObject);
    }
}
