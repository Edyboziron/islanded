using UnityEngine;
using UnityEngine.UI;

public class KasifManager : MonoBehaviour
{
    public static KasifManager Instance;

    [Header("Görsel Referanslar")]
    public GameObject gazMaskesiVisual; // Karakterin yüzündeki maske
    public GameObject heybeVisual;      // Karakterin sırtındaki çanta

    [Header("Power-up Durumları (Kalıcı)")]
    public bool level1_DerinNefes = false;
    public bool level2_GenisHeybe = false;

    [Header("Skill Kartları (Nadir)")]
    public bool skill_ZamanBukucu = false;
    public bool skill_SonGaz = false;

    [Header("Buton Referansları")]
    public Button derinNefesButonu;
    public Button genisHeybeButonu;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Başlangıçta görselleri kapatalım
        if (gazMaskesiVisual != null) gazMaskesiVisual.SetActive(false);
        if (heybeVisual != null) heybeVisual.SetActive(false);
    }

    public void Buy_DerinNefes()
    {
        if (GlobalInventory.Instance.SpendItem("Sarmasik", 10))
        {
            level1_DerinNefes = true;
            SurvivalManager sm = FindFirstObjectByType<SurvivalManager>();
            if (sm != null) sm.maxTime += 15f;

            // GÖRSELİ AKTİF ET
            if (gazMaskesiVisual != null) gazMaskesiVisual.SetActive(true);

            if (derinNefesButonu != null) derinNefesButonu.interactable = false;
        }
    }

    public void Buy_GenisHeybe()
    {
        if (GlobalInventory.Instance.SpendItem("Sarmasik", 10))
        {
            level2_GenisHeybe = true;

            // GÖRSELİ AKTİF ET
            if (heybeVisual != null) heybeVisual.SetActive(true);

            if (genisHeybeButonu != null) genisHeybeButonu.interactable = false;
        }
    }
}