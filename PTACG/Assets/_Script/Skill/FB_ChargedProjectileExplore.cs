using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_ChargedProjectileExplore : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private void OnDestroy()
    {
        Vector3 explorePosition = transform.position;
        Instantiate(explosion, explorePosition, Quaternion.identity);
    }
}
