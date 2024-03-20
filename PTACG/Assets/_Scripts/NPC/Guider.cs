using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guider : MonoBehaviour
{
    [SerializeField] private GameObject instructionCanva;
    private bool closeToGuider;
    public bool isCloseToGuider { get; private set; }

    private void Start() 
    {
        instructionCanva.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            instructionCanva.SetActive(true);
            closeToGuider = true;
            isCloseToGuider = closeToGuider;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            instructionCanva.SetActive(false);
            closeToGuider = false;
            isCloseToGuider = closeToGuider;
        }
    }
}
