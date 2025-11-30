using UnityEngine;

public class BotFXController : MonoBehaviour
{
    public FXPool VFXPrefab;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Transform attackOrigin;

    private BotController botController;

    private void Awake()
    {
        botController = GetComponent<BotController>();
    }
    private void OnEnable()
    {
        botController.AttackEvent += ShowVFX;
        botController.AttackEvent += AudioFX;
    }

    private void OnDisable()
    {
        botController.AttackEvent -= ShowVFX;
        botController.AttackEvent -= AudioFX;
    }

    private void ShowVFX()
    {
        VFXPrefab.PlayFX(attackOrigin.position,attackOrigin.rotation);
    }

    private void AudioFX()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
