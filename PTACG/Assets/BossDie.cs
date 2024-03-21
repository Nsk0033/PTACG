using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    [SerializeField]private Transform portal;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown60());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Countdown60()
    {
        yield return new WaitForSeconds(60);
        GameObject.Destroy(this.gameObject);
        portal.gameObject.SetActive(true);
    }
}
