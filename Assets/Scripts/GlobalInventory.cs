using UnityEngine;
using TMPro;

public class GlobalInventory : MonoBehaviour
{
    public static GlobalInventory Instance;

    [Header("Gida Malzemeleri (Kalıcı Kasa)")]
    public int mantar = 0;
    public int bugday = 0;
    public int koyun = 0;

    [Header("Tamir Malzemeleri (Kalıcı Kasa)")]
    public int tahta = 0;
    public int civi = 0;
    public int sarmasik = 0;

    // --- YENİ: Sadece adadaki yükü takip eder ---
    [HideInInspector] public int currentTripCount = 0;

    [Header("UI Referanslari (Hiyerarsiden Surukle)")]
    public TextMeshProUGUI mantarText;
    public TextMeshProUGUI bugdayText;
    public TextMeshProUGUI koyunText;
    public TextMeshProUGUI tahtaText;
    public TextMeshProUGUI civiText;
    public TextMeshProUGUI sarmasikText;

    [Space(10)]
    [Header("Kapasite Ayari")]
    public TextMeshProUGUI kapasiteText;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        UpdateInventoryUI();
    }

    // --- GÜNCELLENDİ: Esyalar silinmez, sadece canta dolulugu (0/5) sıfırlanır ---
    public void ResetTripCount()
    {
        currentTripCount = 0;
        UpdateInventoryUI();
        Debug.Log("<color=green>Envanter:</color> Ada yuku bosaltildi. Esyalarin hala kasanda!");
    }

    // İhtiyacın olursa her şeyi tamamen silmek için bu kalsın
    public void ResetInventoryFull()
    {
        mantar = 0; bugday = 0; koyun = 0;
        tahta = 0; civi = 0; sarmasik = 0;
        currentTripCount = 0;
        UpdateInventoryUI();
    }

    public int GetTotalItemCount()
    {
        // Bu hala toplamı döndürür (Gerekirse kullanırsın)
        return mantar + bugday + koyun + tahta + civi + sarmasik;
    }

    public void UpdateInventoryUI()
    {
        // Malzeme sayılarını göster (Kasadaki miktar)
        if (mantarText != null) mantarText.text = "Mantar: " + mantar;
        if (bugdayText != null) bugdayText.text = "Bugday: " + bugday;
        if (koyunText != null) koyunText.text = "Koyun: " + koyun;
        if (tahtaText != null) tahtaText.text = "Tahta: " + tahta;
        if (civiText != null) civiText.text = "Civi: " + civi;
        if (sarmasikText != null) sarmasikText.text = "Sarmasik: " + sarmasik;

        // --- KAPASITE YAZISI GUNCELLEME ---
        if (kapasiteText != null)
        {
            int max = (KasifManager.Instance != null && KasifManager.Instance.level2_GenisHeybe) ? 8 : 5;

            // DİKKAT: Artik kapasite yazısı toplam eşyayı değil, "currentTripCount"u gösteriyor!
            kapasiteText.text = "Canta: " + currentTripCount + " / " + max;
            kapasiteText.color = (currentTripCount >= max) ? Color.red : Color.white;
        }
    }

    public bool SpendItem(string itemName, int amount)
    {
        switch (itemName)
        {
            case "Mantar": if (mantar >= amount) { mantar -= amount; return true; } break;
            case "Bugday": if (bugday >= amount) { bugday -= amount; return true; } break;
            case "Koyun": if (koyun >= amount) { koyun -= amount; return true; } break;
            case "Tahta": if (tahta >= amount) { tahta -= amount; return true; } break;
            case "Civi": if (civi >= amount) { civi -= amount; return true; } break;
            case "Sarmasik": if (sarmasik >= amount) { sarmasik -= amount; return true; } break;
        }
        return false;
    }
}