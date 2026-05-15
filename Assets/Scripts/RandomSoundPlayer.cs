using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundPlayer : MonoBehaviour
{
    [Header("Ses Ayarlarý")]
    [Tooltip("Çalýnacak ses efektleri. Birden fazla koyarsan her seferinde rastgele birini seçer.")]
    public AudioClip[] soundClips;

    [Header("Zaman Ayarlarý")]
    [Tooltip("Ýki ses arasýndaki MÝNÝMUM bekleme süresi (saniye)")]
    public float minWaitTime = 3f;

    [Tooltip("Ýki ses arasýndaki MAKSÝMUM bekleme süresi (saniye)")]
    public float maxWaitTime = 10f;

    private AudioSource audioSource;

    void Start()
    {
        // Scriptin baðlý olduðu objedeki AudioSource bileþenini al
        audioSource = GetComponent<AudioSource>();

        // Eðer diziye en az bir ses eklendiyse sistemi baþlat
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
        // Sonsuz döngü: Obje sahnede var olduðu sürece çalýþmaya devam eder
        while (true)
        {
            // 1. Rastgele bir bekleme süresi belirle ve o kadar saniye bekle
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // 2. Çalýnacak rastgele bir ses klibi seç
            int randomIndex = Random.Range(0, soundClips.Length);
            AudioClip clipToPlay = soundClips[randomIndex];

            // 3. Sesi AudioSource'a ata ve çal
            audioSource.clip = clipToPlay;
            audioSource.Play();

            // Not: Ýstersen sesin üst üste binmemesi için sesin uzunluðu kadar ekstra bekletebilirsin
            // yield return new WaitForSeconds(clipToPlay.length);
        }
    }
}