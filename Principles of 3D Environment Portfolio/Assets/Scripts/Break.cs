using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour
{
    [SerializeField] private GameObject broken_object;

    [SerializeField] private ParticleSystem[] particles;

    public float breaking_force;

    public void playBreakAnim()
    {
        playParticles();
        breakObject();
    }

    private void breakObject()
    {
        GameObject new_object = Instantiate(broken_object, transform.position, transform.rotation);

        foreach (Rigidbody rb in new_object.GetComponentsInChildren<Rigidbody>())
        {
            Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * breaking_force;
            rb.AddForce(force);
        }

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
