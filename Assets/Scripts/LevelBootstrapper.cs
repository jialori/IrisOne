using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBootstrapper : MonoBehaviour
{
    public DialogueText dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText.PlayDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
