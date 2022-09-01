using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text displayText;
    public InputAction[] inputActions;

    [SerializeField] private KeyResponses[] keyResponses;
    [SerializeField] private NonKeyResponses[] nonKeyResponses;
    public Dictionary<string, string[]> keyResponsesDictionary = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> nonKeyResponsesDictionary = new Dictionary<string, string[]>();

    [HideInInspector] public List<string> interactionDescriptionsInRoom = new List<string>();
    [HideInInspector] public RoomNavigation roomNavigation;
    [HideInInspector] public InteractableItems interactableItems;
    
    List<string> actionLog = new List<string>();

    private void Awake()
    {
        interactableItems = GetComponent<InteractableItems>();
        roomNavigation = GetComponent<RoomNavigation>();
    }

    private void Start()
    {
        keyResponsesDictionary.Clear();
        nonKeyResponsesDictionary.Clear();
        PopulateDictionaries();

        DisplayRoomText();
        DisplayLoggedText();
    }

    public void DisplayLoggedText()
    {
        string logAsText = string.Join("\n", actionLog.ToArray());
        
        displayText.text = logAsText;
    }

    public void DisplayRoomText()
    {
        ClearCollectionsForNewRoom();

        UnpackRoom();

        string joinedInteractionDescriptions = string.Join("\n", interactionDescriptionsInRoom.ToArray());

        string combinedText = roomNavigation.currentRoom.description + "\n" + joinedInteractionDescriptions;

        LogStringWithReturn(combinedText);
    }

    /* Plays a sound when entering the room.
     * Some room sounds should only play once, need another function.
    Player could check sounds through doors and try to listen to what's in the next room.
    public void PlayRoomSound()
    {

    } */

    private void UnpackRoom()
    {
        roomNavigation.UnpackExitsInRoom();
        PrepareObjectsToTakeOrExamine(roomNavigation.currentRoom);
    }

    private void PrepareObjectsToTakeOrExamine(Room currentRoom)
    {
        for (int i = 0; i < currentRoom.interactableObjectsInRoom.Length; i++)
        {
            string descriptionNotInInventory = interactableItems.GetObjectsNotInInventory(currentRoom, i);
            if (descriptionNotInInventory != null)
            {
                interactionDescriptionsInRoom.Add(descriptionNotInInventory);
            }

            InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

            for (int j = 0; j < interactableInRoom.interactions.Length; j++)
            {
                Interaction interaction = interactableInRoom.interactions[j];
                if (interaction.inputAction.keyWord == "examine")
                {
                    interactableItems.examineDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }

                if (interaction.inputAction.keyWord == "take")
                {
                    interactableItems.takeDictionary.Add(interactableInRoom.noun, interaction.textResponse);
                }
            }
        }
    }

    public string TestVerbDictionaryWithNoun(Dictionary<string, string>verbDictionary, string[] separatedInputWords)
    {
        string verb = separatedInputWords[0];
        if (separatedInputWords.Length > 1)
        {
            string noun = separatedInputWords[1];

            if (verbDictionary.ContainsKey(noun))
            {
                return verbDictionary[noun];
            }

            return "You can't " + verb + " " + noun;
        }
        else
        {
            return verb + " what?";
        }
    }

    private void PopulateDictionaries()
    {
        for (int i = 0; i < keyResponses.Length; i++)
        {
            keyResponsesDictionary.Add(keyResponses[i].keyWord, keyResponses[i].keyResponses);
        }

        for (int i = 0; i < nonKeyResponses.Length; i++)
        {
            nonKeyResponsesDictionary.Add(nonKeyResponses[i].keyWord, nonKeyResponses[i].nonKeyResponses);
        }
    }



    private void ClearCollectionsForNewRoom()
    {
        interactableItems.ClearCollections();
        interactionDescriptionsInRoom.Clear();
        roomNavigation.ClearExits();
    }

    public void LogStringWithReturn(string stringToAdd)
    {
        actionLog.Add(stringToAdd + "\n");
    }
}
