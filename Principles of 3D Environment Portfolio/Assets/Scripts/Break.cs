using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    [SerializeField] private GameObject broken_object;

    [SerializeField] private ParticleSystem[] particles;

    public void playBreakAnim()
    {
        playParticles();
        breakObject();
    }

    private void breakObject()
    {
        Instantiate(broken_object, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void playParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            var emission = particle.emission;
            emission.enabled = true;
        }

        particles[0].Play();
        particles[1].Play();
    }
}
