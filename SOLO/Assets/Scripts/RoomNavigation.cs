using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNavigation : MonoBehaviour
{
    public Room currentRoom;

    private Dictionary<string, Room> exitDictionary = new Dictionary<string, Room>();

    private GameController controller;
    
    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public void UnpackExitsInRoom()
    {
        for (int i = 0; i < currentRoom.exits.Length; i++)
        {
            int n = i;
            for (int a = 0; a < currentRoom.exits[n].keyString.Length; a++)
            {
                exitDictionary.Add(currentRoom.exits[n].keyString[a], currentRoom.exits[n].valueRoom);
            }

            controller.interactionDescriptionsInRoom.Add(currentRoom.exits[i].exitDescription);
        }
    }

    public void AttemptToChangeRooms(string[] separatedInputWords)
    {
        string directionNoun = "Invalid Direction";

        if (separatedInputWords[0] != "go")
        {
            directionNoun = String.Join(" ", separatedInputWords);

            ChangeRooms(directionNoun);
        }
        else
        {
            switch (separatedInputWords.Length)
            {
                case 1:
                    directionNoun = separatedInputWords[0];
                    controller.LogStringWithReturn(controller.nonKeyResponsesDictionary[directionNoun][0]);
                    break;

                case 2:
                    directionNoun = separatedInputWords[1];
                    ChangeRooms(directionNoun);
                    break;

                case 3:
                    directionNoun = separatedInputWords[1] + separatedInputWords[2];
                    ChangeRooms(directionNoun);
                    break;

                default:
                    controller.LogStringWithReturn("that direction doesn't exist");
                    break;
            }
        }
    }

    public void ChangeRooms(string directionNoun)
    {
        if (exitDictionary.ContainsKey(directionNoun))
        {
            currentRoom = exitDictionary[directionNoun];
            controller.LogStringWithReturn(controller.keyResponsesDictionary[directionNoun][0]);
            controller.DisplayRoomText();
            //controller.PlayRoomSound(); //Plays room sound.
        }
        else if(controller.nonKeyResponsesDictionary.ContainsKey(directionNoun))
        {
            controller.LogStringWithReturn(controller.nonKeyResponsesDictionary[directionNoun][0]);
        }
        else
        {
            controller.LogStringWithReturn("that direction doesn't exist");
        }
    }

    public void ClearExits()
    {
        exitDictionary.Clear();
    }
}
