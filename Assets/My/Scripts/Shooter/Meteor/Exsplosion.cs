using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exsplosion : MonoBehaviour
{
#pragma warning disable 0649
    [Range(0,7f)]
    [SerializeField] private float lifeTime;
    [SerializeField] private List<AudioSource> explosionSongs;
#pragma warning restore 0649

    private void Start()
    {       
        if (GameC.Instance.GetData().toggleSoundEffect)
            explosionSongs[Random.Range(0, explosionSongs.Count)].Play();

        Destroy(gameObject, Random.Range(lifeTime - 0, lifeTime + 0));
    }
}
