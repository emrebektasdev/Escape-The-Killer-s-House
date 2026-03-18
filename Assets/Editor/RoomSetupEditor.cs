#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PuzzleGame.RoomSystem;
using PuzzleGame.UI;
using PuzzleGame.Core;

namespace PuzzleGame.EditorTools
{
    public class RoomSetupEditor : EditorWindow
    {
        [MenuItem("PuzzleGame/Setup Room UI")]
        public static void CreateRoomUI()
        {
            // 1. Canvas
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0.5f;

                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // 2. EventSystem
            if (GameObject.FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
                System.Type inputModuleType = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
                if (inputModuleType != null) eventSystem.AddComponent(inputModuleType);
                else eventSystem.AddComponent<StandaloneInputModule>();
#else
                eventSystem.AddComponent<StandaloneInputModule>();
#endif
            }

            // 3. RoomManager
            GameObject roomManagerObj = GameObject.Find("RoomManager");
            if (roomManagerObj == null) roomManagerObj = new GameObject("RoomManager");
            RoomViewManager roomManager = roomManagerObj.GetComponent<RoomViewManager>();
            if (roomManager == null) roomManager = roomManagerObj.AddComponent<RoomViewManager>();

            // 4. Room Image
            Transform roomImageT = canvas.transform.Find("RoomImage");
            GameObject roomImageObj = roomImageT != null ? roomImageT.gameObject : new GameObject("RoomImage");
            roomImageObj.transform.SetParent(canvas.transform, false);
            roomImageObj.transform.SetAsFirstSibling(); // send to back
            Image roomImage = roomImageObj.GetComponent<Image>();
            if (roomImage == null) roomImage = roomImageObj.AddComponent<Image>();
            roomImage.color = Color.white;
            roomImage.preserveAspect = true;
            SetStretchRect(roomImageObj.GetComponent<RectTransform>());
            roomManager.roomImage = roomImage;

            // 5. Navigation Buttons
            Transform btnLeftT = canvas.transform.Find("BtnLeftArrow");
            GameObject btnLeftObj = btnLeftT != null ? btnLeftT.gameObject : CreateButton("BtnLeftArrow", canvas.transform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(100, 100), new Vector2(50, 0));
            
            Transform btnRightT = canvas.transform.Find("BtnRightArrow");
            GameObject btnRightObj = btnRightT != null ? btnRightT.gameObject : CreateButton("BtnRightArrow", canvas.transform, new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(100, 100), new Vector2(-50, 0));

            Button btnLeft = btnLeftObj.GetComponent<Button>();
            Button btnRight = btnRightObj.GetComponent<Button>();

            UnityEditor.Events.UnityEventTools.RemovePersistentListener(btnLeft.onClick, new UnityEngine.Events.UnityAction(roomManager.PrevView));
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btnLeft.onClick, new UnityEngine.Events.UnityAction(roomManager.PrevView));

            UnityEditor.Events.UnityEventTools.RemovePersistentListener(btnRight.onClick, new UnityEngine.Events.UnityAction(roomManager.NextView));
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btnRight.onClick, new UnityEngine.Events.UnityAction(roomManager.NextView));

            // 6. Hotspots
            Transform hotspotsParentT = canvas.transform.Find("Hotspots");
            GameObject hotspotsParent = hotspotsParentT != null ? hotspotsParentT.gameObject : new GameObject("Hotspots");
            hotspotsParent.transform.SetParent(canvas.transform, false);
            SetStretchRect(hotspotsParent.GetComponent<RectTransform>() ?? hotspotsParent.AddComponent<RectTransform>());

            Transform hpMain = hotspotsParent.transform.Find("Hotspots_Main");
            roomManager.hotspotsMain = hpMain != null ? hpMain.gameObject : CreateHotspotContainer("Hotspots_Main", hotspotsParent.transform);

            Transform hpRight = hotspotsParent.transform.Find("Hotspots_Right");
            roomManager.hotspotsRight = hpRight != null ? hpRight.gameObject : CreateHotspotContainer("Hotspots_Right", hotspotsParent.transform);

            Transform hpLeft = hotspotsParent.transform.Find("Hotspots_Left");
            roomManager.hotspotsLeft = hpLeft != null ? hpLeft.gameObject : CreateHotspotContainer("Hotspots_Left", hotspotsParent.transform);

            // 6.5 Core Managers
            GameObject managersObj = GameObject.Find("GameManagers");
            if (managersObj == null) {
                managersObj = new GameObject("GameManagers");
                managersObj.AddComponent<Inventory>();
            } else if (managersObj.GetComponent<Inventory>() == null) {
                managersObj.AddComponent<Inventory>();
            }

            // safely remove old UI to prevent duplicates
            Transform oldInv = canvas.transform.Find("InventoryUI");
            if (oldInv != null) DestroyImmediate(oldInv.gameObject);
            Transform oldPickup = canvas.transform.Find("PickupAnimatorOverlay");
            if (oldPickup != null) DestroyImmediate(oldPickup.gameObject);
            Transform oldZoom = canvas.transform.Find("ZoomPanel");
            if (oldZoom != null) DestroyImmediate(oldZoom.gameObject);

            // 6.6 Inventory UI
            GameObject inventoryUIObj = new GameObject("InventoryUI");
            inventoryUIObj.transform.SetParent(canvas.transform, false);
            SetStretchRect(inventoryUIObj.AddComponent<RectTransform>());

            GameObject btnInvObj = CreateButton("BtnInventory", inventoryUIObj.transform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(80, 80), new Vector2(-60, -60));
            Button btnInv = btnInvObj.GetComponent<Button>();

            GameObject invContainerObj = new GameObject("InventoryContainer");
            invContainerObj.transform.SetParent(inventoryUIObj.transform, false);
            SetStretchRect(invContainerObj.AddComponent<RectTransform>());

            GameObject blurBgObj = new GameObject("BlurBackground");
            blurBgObj.transform.SetParent(invContainerObj.transform, false);
            SetStretchRect(blurBgObj.AddComponent<RectTransform>());
            Image blurBgImg = blurBgObj.AddComponent<Image>();
            blurBgImg.color = new Color(1, 1, 1, 1f); 
            
            if (!System.IO.Directory.Exists("Assets/Materials")) {
                System.IO.Directory.CreateDirectory("Assets/Materials");
                UnityEditor.AssetDatabase.Refresh();
            }
            Material blurMat = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/InventoryBlurMat.mat");
            if (blurMat == null) {
                Shader blurShader = Shader.Find("Custom/UIBlur");
                if (blurShader != null) {
                    blurMat = new Material(blurShader);
                    blurMat.SetFloat("_Radius", 3f);
                    blurMat.SetColor("_Tint", new Color(0.1f, 0.1f, 0.1f, 0.8f));
                    UnityEditor.AssetDatabase.CreateAsset(blurMat, "Assets/Materials/InventoryBlurMat.mat");
                }
            }
            if (blurMat != null) blurBgImg.material = blurMat;

            GameObject invPanelObj = new GameObject("InventoryPanel");
            invPanelObj.transform.SetParent(invContainerObj.transform, false);
            RectTransform invPanelRect = invPanelObj.AddComponent<RectTransform>();
            invPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
            invPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
            invPanelRect.sizeDelta = new Vector2(360, 245);
            invPanelRect.anchoredPosition = Vector2.zero;
            Image invPanelImg = invPanelObj.AddComponent<Image>();
            invPanelImg.color = new Color(0.2f, 0.2f, 0.2f, 0.6f);

            GridLayoutGroup grid = invPanelObj.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(100, 100);
            grid.spacing = new Vector2(15, 15);
            grid.padding = new RectOffset(15, 15, 15, 15);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 3;
            grid.childAlignment = TextAnchor.MiddleCenter;

            InventoryUIManager invUIManager = inventoryUIObj.AddComponent<InventoryUIManager>();
            invUIManager.inventoryPanel = invContainerObj;
            invUIManager.slotImages = new Image[6];

            for (int i = 0; i < 6; i++) {
                GameObject slotObj = new GameObject($"Slot_{i}");
                slotObj.transform.SetParent(invPanelObj.transform, false);
                
                Image slotBg = slotObj.AddComponent<Image>();
                slotBg.color = new Color(0.1f, 0.1f, 0.1f, 1f);

                GameObject itemImgObj = new GameObject("ItemIcon");
                itemImgObj.transform.SetParent(slotObj.transform, false);
                SetStretchRect(itemImgObj.AddComponent<RectTransform>());
                Image itemImg = itemImgObj.AddComponent<Image>();
                itemImg.preserveAspect = true;
                itemImg.color = new Color(1, 1, 1, 0);

                Button slotBtn = slotObj.AddComponent<Button>();
                invUIManager.slotImages[i] = itemImg;
            }

            UnityEngine.Events.UnityAction toggleAction = new UnityEngine.Events.UnityAction(invUIManager.ToggleInventory);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btnInv.onClick, toggleAction);
            invContainerObj.SetActive(false);

            // 6.7 Pickup Animator
            GameObject pickupObj = new GameObject("PickupAnimatorOverlay");
            pickupObj.transform.SetParent(canvas.transform, false);
            SetStretchRect(pickupObj.AddComponent<RectTransform>());

            GameObject pickupPanel = new GameObject("PickupPanel");
            pickupPanel.transform.SetParent(pickupObj.transform, false);
            Image pickupPanelImg = pickupPanel.AddComponent<Image>();
            pickupPanelImg.color = new Color(0, 0, 0, 0.8f);
            SetStretchRect(pickupPanel.GetComponent<RectTransform>());

            GameObject pickupIconObj = new GameObject("PickupIcon");
            pickupIconObj.transform.SetParent(pickupPanel.transform, false);
            RectTransform pickupIconRect = pickupIconObj.AddComponent<RectTransform>();
            pickupIconRect.anchorMin = new Vector2(0.5f, 0.5f);
            pickupIconRect.anchorMax = new Vector2(0.5f, 0.5f);
            pickupIconRect.sizeDelta = new Vector2(500, 500);
            pickupIconRect.anchoredPosition = Vector2.zero;
            Image pickupIconImg = pickupIconObj.AddComponent<Image>();
            pickupIconImg.preserveAspect = true;

            ItemPickupAnimator animator = pickupObj.AddComponent<ItemPickupAnimator>();
            animator.pickupPanel = pickupPanel;
            animator.pickupImage = pickupIconImg;

            // 7. Zoom Panel
            GameObject zoomPanelObj = new GameObject("ZoomPanel");
            zoomPanelObj.transform.SetParent(canvas.transform, false);
            Image zoomPanelImg = zoomPanelObj.AddComponent<Image>();
            zoomPanelImg.color = new Color(0, 0, 0, 1f);
            SetStretchRect(zoomPanelObj.GetComponent<RectTransform>());

            ZoomUIManager zoomUIManager = zoomPanelObj.AddComponent<ZoomUIManager>();
            zoomUIManager.zoomPanel = zoomPanelObj;

            GameObject zoomImageObj = new GameObject("ZoomImage");
            zoomImageObj.transform.SetParent(zoomPanelObj.transform, false);
            Image zoomedImg = zoomImageObj.AddComponent<Image>();
            zoomedImg.preserveAspect = true;
            SetStretchRect(zoomImageObj.GetComponent<RectTransform>());
            zoomUIManager.zoomImage = zoomedImg;

            GameObject btnBackObj = CreateButton("BtnBack", zoomPanelObj.transform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(100, 50), new Vector2(-60, -30));
            Button btnBack = btnBackObj.GetComponent<Button>();
            
            UnityEngine.Events.UnityAction backAction = new UnityEngine.Events.UnityAction(zoomUIManager.CloseZoom);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btnBack.onClick, backAction);

            zoomPanelObj.SetActive(false);

            Debug.Log("Puzzle Game Room UI successfully generated/updated without destroying existing Hotspots!");
            Selection.activeGameObject = roomManagerObj;
        }

        private static GameObject CreateButton(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);
            RectTransform rect = btnObj.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;

            Image img = btnObj.AddComponent<Image>();
            img.color = Color.white;
            btnObj.AddComponent<Button>();
            return btnObj;
        }

        private static GameObject CreateHotspotContainer(string name, Transform parent)
        {
            GameObject container = new GameObject(name);
            container.transform.SetParent(parent, false);
            SetStretchRect(container.AddComponent<RectTransform>());
            return container;
        }

        private static void SetStretchRect(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}
#endif
