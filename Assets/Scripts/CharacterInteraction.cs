using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using StarterAssets;

public class CharacterInteraction : MonoBehaviour
{
    [Header("Etkileşim Ayarları")]
    public float interactionRange = 3f;
    public GameObject characterUIPanel;

    [Header("Ortak UI")]
    private static GameObject pressEPanel;

    private Transform playerTransform;
    private StarterAssetsInputs _input;
    private bool isPlayerInRange = false;
    private bool isUIPanelOpen = false;

    private static List<CharacterInteraction> charactersInRange = new List<CharacterInteraction>();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            _input = player.GetComponent<StarterAssetsInputs>();
        }

        if (pressEPanel == null) pressEPanel = GameObject.Find("PressEPanel");
        if (characterUIPanel != null) characterUIPanel.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null || this == null) return;

        // --- HATA KORUMASI: Listeyi temizle ---
        charactersInRange.RemoveAll(c => c == null);

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionRange)
        {
            if (!isPlayerInRange) OnEnterRange();
            if (Keyboard.current.eKey.wasPressedThisFrame) ToggleCharacterUI();
        }
        else if (isPlayerInRange) OnExitRange();
    }

    public void ToggleCharacterUI()
    {
        if (characterUIPanel == null) return;

        isUIPanelOpen = !isUIPanelOpen;
        characterUIPanel.SetActive(isUIPanelOpen);
        if (pressEPanel != null) pressEPanel.SetActive(!isUIPanelOpen);
        SetCursorState(isUIPanelOpen);
    }

    void SetCursorState(bool uiAcikMi)
    {
        if (_input == null) return;
        Cursor.visible = uiAcikMi;
        Cursor.lockState = uiAcikMi ? CursorLockMode.None : CursorLockMode.Locked;
        _input.cursorLocked = !uiAcikMi;
        _input.cursorInputForLook = !uiAcikMi;
    }

    void OnEnterRange()
    {
        isPlayerInRange = true;
        if (!charactersInRange.Contains(this)) charactersInRange.Add(this);
        if (pressEPanel != null && !isUIPanelOpen) pressEPanel.SetActive(true);
    }

    void OnExitRange()
    {
        isPlayerInRange = false;
        if (characterUIPanel != null) characterUIPanel.SetActive(false);
        if (isUIPanelOpen) { isUIPanelOpen = false; SetCursorState(false); }
        charactersInRange.Remove(this);
        if (pressEPanel != null && charactersInRange.Count == 0) pressEPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        // Bu obje yok edilirse listeden sildiriyoruz
        if (charactersInRange != null && charactersInRange.Contains(this))
        {
            charactersInRange.Remove(this);
        }
    }
}