using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CheckSaveFileExist : MonoBehaviour
{
    private Button ContinueBtn;

    // Start is called before the first frame update
    private void Start()
    {
        ContinueBtn = GetComponent<Button>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (File.Exists(Application.dataPath + "/saveFile.json")) 
        {
            ContinueBtn.interactable = true;
        }
        else
            ContinueBtn.interactable = false;
    }
}
