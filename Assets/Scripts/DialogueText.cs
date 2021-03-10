using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq; // Count
using System; // Char.IsWhiteSpace


public class DialogueText : MonoBehaviour
{
    public TMP_Text textBox;
    public TextAsset dialogueTextAsset;

    private bool m_willExitDialogue;
    private Func<bool> GetScreenTap = () => Input.GetButtonDown("Submit");


    public void PlayDialogue()
    {
        if (dialogueTextAsset && textBox)
        {
            StartCoroutine("PlayDialogueCoroutine");
        }
    }

    public void ExitDialogue()
    {
        m_willExitDialogue = true;
    }

    private IEnumerator PlayDialogueCoroutine()
    {
        gameObject.SetActive(true);
        
        GameState.gameControlState = GameControlState.InteractWithUI;
        string text = dialogueTextAsset.text;
        string[] lines = text.Split('\n');
        ShowDialogueLine(lines[0]);
        int i = SkipToNextValidLine(ref lines, 1);
        while (i < lines.Length)
        {
            if (GetScreenTap())
            {
                ShowDialogueLine(lines[i]);
                i++;
                i = SkipToNextValidLine(ref lines, i);
            }

            yield return null;
        }

        while (!GetScreenTap())
        {
            yield return null;
        }
        gameObject.SetActive(false);

        GameState.gameControlState = GameControlState.InteractWithGame;
    }


    private void ShowDialogueLine(string line)
    {
        textBox.text = line;
    }

    private int SkipToNextValidLine(ref string[] lines, int startCheckIndex)
    {
        int i = startCheckIndex;
        while (i < lines.Length)
        {
            string line = lines[i];
            if (line.Count(ch => Char.IsWhiteSpace(ch)) < line.Length && line.Length > 0)
            { // is a proper line
                return i;
            }
            i++;
        }
        return i;
    }

}
