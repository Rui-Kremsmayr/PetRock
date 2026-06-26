using System;
using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    [SerializeField] Transform _bowlPos;
    [SerializeField] Transform _feedingPos;
    [SerializeField] float _feedingTime;
    [SerializeField] SoundInfoSO _thumpSound;
    [SerializeField] SoundInfoSO _interactionSound;

    Transform _origParent;
    Vector3 _startPos;

    public event Action OnFinishedInteraction;

    void Start()
    {
        _origParent = transform.parent;
        _startPos = transform.position;
    }

    public void OnClick()
    {
        Debug.Log("Clicked on Food");

        AudioManagerWithPool.Instance.Play(_thumpSound, transform.position);

        transform.SetParent(_bowlPos);
        transform.position = _bowlPos.position;

        PetRock.OnChangeState?.Invoke(PetRock.State.Eating, _feedingPos);
        PetRock.OnArrivedAtSpecificPosition += StartFeedingTime;
    }

    void StartFeedingTime()
    {
        PetRock.OnArrivedAtSpecificPosition -= StartFeedingTime;
        StartCoroutine(Feed());
    }

    IEnumerator Feed()
    {
        AudioManagerWithPool.Instance.Play(_interactionSound, transform.position);

        yield return new WaitForSeconds(_feedingTime);

        Debug.Log("End Feeding Time");
        transform.SetParent(_origParent);
        transform.position = _startPos;

        PetRock.OnChangeState?.Invoke(PetRock.State.Wandering, null);
        OnFinishedInteraction?.Invoke();
    }

    public void OnPointerDown()
    {
        
    }

    public void OnPointerUp()
    {
        
    }
}
