using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCDialogSystem : MonoBehaviour
{
    [Header("Conversation Related Panel")]
    [SerializeField] GameObject wholePanel; //whole panel
    [SerializeField] private TextMeshProUGUI textContent; //the TMP need to be generated
    [SerializeField] string[] lines; //all needed conversation
    [SerializeField] float textSpeed;
    [SerializeField] GameObject interactText;
	
	private bool canOpen;
    private int index;

    private void Start()
    {
        index = 0;
        //clear all the text indide the panel
        textContent.text = string.Empty;
        wholePanel.SetActive(false);
    }

    private void Update()
    {
        //active chat panel when press F and close enough
        if (Input.GetKeyDown(KeyCode.E) && canOpen)
        {
            Debug.Log("close");
            //active the chat panel
            wholePanel.SetActive(true);
            if (textContent.text == lines[index])
            {
                Debug.Log("reading");
                NextLine();
            }
            else
            {
                Debug.Log("end");
                StopAllCoroutines();
                textContent.text = lines[index];
            }
        }
        else if(!canOpen) 
        {
            wholePanel.SetActive(false);
            textContent.text = string.Empty;
            index = 0;
        }
    }

    private IEnumerator TypeLine()
    {
        //evey single word in the array[i] will show one by one
        foreach (char c in lines[index].ToCharArray())
        {
            //show the single word one by one
            textContent.text += c;
            //the speed when the word to show
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        //if index less than the length will keep running
        if (index < lines.Length - 1)
        {
            index++;
            //this line dun know
            textContent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            Debug.Log("keep moving");
            //hide the chat panel if all the chat element was shown
            wholePanel.SetActive(false);
        }
    }
	
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.CompareTag("Player"))
		{
			canOpen = true;
			interactText.SetActive(true);
		}
	}
	
	private void OnTriggerExit2D(Collider2D collider)
	{
		if(collider.CompareTag("Player"))
		{
			canOpen = false;
			interactText.SetActive(false);
		}
	}
	
	

}
