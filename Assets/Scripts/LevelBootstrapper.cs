using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBootstrapper : MonoBehaviour
{
    public DialogueText dialogueText;
    public GameObject tutorialDialogueInterface;
    public GameObject dayReminderInterface;

    [Header("Control Params")]
    public bool willPlayTutorial;
    public bool willShowDayReminder;

    void Start()
    {
        tutorialDialogueInterface.SetActive(false);
        dayReminderInterface.SetActive(false);
        StartCoroutine(LevelStartingSequence());
    }

    private IEnumerator LevelStartingSequence()
    {
        GameState.gameControlState = GameControlState.InteractWithUI;
        if (willShowDayReminder)
        {
            dayReminderInterface.SetActive(true);
            yield return new WaitForSeconds(3);
            dayReminderInterface.SetActive(false);
        }
        if (willPlayTutorial)
        {
            tutorialDialogueInterface.SetActive(true);
            yield return StartCoroutine(dialogueText.PlayDialogue());
        }
        GameState.gameControlState = GameControlState.InteractWithGame;
    } 
}
