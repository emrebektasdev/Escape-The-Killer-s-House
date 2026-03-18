using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame.Core
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        public struct InventoryItem
        {
            public string id;
            public Sprite icon;
        }

        private List<InventoryItem> items = new List<InventoryItem>();
        private const int MAX_SLOTS = 6;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // Optional: DontDestroyOnLoad(gameObject); if you want inventory to persist across scenes
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool AddItem(string itemId, Sprite icon)
        {
            if (items.Count >= MAX_SLOTS)
            {
                Debug.LogWarning("Inventory is full!");
                return false;
            }

            if (!HasItem(itemId))
            {
                items.Add(new InventoryItem { id = itemId, icon = icon });
                Debug.Log($"Item added to inventory: {itemId}");
                
                if (PuzzleGame.UI.InventoryUIManager.Instance != null && PuzzleGame.UI.InventoryUIManager.Instance.inventoryPanel.activeSelf)
                {
                    PuzzleGame.UI.InventoryUIManager.Instance.RefreshUI();
                }
                
                return true;
            }
            return false;
        }

        public bool HasItem(string itemId)
        {
            return items.Exists(item => item.id == itemId);
        }

        public void RemoveItem(string itemId)
        {
            int index = items.FindIndex(item => item.id == itemId);
            if (index != -1)
            {
                items.RemoveAt(index);
                Debug.Log($"Item removed from inventory: {itemId}");
                
                if (PuzzleGame.UI.InventoryUIManager.Instance != null && PuzzleGame.UI.InventoryUIManager.Instance.inventoryPanel.activeSelf)
                {
                    PuzzleGame.UI.InventoryUIManager.Instance.RefreshUI();
                }
            }
        }

        public List<InventoryItem> GetItems()
        {
            return items;
        }
    }
}
