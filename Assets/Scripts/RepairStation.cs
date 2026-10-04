using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class RepairStation : MonoBehaviour
{
    [Header("Tamir Ayarları")]
    public string itemName = "Tahta";
    public int requiredAmount = 2;
    private int currentAmount = 0;
    public float interactionRange = 3f;

    [Tooltip("Eğer bu eşyanın oyun başında BOZUK başlamasını istiyorsan bu tiki işaretle.")]
    public bool startBroken = false;

    [Header("Görsel ve UI")]
    public TextMeshProUGUI statusText;
    public GameObject pressEPanel;
    public GameObject repairedObject;
    public GameObject brokenObject;

    private Transform playerTransform;
    private bool isPlayerInRange = false;

    void Start()
    {
        FindPlayer(); // İlk aramayı yap

        if (!startBroken)
        {
            currentAmount = requiredAmount;
        }

        UpdateStatusUI();
        if (pressEPanel != null) pressEPanel.SetActive(false);

        if (currentAmount >= requiredAmount)
        {
            if (repairedObject != null) repairedObject.SetActive(true);
            if (brokenObject != null) brokenObject.SetActive(false);
        }
        else
        {
            if (repairedObject != null) repairedObject.SetActive(false);
            if (brokenObject != null) brokenObject.SetActive(true);
        }
    }

    void Update()
    {
        // --- ÇÖZÜM BURASI ---
        if (playerTransform == null)
        {
            FindPlayer();
            if (playerTransform == null) return;
        }

        if (currentAmount >= requiredAmount)
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                if (pressEPanel != null) pressEPanel.SetActive(false);
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionRange)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                if (pressEPanel != null) pressEPanel.SetActive(true);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryRepair();
            }
        }
        else if (isPlayerInRange)
        {
            isPlayerInRange = false;
            if (pressEPanel != null) pressEPanel.SetActive(false);
        }
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void TryRepair()
    {
        if (GlobalInventory.Instance != null && GlobalInventory.Instance.SpendItem(itemName, 1))
        {
            currentAmount++;
            UpdateStatusUI();

            if (currentAmount >= requiredAmount)
            {
                FinishRepair();
            }
        }
    }

    void UpdateStatusUI()
    {
        if (statusText != null)
        {
            statusText.text = itemName + ": " + currentAmount + " / " + requiredAmount;
        }
    }

    void FinishRepair()
    {
        if (repairedObject != null) repairedObject.SetActive(true);
        if (brokenObject != null) brokenObject.SetActive(false);
        if (pressEPanel != null) pressEPanel.SetActive(false);

        if (statusText != null)
        {
            statusText.text = "TAMİR EDİLDİ";
        }
    }

    public bool IsRepaired()
    {
        return currentAmount >= requiredAmount;
    }

    public void BreakStation()
    {
        currentAmount = 0;

        if (repairedObject != null) repairedObject.SetActive(false);
        if (brokenObject != null) brokenObject.SetActive(true);

        UpdateStatusUI();
    }
}