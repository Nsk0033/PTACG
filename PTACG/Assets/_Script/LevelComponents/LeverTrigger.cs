using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    [SerializeField] private GameObject poisonGas;
    [SerializeField] private GameObject text;
    [SerializeField] private Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private bool canTakeAction;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true);
            canTakeAction = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                spriteRenderer.sprite = sprite;
                poisonGas.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }
}
