using UnityEngine;

public class SplashParticle : MonoBehaviour
{
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        gameObject.SetActive(false); // keep disabled until triggered
    }

    void OnEnable()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        ps.Play(); // play automatically when enabled
    }

    void Update()
    {
        // When the particle system is finished, disable the object
        if (gameObject.activeSelf && ps != null && !ps.IsAlive(true))
        {
            gameObject.SetActive(false);
        }
    }
}
