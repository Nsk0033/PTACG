using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITABossTrigger : MonoBehaviour
{
	public bool hasPlayerEnter;
	
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
			hasPlayerEnter = true;
	}
}
