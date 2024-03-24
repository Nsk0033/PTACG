using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    [SerializeField]private Transform portal;
    [SerializeField]private Transform player;
    [SerializeField]private TextMeshProUGUI textMeshPro;

    // Update is called once per frame
    void Update()
    {
        if (player.gameObject.activeSelf == false)
        {
            StopAllCoroutines();
            textMeshPro.text = "Press P to resurrect";
        }

        else
        {
            StartCoroutine(Countdown30());
            textMeshPro.text = "Survive for 30 seconds";
        }
    }

    private IEnumerator Countdown30()
    {
        yield return new WaitForSeconds(30);
        GameObject.Destroy(this.gameObject);
        portal.gameObject.SetActive(true);
    }
}
