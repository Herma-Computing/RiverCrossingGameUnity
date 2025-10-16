using UnityEngine;

public class PaddleSplash : MonoBehaviour
{
    public ParticleSystem splashEffect; // assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Paddle hit water!");

            // Move splash to paddle position
            splashEffect.transform.position = transform.position;

            // Play splash effect
            splashEffect.Play();
        }
    }
}
