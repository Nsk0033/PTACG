using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOpen : MonoBehaviour
{
    [SerializeField] private GameObject InteractMsg;
    private Animator BoxOpenAnimation;

    private void Start()
    {
        InteractMsg.SetActive(false);
        BoxOpenAnimation = GetComponentInChildren<Animator>();
        Debug.Log(BoxOpenAnimation);
    }

    private void Update()
    {
        if (BoxOpenAnimation != null) 
        {
            if (InteractMsg.activeSelf) 
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    BoxOpenAnimation.SetTrigger("BoxOpen");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            InteractMsg.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractMsg.SetActive(false);
        }
    }
}
