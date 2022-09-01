using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/InputActions/Go")]
public class Go : InputAction
{
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        controller.roomNavigation.AttemptToChangeRooms(separatedInputWords);
    }

    public override void RespondToGoInput(GameController controller, string[] separatedInputWords)
    {
        controller.roomNavigation.AttemptToChangeRooms(separatedInputWords);
    }
}

/*
    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 2)
        {
            controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[1] + " " + separatedInputWords[2]);
        }
        else if (separatedInputWords.Length > 1)
        {
            controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[1]);
        }
    }

    public override void RespondToGoInput(GameController controller, string[] separatedInputWords)
    {
        if (separatedInputWords.Length > 1)
        {
            controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[0] + " " + separatedInputWords[1]);
        }
        else if (separatedInputWords.Length > 0)
        {
            controller.roomNavigation.AttemptToChangeRooms(separatedInputWords[0]);
        }
        else
        {
            controller.roomNavigation.AttemptToChangeRooms(separatedInputWords);
        }
    } */
