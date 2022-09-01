using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItems : MonoBehaviour
{
    public List<InteractableObject> usableItemList;

    public Dictionary<string, string> examineDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> takeDictionary = new Dictionary<string, string>();

    [HideInInspector] public List<string> nounsInRoom = new List<string>();

    private Dictionary<string, ActionResponse> useDictionary = new Dictionary<string, ActionResponse>();
    public List<string> nounsInInventory = new List<string>();
    private GameController controller;

    private void Awake()
    {
        controller = GetComponent<GameController>();
    }

    public string GetObjectsNotInInventory(Room currentRoom, int i)
    {
        InteractableObject interactableInRoom = currentRoom.interactableObjectsInRoom[i];

        if (!nounsInInventory.Contains(interactableInRoom.noun))
        {
            nounsInRoom.Add(interactableInRoom.noun);
            return interactableInRoom.description;
        }

        return null;
    }

    public void AddActionResponsesToUseDictionary()
    {
        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            string noun = nounsInInventory[i];

            InteractableObject interactableObjectInInventory = GetInteractableObjectFromUsableList(noun);
            if (interactableObjectInInventory == null)
                continue;

            for (int j = 0; j < interactableObjectInInventory.interactions.Length; j++)
            {
                Interaction interaction = interactableObjectInInventory.interactions[j];

                if (interaction.actionResponse == null)
                    continue;

                if (!useDictionary.ContainsKey(noun))
                {
                    useDictionary.Add(noun, interaction.actionResponse);
                }
            }
        }
    }

    private InteractableObject GetInteractableObjectFromUsableList(string noun)
    {
        for (int i = 0; i < usableItemList.Count; i++)
        {
            if (usableItemList[i].noun == (noun))
            {
                return usableItemList[i];
            }
        }
        return null;
    }

    public void DisplayInventory()
    {
        controller.LogStringWithReturn("You look in your backpack and pockets, inside you have: ");

        for (int i = 0; i < nounsInInventory.Count; i++)
        {
            controller.LogStringWithReturn("- " + nounsInInventory[i]);
        }
    }

    public void ClearCollections()
    {
        examineDictionary.Clear();
        takeDictionary.Clear();
        nounsInRoom.Clear();
    }

    public Dictionary<string, string> Take(string[] separatedInputWords) //HAVE TO DELETE NOUNS IN ROOM, OTHERWISE PLAYER WILL KEEP TAKING THE SAME ITEM OVER AND OVER AGAIN // "TAKE"(ONLY) BUGS
    {
        if (separatedInputWords.Length > 1)
        {
            string noun = separatedInputWords[1];

            if (nounsInRoom.Contains(noun) && !nounsInInventory.Contains(noun))
            {
                nounsInInventory.Add(noun);
                //nounsInRoom.Remove(noun);
                AddActionResponsesToUseDictionary();
                return takeDictionary;
            }
            else
            {
                controller.LogStringWithReturn("There is no " + noun + " here to take.");
                return null;
            }
        }
        else
        {
            controller.LogStringWithReturn("take what?");
            return null;
        }
    }

    public void UseItem(string[] separatedInputWords)
    {
        if(separatedInputWords.Length > 1)
        {
            string nounToUse = separatedInputWords[1];

            if (nounsInInventory.Contains(nounToUse))
            {
                if (useDictionary.ContainsKey(nounToUse))
                {
                    bool actionResult = useDictionary[nounToUse].DoActionResponse(controller);
                    if (!actionResult)
                    {
                        controller.LogStringWithReturn("Hmm, nothing happens.");
                    }
                }
                else
                {
                    controller.LogStringWithReturn("You can't use the " + nounToUse);
                }
            }
            else
            {
                controller.LogStringWithReturn("There is no " + nounToUse + " in your inventory to use");
            }
        }
        else
        {
            controller.LogStringWithReturn("use what?");
        }
    }
}
