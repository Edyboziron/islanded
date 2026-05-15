using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    [Header("Hasar Ayarlari")]
    public float saniyeBasiHasar = 15f;

    private bool isPlayerInWater = false;
    private Transform playerTransform;

    // Oyuncu suya girdiðinde
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInWater = true;
            playerTransform = other.transform; // Oyuncunun yerini aklýmýzda tutuyoruz
            Debug.Log("<color=blue>Suya girildi!</color> Can azalýyor...");
        }
    }

    // Oyuncu sudan yürüyerek çýktýðýnda
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInWater = false;
            Debug.Log("<color=blue>Sudan çýkýldý!</color> Hasar durdu.");
        }
    }

    // Her saniye hasar kontrolü
    private void Update()
    {
        if (isPlayerInWater)
        {
            // --- IÞINLANMA BUG'I ÇÖZÜMÜ ---
            // Eðer oyuncu gemiye ýþýnlandýysa Unity OnTriggerExit'i unutur.
            // Biz manuel olarak oyuncunun gemiye gidip gitmediðini kontrol ediyoruz.
            if (playerTransform != null && SurvivalManager.Instance != null)
            {
                if (SurvivalManager.Instance.largeShipBoardingTarget != null)
                {
                    // Oyuncunun gemi ýþýnlanma noktasýna olan uzaklýðýna bakýyoruz
                    float distToShip = Vector3.Distance(playerTransform.position, SurvivalManager.Instance.largeShipBoardingTarget.position);

                    // Eðer oyuncu gemiye (ýþýnlanma noktasýna) çok yakýnsa, kesinlikle sudan çýkmýþtýr.
                    if (distToShip < 20f)
                    {
                        isPlayerInWater = false;
                        Debug.Log("<color=yellow>Iþýnlanma Tespit Edildi:</color> Su hasarý zorla durduruldu.");
                        return; // Hasar vermeden döngüden çýk
                    }
                }
            }

            // Eðer hala sudaysa hasar vermeye devam et
            if (SurvivalManager.Instance != null)
            {
                SurvivalManager.Instance.TakeDamage(saniyeBasiHasar * Time.deltaTime);
            }
        }
    }
}