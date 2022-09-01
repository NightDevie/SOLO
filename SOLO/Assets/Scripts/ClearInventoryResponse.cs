using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextAdventure/ActionResponses/ClearInventory")]
public class ClearInventoryResponse : ActionResponse
{
    public override bool DoActionResponse(GameController controller)
    {
        if (controller.roomNavigation.currentRoom.roomName == requiredString)
        {
            controller.interactableItems.nounsInInventory.Clear();
            return true;
        }
        return false;
    }
}
