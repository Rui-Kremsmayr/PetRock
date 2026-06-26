using UnityEngine;

public class Toy_Ball : Toy
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        // if (collision.gameObject.TryGetComponent(out PetRock petRock))
        if (collision.gameObject.CompareTag("PetRock"))
        {
            AudioManagerWithPool.Instance.Play(_interactionSound, transform.position);

            Vector3 direction = (transform.position - collision.transform.position).normalized; 
            _rb.AddForce(direction * _throwForce, ForceMode.Impulse);
        }        
    }
}
