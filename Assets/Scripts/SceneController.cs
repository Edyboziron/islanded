using UnityEngine;
using UnityEngine.SceneManagement; // Sahneleri yönetmek için şart

public class SceneController : MonoBehaviour
{
    // Bu metodu butonun OnClick kısmına bağlayacağız
    public void RestartCurrentScene()
    {
        // Zaman durdurulmuşsa (Time.timeScale = 0) normale döndür
        Time.timeScale = 1f;

        // Şu an aktif olan sahnenin indeksini alıp baştan yüklüyor
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}