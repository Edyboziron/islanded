using UnityEngine;
using StarterAssets; // StarterAssets paketini kullanýyoruz

public class FootstepManager : MonoBehaviour
{
    [Header("Ses Ayarlari")]
    public AudioSource audioSource;
    public AudioClip[] footstepSounds; // Farkli adim sesleri (cesitlilik iyidir)
    public float stepRate = 0.5f;      // Adim atma sýklýgý
    public float sprintStepRate = 0.3f; // Kosarken adim sýklýgý

    private ThirdPersonController _controller;
    private CharacterController _characterController;
    private float _stepTimer;

    void Start()
    {
        // Karakter bilesenlerini aliyoruz
        _controller = GetComponent<ThirdPersonController>();
        _characterController = GetComponent<CharacterController>();

        // Eger AudioSource atanmamissa otomatik bulmaya calis
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleFootsteps();
    }

    void HandleFootsteps()
    {
        // 1. KONTROL: Yerde miyiz ve hareket ediyor muyuz?
        // StarterAssets'in kendi Grounded degiskenini ve CharacterController'in hizini kullaniyoruz
        if (_controller.Grounded && _characterController.velocity.magnitude > 0.1f)
        {
            // Hareket hizina gore adim sýklýgýný belirle (Kosuyor mu?)
            float currentRate = _controller.SprintSpeed > _controller.MoveSpeed && _characterController.velocity.magnitude > _controller.MoveSpeed + 1f
                                ? sprintStepRate : stepRate;

            _stepTimer += Time.deltaTime;

            if (_stepTimer >= currentRate)
            {
                PlayFootstepSound();
                _stepTimer = 0;
            }
        }
        else
        {
            // Duruyorsak veya havadaysak sayaci sifirla
            _stepTimer = 0;
        }
    }

    void PlayFootstepSound()
    {
        if (footstepSounds.Length == 0) return;

        // Rastgele bir ses sec (Hep ayni ses cikmasin, kafa utuluyor yoksa)
        int index = Random.Range(0, footstepSounds.Length);

        // Sesi cal (Pitch'i hafif rastgele yaparak daha dogal hale getiriyoruz)
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(footstepSounds[index]);
    }
}