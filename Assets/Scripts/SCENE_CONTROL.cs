using UnityEngine;
using UnityEngine.SceneManagement; // Sahneleri yönetmek için þart

public class SceneController : MonoBehaviour
{
    // Bu metodu butonun OnClick kýsmýna baðlayacaðýz
    public void RestartCurrentScene()
    {
        // Zaman durdurulmuþsa (Time.timeScale = 0) normale döndür
        Time.timeScale = 1f;

        // Þu an aktif olan sahnenin indeksini alýp baþtan yüklüyor
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}