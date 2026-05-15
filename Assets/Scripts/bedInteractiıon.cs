using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BedInteraction : MonoBehaviour
{
    [Header("Isinlanma Ayarlari")]
    public Transform islandSpawnPoint;
    public float interactionRange = 3f;

    [Header("Aclik Bedeli")]
    public float sleepHungerCost = 30f;

    [Header("Bozulma Sistemi")]
    public int daysToBreak = 3;
    private int sleepCount = 0;

    [Header("UI Referanslari")]
    public GameObject pressEPanel;

    private Transform playerTransform;
    private CharacterController _controller;
    private bool isPlayerInRange = false;

    void Start()
    {
        FindPlayer(); // Ýlk baþta bir aramayý dene
    }

    void Update()
    {
        // --- ÇÖZÜM BURASI ---
        // Eðer Player'ý hala bulamadýysa aramaya devam et, pes etme!
        if (playerTransform == null)
        {
            FindPlayer();
            if (playerTransform == null) return; // Hala yoksa bu frame'i atla
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionRange)
        {
            if (!isPlayerInRange) OnEnterRange();
            if (Keyboard.current.eKey.wasPressedThisFrame) SleepAndGoToIsland();
        }
        else if (isPlayerInRange) OnExitRange();
    }

    // Arama iþlemini tek bir yere topladýk
    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            _controller = player.GetComponent<CharacterController>();
        }
    }

    void SleepAndGoToIsland()
    {
        SurfaceScatterSpawner[] spawners = FindObjectsOfType<SurfaceScatterSpawner>();
        foreach (var spawner in spawners) { spawner.RegenerateIsland(); }

        if (GlobalInventory.Instance != null)
        {
            GlobalInventory.Instance.ResetTripCount();
        }

        if (SurvivalManager.Instance != null)
        {
            float finalCost = sleepHungerCost;

            if (HungerManager.Instance != null && HungerManager.Instance.level1_ToklukHissi)
            {
                finalCost /= 2f;
            }

            SurvivalManager.Instance.AddTokluk(-finalCost);
            SurvivalManager.Instance.SyncHealthWithTokluk();
            SurvivalManager.Instance.StartIslandSession();
        }

        sleepCount++;

        if (sleepCount >= daysToBreak)
        {
            BreakRandomRepairedObject();
            sleepCount = 0;
        }

        if (islandSpawnPoint != null && _controller != null)
        {
            _controller.enabled = false;
            playerTransform.position = islandSpawnPoint.position;
            playerTransform.rotation = islandSpawnPoint.rotation;
            _controller.enabled = true;
        }

        OnExitRange();
    }

    void BreakRandomRepairedObject()
    {
        RepairStation[] allStations = FindObjectsOfType<RepairStation>(true);
        List<RepairStation> repairedStations = new List<RepairStation>();

        foreach (var station in allStations)
        {
            if (station.IsRepaired())
            {
                repairedStations.Add(station);
            }
        }

        if (repairedStations.Count > 0)
        {
            int randomIndex = Random.Range(0, repairedStations.Count);
            repairedStations[randomIndex].BreakStation();
        }
    }

    void OnEnterRange()
    {
        isPlayerInRange = true;
        if (pressEPanel != null) pressEPanel.SetActive(true);
    }

    void OnExitRange()
    {
        isPlayerInRange = false;
        if (pressEPanel != null) pressEPanel.SetActive(false);
    }
}