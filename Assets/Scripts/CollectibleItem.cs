using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class CollectibleItem : MonoBehaviour
{
    [Header("Eşya Ayarları")]
    public string itemType = "Mantar";
    public int amount = 1;
    public float interactionRange = 3f;
    public Color highlightColor = Color.yellow;

    [Header("Ses Ayarları")]
    public AudioClip pickupSound; // Buraya ses dosyasını sürükleyip bırakacaksın

    private static GameObject pressEPanel;
    private Color originalColor;
    private Renderer objRenderer;
    private Transform playerTransform;
    private bool isPlayerInRange = false;

    private static List<CollectibleItem> itemsInRange = new List<CollectibleItem>();
    private static int lastProcessedFrame = -1;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        if (objRenderer != null) originalColor = objRenderer.material.color;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        if (pressEPanel == null) pressEPanel = GameObject.Find("PressEPanel");
    }

    void Update()
    {
        if (playerTransform == null) return;
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionRange)
        {
            if (!isPlayerInRange) OnEnterRange();

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && Time.frameCount != lastProcessedFrame)
            {
                CollectibleItem closest = GetClosestItem();
                if (closest == this)
                {
                    lastProcessedFrame = Time.frameCount;
                    CollectItem();
                }
            }
        }
        else if (isPlayerInRange) OnExitRange();
    }

    void CollectItem()
    {
        if (GlobalInventory.Instance != null)
        {
            int currentTripLoad = GlobalInventory.Instance.currentTripCount;
            int maxCapacity = (KasifManager.Instance != null && KasifManager.Instance.level2_GenisHeybe) ? 8 : 5;

            if (currentTripLoad + amount > maxCapacity)
            {
                Debug.LogWarning("Canta dolu! Bu seferde daha fazla esya tasiyamazsin.");
                return;
            }

            // --- SES ÇALMA (YENİ) ---
            if (pickupSound != null)
            {
                // Sesi oyuncunun olduğu pozisyonda çalar
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            switch (itemType)
            {
                case "Mantar": GlobalInventory.Instance.mantar += amount; break;
                case "Bugday": GlobalInventory.Instance.bugday += amount; break;
                case "Koyun": GlobalInventory.Instance.koyun += amount; break;
                case "Tahta": GlobalInventory.Instance.tahta += amount; break;
                case "Civi": GlobalInventory.Instance.civi += amount; break;
                case "Sarmasik": GlobalInventory.Instance.sarmasik += amount; break;
                default: Debug.LogWarning("Bilinmeyen esya tipi: " + itemType); break;
            }

            GlobalInventory.Instance.currentTripCount += amount;
            GlobalInventory.Instance.UpdateInventoryUI();
        }

        CleanupAndDestroy();
    }

    // ... (Geri kalan Cleanup, OnDestroy, GetClosestItem vb. aynı kalıyor)
    private void CleanupAndDestroy()
    {
        if (itemsInRange.Contains(this)) itemsInRange.Remove(this);
        if (pressEPanel != null && itemsInRange.Count == 0) pressEPanel.SetActive(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (itemsInRange != null && itemsInRange.Contains(this))
        {
            itemsInRange.Remove(this);
        }
    }

    private CollectibleItem GetClosestItem()
    {
        CollectibleItem closest = null;
        float minDistance = float.MaxValue;

        for (int i = itemsInRange.Count - 1; i >= 0; i--)
        {
            if (itemsInRange[i] == null) { itemsInRange.RemoveAt(i); continue; }

            float dist = Vector3.Distance(itemsInRange[i].transform.position, playerTransform.position);
            if (dist < minDistance) { minDistance = dist; closest = itemsInRange[i]; }
        }
        return closest;
    }

    void OnEnterRange()
    {
        isPlayerInRange = true;
        if (!itemsInRange.Contains(this)) itemsInRange.Add(this);
        if (objRenderer != null) objRenderer.material.color = highlightColor;
        if (pressEPanel != null) pressEPanel.SetActive(true);
    }

    void OnExitRange()
    {
        isPlayerInRange = false;
        if (itemsInRange.Contains(this)) itemsInRange.Remove(this);
        if (objRenderer != null) objRenderer.material.color = originalColor;
        if (pressEPanel != null && itemsInRange.Count == 0) pressEPanel.SetActive(false);
    }
}