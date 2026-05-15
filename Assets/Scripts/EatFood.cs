using UnityEngine;

public class EatFoodManager : MonoBehaviour
{
    [Header("Yemeklerin Verdiði Tokluk Deðerleri")]
    public float mantarTokluk = 10f;
    public float bugdayTokluk = 20f;
    public float koyunTokluk = 40f;

    // --- MANTAR BUTONU ÝÇÝN ---
    public void EatMantar()
    {
        // 1. Envanterde Mantar var mý kontrol et ve 1 tane harca
        if (GlobalInventory.Instance != null && GlobalInventory.Instance.SpendItem("Mantar", 1))
        {
            float miktar = mantarTokluk;

            // 2. Besleyici Öðün skilli açýksa %50 daha fazla doyursun
            if (HungerManager.Instance != null && HungerManager.Instance.level2_BesleyiciOgun)
            {
                miktar *= 1.5f;
            }

            // 3. Tokluðu artýr ve UI'ý güncelle
            SurvivalManager.Instance.AddTokluk(miktar);
            GlobalInventory.Instance.UpdateInventoryUI();

            Debug.Log("<color=green>Mantar yendi!</color> +" + miktar + " Tokluk eklendi.");
        }
        else
        {
            Debug.Log("<color=red>HATA:</color> Çantanda hiç Mantar yok!");
        }
    }

    // --- BUÐDAY BUTONU ÝÇÝN ---
    public void EatBugday()
    {
        if (GlobalInventory.Instance != null && GlobalInventory.Instance.SpendItem("Bugday", 1))
        {
            float miktar = bugdayTokluk;

            if (HungerManager.Instance != null && HungerManager.Instance.level2_BesleyiciOgun)
            {
                miktar *= 1.5f;
            }

            SurvivalManager.Instance.AddTokluk(miktar);
            GlobalInventory.Instance.UpdateInventoryUI();

            Debug.Log("<color=green>Buðday yendi!</color> +" + miktar + " Tokluk eklendi.");
        }
        else
        {
            Debug.Log("<color=red>HATA:</color> Çantanda hiç Buðday yok!");
        }
    }

    // --- KOYUN BUTONU ÝÇÝN ---
    public void EatKoyun()
    {
        if (GlobalInventory.Instance != null && GlobalInventory.Instance.SpendItem("Koyun", 1))
        {
            float miktar = koyunTokluk;

            if (HungerManager.Instance != null && HungerManager.Instance.level2_BesleyiciOgun)
            {
                miktar *= 1.5f;
            }

            SurvivalManager.Instance.AddTokluk(miktar);
            GlobalInventory.Instance.UpdateInventoryUI();

            Debug.Log("<color=green>Koyun yendi!</color> +" + miktar + " Tokluk eklendi.");
        }
        else
        {
            Debug.Log("<color=red>HATA:</color> Çantanda hiç Koyun yok!");
        }
    }
}