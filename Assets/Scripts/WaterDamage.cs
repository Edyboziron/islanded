using UnityEngine;

public class WaterDamage : MonoBehaviour
{
    [Header("Hasar Ayarlari")]
    public float saniyeBasiHasar = 15f;

    private bool isPlayerInWater = false;
    private Transform playerTransform;

    // Oyuncu suya girdiğinde
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInWater = true;
            playerTransform = other.transform; // Oyuncunun yerini aklımızda tutuyoruz
            Debug.Log("<color=blue>Suya girildi!</color> Can azalıyor...");
        }
    }

    // Oyuncu sudan yürüyerek çıktığında
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInWater = false;
            Debug.Log("<color=blue>Sudan çıkıldı!</color> Hasar durdu.");
        }
    }

    // Her saniye hasar kontrolü
    private void Update()
    {
        if (isPlayerInWater)
        {
            // --- IŞINLANMA BUG'I ÇÖZÜMÜ ---
            // Eğer oyuncu gemiye ışınlandıysa Unity OnTriggerExit'i unutur.
            // Biz manuel olarak oyuncunun gemiye gidip gitmediğini kontrol ediyoruz.
            if (playerTransform != null && SurvivalManager.Instance != null)
            {
                if (SurvivalManager.Instance.largeShipBoardingTarget != null)
                {
                    // Oyuncunun gemi ışınlanma noktasına olan uzaklığına bakıyoruz
                    float distToShip = Vector3.Distance(playerTransform.position, SurvivalManager.Instance.largeShipBoardingTarget.position);

                    // Eğer oyuncu gemiye (ışınlanma noktasına) çok yakınsa, kesinlikle sudan çıkmıştır.
                    if (distToShip < 20f)
                    {
                        isPlayerInWater = false;
                        Debug.Log("<color=yellow>Işınlanma Tespit Edildi:</color> Su hasarı zorla durduruldu.");
                        return; // Hasar vermeden döngüden çık
                    }
                }
            }

            // Eğer hala sudaysa hasar vermeye devam et
            if (SurvivalManager.Instance != null)
            {
                SurvivalManager.Instance.TakeDamage(saniyeBasiHasar * Time.deltaTime);
            }
        }
    }
}