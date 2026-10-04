using UnityEngine;
using TMPro;

public class ShipManager : MonoBehaviour
{
    public static ShipManager Instance;

    [Header("Gemi İstatistikleri")]
    public int currentLeaks = 0;
    public int maxLeaks = 3; // Başlangıçta 3, zırh alınınca 4 olacak
    public int leakFrequency = 3;
    private int daysPassed = 0;

    [Header("UI")]
    public TextMeshProUGUI shipStatusText;

    void Awake()
    {
        // Singleton yapısı: Eğer sahnede başka bir ShipManager varsa yenisini siler.
        if (Instance == null) Instance = this;
        else if (Instance != this) { Destroy(gameObject); return; }
    }

    void OnDestroy()
    {
        // Obje silinirse referansı temizle ki MissingReference hatası vermesin.
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

        // --- ÖLÜM (BATIŞ) KONTROLÜ BURADA ---
        if (currentLeaks >= maxLeaks)
        {
            Debug.LogError("Gemi Battı! OYUN BİTTİ.");

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