using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInput : MonoBehaviour
{
    public TMP_InputField inputField;

    private GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
        inputField.onEndEdit.AddListener(AcceptStringInput);
    }

    private void AcceptStringInput(string userInput)
    {
        userInput = userInput.ToLower();
        controller.LogStringWithReturn(">" + userInput);

        char[] delimiterCharacters = { ' ' };
        string[] separatedInputWords = userInput.Split(delimiterCharacters);

        for (int i = 0; i < controller.inputActions.Length; i++)
        {
            InputAction inputAction = controller.inputActions[i];

            if (inputAction.keyWord == separatedInputWords[0])
            {
                inputAction.RespondToInput(controller, separatedInputWords);
            }
            else if (controller.keyResponsesDictionary.ContainsKey(separatedInputWords[0])
                || separatedInputWords.Length > 1 && controller.keyResponsesDictionary.ContainsKey(separatedInputWords[0] + separatedInputWords[1]))
            {
                inputAction.RespondToGoInput(controller, separatedInputWords);
            }
            else if (i == controller.inputActions.Length - 1)
            {
                bool flag = false;

                for (int j = 0; j < controller.inputActions.Length; j++)
                {
                    if (separatedInputWords[0] == controller.inputActions[j].keyWord)
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    //string joinedWords = String.Join(" ", separatedInputWords);
                    if (controller.nonKeyResponsesDictionary.ContainsKey(userInput))
                    {
                        controller.LogStringWithReturn(controller.nonKeyResponsesDictionary[userInput][0]);
                    }
                    else
                    {
                        controller.LogStringWithReturn("What do you mean, " + userInput + "?");
                    }
                }
            }
        }


        InputComplete();
    }

    private void InputComplete()
    {
        controller.DisplayLoggedText();
        inputField.ActivateInputField();
        inputField.text = null;
    }
}
