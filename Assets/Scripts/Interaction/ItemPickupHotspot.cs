using UnityEngine;
using PuzzleGame.Core;
using PuzzleGame.UI;

namespace PuzzleGame.Interaction
{
    public class ItemPickupHotspot : Hotspot
    {
        [Header("Pickup Settings")]
        [Tooltip("The ID of the item to add to inventory when clicked.")]
        public string itemIdToGive;
        
        [Tooltip("If true, the hotspot will be deactivated after picking up the item.")]
        public bool deactivateOnPickup = true;

        public override void OnClick()
        {
            // If it's locked, behave as defined in base Hotspot
            if (showLockedMessage)
            {
                base.OnClick();
                return;
            }

            // Ensure we have an inventory system
            if (Inventory.Instance != null)
            {
                // Check if we already have it
                if (!Inventory.Instance.HasItem(itemIdToGive))
                {
                    // Assume the zoomSprite is the icon intended for the item
                    Sprite iconToUse = zoomSprite; 
                    
                    if (ItemPickupAnimator.Instance != null && iconToUse != null)
                    {
                        // The animator will handle adding to the inventory and the visual display
                        ItemPickupAnimator.Instance.ShowPickup(itemIdToGive, iconToUse);
                    }
                    else
                    {
                        // Fallback if no animator or sprite is assigned
                        Inventory.Instance.AddItem(itemIdToGive, iconToUse);
                        Debug.Log($"Picked up item invisibly: {itemIdToGive}");
                    }

                    if (deactivateOnPickup)
                    {
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log($"Item {itemIdToGive} is already in inventory.");
                }
            }
            else
            {
                Debug.LogError("Inventory instance not found!");
            }
        }
    }
}
