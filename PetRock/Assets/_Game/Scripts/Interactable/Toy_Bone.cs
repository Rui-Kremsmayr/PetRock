using UnityEngine;

public class Toy_Bone : Toy
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.TryGetComponent(out PetRock petRock))
        {
            AudioManagerWithPool.Instance.Play(_interactionSound, transform.position);

            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            
            transform.SetParent(petRock.HoldingPos);
            transform.localPosition = Vector3.zero;

            PetRock.OnChangeState?.Invoke(PetRock.State.Waiting, null);
        }
    }
}
