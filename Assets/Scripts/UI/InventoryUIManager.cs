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

        [Header("Global UI References")]
        [Tooltip("The left navigation arrow to hide when inventory is open.")]
        public GameObject leftArrowButton;
        [Tooltip("The right navigation arrow to hide when inventory is open.")]
        public GameObject rightArrowButton;
        [Tooltip("The back button to show when inventory is open (closes the inventory).")]
        public GameObject backButton;
        [Tooltip("The backpack button to ensure is shown.")]
        public GameObject backpackButton;

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
            
            // Back button should be off by default if inventory is off
            if (backButton != null)
            {
                backButton.SetActive(false);
            }
            
            // Backpack should be on by default
            if (backpackButton != null) backpackButton.SetActive(true);
            
            ClearAllSlots();
        }

        public void ToggleInventory()
        {
            if (inventoryPanel == null) return;

            isOpen = !isOpen;
            
            // Toggle panel
            inventoryPanel.SetActive(isOpen);
            if (isOpen) 
            {
                inventoryPanel.transform.SetAsLastSibling();
                transform.SetAsLastSibling(); // Bring the whole InventoryUI parent to front of Canvas
            }
            
            // Toggle navigation arrows (Hide if open, show if closed)
            if (leftArrowButton != null) leftArrowButton.SetActive(!isOpen);
            if (rightArrowButton != null) rightArrowButton.SetActive(!isOpen);
            
            // Toggle back button (Show if open, hide if closed)
            if (backButton != null) 
            {
                backButton.SetActive(isOpen);
                if (isOpen) backButton.transform.SetAsLastSibling();
            }
            
            // Ensure backpack button is always on as per request
            if (backpackButton != null) 
            {
                backpackButton.SetActive(true);
                if (isOpen) backpackButton.transform.SetAsLastSibling();
            }

            if (isOpen)
            {
                RefreshUI();
            }
        }

        public void CloseInventory()
        {
            if (isOpen)
            {
                ToggleInventory();
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
