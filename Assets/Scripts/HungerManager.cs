using UnityEngine;
using UnityEngine.UI;

public class HungerManager : MonoBehaviour
{
    public static HungerManager Instance;

    [Header("Görsel Referanslar")]
    public GameObject mataraVisual;    // Beldeki su matarası
    public GameObject yemekKutusuVisual; // Yanındaki yemek çantası

    [Header("Aşçı Power-up Durumları")]
    public bool level1_ToklukHissi = false;
    public bool level2_BesleyiciOgun = false;

    [Header("Buton Referansları")]
    public Button toklukButonu;
    public Button besleyiciButonu;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (mataraVisual != null) mataraVisual.SetActive(false);
        if (yemekKutusuVisual != null) yemekKutusuVisual.SetActive(false);
    }

    public void Buy_ToklukHissi()
    {
        if (level1_ToklukHissi) return;
        if (GlobalInventory.Instance.SpendItem("Mantar", 10))
        {
            level1_ToklukHissi = true;

            // GÖRSELİ AKTİF ET
            if (mataraVisual != null) mataraVisual.SetActive(true);

            if (toklukButonu != null) toklukButonu.interactable = false;
        }
    }

    public void Buy_BesleyiciOgun()
    {
        if (level2_BesleyiciOgun) return;
        if (GlobalInventory.Instance.SpendItem("Bugday", 10))
        {
            level2_BesleyiciOgun = true;

            // GÖRSELİ AKTİF ET
            if (yemekKutusuVisual != null) yemekKutusuVisual.SetActive(true);

            if (besleyiciButonu != null) besleyiciButonu.interactable = false;
        }
    }
}