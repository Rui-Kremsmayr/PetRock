using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Toy : MonoBehaviour, IInteractable
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected float _throwForce = 3;
    [SerializeField] SoundInfoSO _swooshSound;
    [SerializeField] protected SoundInfoSO _interactionSound;

    Vector3 _startPos;
    Quaternion _startRot;
    Transform _parent;

    public event Action OnFinishedInteraction;

    void Start()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;

        _startPos = transform.position;
        _startRot = transform.rotation;
        _parent = transform.parent;
    }

    public virtual void OnClick()
    {
        Debug.Log("OnClick");

        transform.SetParent(null);

        _rb.useGravity = true;
        _rb.isKinematic = false;

        AudioManagerWithPool.Instance.Play(_swooshSound, transform.position);

        Vector3 direction = (transform.position - Camera.main.transform.position).normalized + new Vector3(0, 1, 0); 
        _rb.AddForce(direction * _throwForce, ForceMode.Impulse);

        PetRock.OnChangeState?.Invoke(PetRock.State.Playing, transform);

        OnFinishedInteraction?.Invoke();
        PetRock.OnIsWaiting += CalledBack;
    }

    void CalledBack(bool waiting)
    {
        PetRock.OnIsWaiting -= CalledBack;
        
        _rb.isKinematic = true;
        _rb.useGravity = false;
        transform.parent = _parent;
        transform.SetPositionAndRotation(_startPos, _startRot);
    }        

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
    }

    public void OnPointerDown()
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp()
    {
        Debug.Log("OnPointerUp");
    }
}
