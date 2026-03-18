using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleGame.UI
{
    public class ItemPickupAnimator : MonoBehaviour
    {
        public static ItemPickupAnimator Instance { get; private set; }

        [Header("UI Elements")]
        public GameObject pickupPanel;
        public Image pickupImage;

        [Header("Animation Settings")]
        public float displayDuration = 1.5f;
        public float fadeDuration = 0.25f;

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
            if (pickupPanel != null)
            {
                pickupPanel.SetActive(false);
            }
        }

        public void ShowPickup(string itemId, Sprite icon)
        {
            if (pickupPanel == null || pickupImage == null) return;
            
            // Try to add item to inventory first, if full, don't play animation
            if (Core.Inventory.Instance.AddItem(itemId, icon))
            {
                StartCoroutine(PickupRoutine(icon));
            }
        }

        private IEnumerator PickupRoutine(Sprite icon)
        {
            pickupImage.sprite = icon;
            pickupImage.preserveAspect = true;
            pickupPanel.SetActive(true);
            pickupPanel.transform.SetAsLastSibling(); // Ensure it's on top

            // Optional: Add simple scale up or fade in here

            yield return new WaitForSeconds(displayDuration);

            // Hide after duration
            pickupPanel.SetActive(false);
            pickupImage.sprite = null;
        }
    }
}
