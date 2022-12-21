using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPath : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    public void StopParticle()
    {
        particleSystem.Stop();
        Destroy(gameObject, 1);
    }

}
