using UnityEngine;
using TMPro;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    [Header("Gemi Ýstatistikleri")]
    public int currentLeaks = 0;
    public int maxLeaks = 3; // Baþlangýçta 3, zýrh alýnýnca 4 olacak
    public int leakFrequency = 3;
    private int daysPassed = 0;

    [Header("UI")]
    public TextMeshProUGUI shipStatusText;

    void Awake()
    {
        // Singleton yapýsý: Eðer sahnede baþka bir ShipManager varsa yenisini siler.
        if (Instance == null) Instance = this;
        else if (Instance != this) { Destroy(gameObject); return; }
    }

    void OnDestroy()
    {
        // Obje silinirse referansý temizle ki MissingReference hatasý vermesin.
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        UpdateShipUI();
    }

    public void NextDay()
    {
        daysPassed++;
        if (daysPassed % leakFrequency == 0)
        {
            CreateLeak();
        }
        UpdateShipUI();
    }

    public void CreateLeak()
    {
        currentLeaks++;

        // --- ÖLÜM (BATIÞ) KONTROLÜ BURADA ---
        if (currentLeaks >= maxLeaks)
        {
            Debug.LogError("Gemi Battý! OYUN BÝTTÝ.");

            // SurvivalManager'daki ölüm panelini tetikle
            if (SurvivalManager.Instance != null)
            {
                SurvivalManager.Instance.TriggerGameOver();
            }
        }

        UpdateShipUI();
    }

    public void RepairLeak()
    {
        if (currentLeaks > 0)
        {
            currentLeaks--;
            UpdateShipUI();
        }
    }

    public void UpdateShipUI()
    {
        if (this == null || shipStatusText == null) return;
        shipStatusText.text = "Gemi Delik: " + currentLeaks + " / " + maxLeaks;
    }
}