using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleGame.UI
{
    public class InventoryUIManager : MonoBehaviour
    {
        public static InventoryUIManager Instance { get; private set; }

        [Header("UI Panels")]
        public GameObject inventoryPanel;
        
        [Header("Inventory Slots")]
        [Tooltip("The 6 image slots in the inventory grid.")]
        public Image[] slotImages;

        private bool isOpen = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false);
            }
            ClearAllSlots();
        }

        public void ToggleInventory()
        {
            if (inventoryPanel == null) return;

            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);

            if (isOpen)
            {
                RefreshUI();
            }
        }

        public void RefreshUI()
        {
            if (Core.Inventory.Instance == null || slotImages == null || slotImages.Length == 0) return;

            List<Core.Inventory.InventoryItem> items = Core.Inventory.Instance.GetItems();

            for (int i = 0; i < slotImages.Length; i++)
            {
                if (i < items.Count)
                {
                    slotImages[i].sprite = items[i].icon;
                    slotImages[i].color = Color.white; // Make visible
                    slotImages[i].preserveAspect = true;
                    // Need to ensure raycast target is active if we add click-to-use later
                    slotImages[i].raycastTarget = true; 
                }
                else
                {
                    slotImages[i].sprite = null;
                    slotImages[i].color = new Color(1, 1, 1, 0); // Hide empty slots
                    slotImages[i].raycastTarget = false;
                }
            }
        }

        private void ClearAllSlots()
        {
            if (slotImages == null) return;
            foreach (var img in slotImages)
            {
                if (img != null)
                {
                    img.sprite = null;
                    img.color = new Color(1, 1, 1, 0);
                    img.raycastTarget = false;
                }
            }
        }
    }
}
