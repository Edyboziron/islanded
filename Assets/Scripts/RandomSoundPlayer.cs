using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Ses Ayarları")]
    [Tooltip("Çalınacak ses efektleri. Birden fazla koyarsan her seferinde rastgele birini seçer.")]
    public AudioClip[] soundClips;

    [Header("Zaman Ayarları")]
    [Tooltip("İki ses arasındaki MİNİMUM bekleme süresi (saniye)")]
    public float minWaitTime = 3f;

    [Tooltip("İki ses arasındaki MAKSİMUM bekleme süresi (saniye)")]
    public float maxWaitTime = 10f;

    private AudioSource audioSource;

    void Start()
    {
        // Scriptin bağlı olduğu objedeki AudioSource bileşenini al
        audioSource = GetComponent<AudioSource>();

        // Eğer diziye en az bir ses eklendiyse sistemi başlat
        if (soundClips.Length > 0)
        {
            StartCoroutine(PlayRandomSoundRoutine());
        }
        else
        {
            Debug.LogWarning("RandomSoundPlayer: Lütfen Inspector'dan 'Sound Clips' dizisine ses ekleyin!");
        }
    }

    private IEnumerator PlayRandomSoundRoutine()
    {
        // Sonsuz döngü: Obje sahnede var olduğu sürece çalışmaya devam eder
        while (true)
        {
            // 1. Rastgele bir bekleme süresi belirle ve o kadar saniye bekle
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 2. Çalınacak rastgele bir ses klibi seç
            int randomIndex = Random.Range(0, soundClips.Length);
            AudioClip clipToPlay = soundClips[randomIndex];

            // 3. Sesi AudioSource'a ata ve çal
            audioSource.clip = clipToPlay;
            audioSource.Play();

            // Not: İstersen sesin üst üste binmemesi için sesin uzunluğu kadar ekstra bekletebilirsin
            // yield return new WaitForSeconds(clipToPlay.length);
        }
    }
}