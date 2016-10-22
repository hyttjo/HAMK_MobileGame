using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    private static AudioSource audioPlayer;

    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip heartSound;
    public AudioClip pickPowerUpSound;

    void Start() {
        audioPlayer = gameObject.AddComponent<AudioSource>();

        AudioControl.onPlayerJump += PlayPlayerJumpSound;
        AudioControl.onPlayerCollectCoin += PlayPlayerCollectCoinSound;
        AudioControl.onPlayerCollectHeart += PlayPlayerCollectHeartSound;
        AudioControl.onPlayerCollectPowerUp += PlayPlayerCollectPowerUpSound;
    }

    //Delegate:
    public delegate void OnAudioEvent(GameObject unit);

    //Eventtien linkitys:
    public static event OnAudioEvent onPlayerJump;
    public static event OnAudioEvent onPlayerCollectCoin;
    public static event OnAudioEvent onPlayerCollectHeart;
    public static event OnAudioEvent onPlayerCollectPowerUp;

    public static void PlayerJump(GameObject unit) {
        if (onPlayerJump != null) {
            onPlayerJump(unit);
        }
    }

    public static void PlayerCollectCoin(GameObject unit) {
        if (onPlayerCollectCoin != null) {
            onPlayerCollectCoin(unit);
        }
    }

    public static void PlayerCollectHeart(GameObject unit) {
        if (onPlayerCollectHeart != null) {
            onPlayerCollectHeart(unit);
        }
    }

    public static void PlayerCollectPowerUp(GameObject unit) {
        if (onPlayerCollectPowerUp != null) {
            onPlayerCollectPowerUp(unit);
        }
    }

    //Metodit jotka toistavat äänet
    public void PlayPlayerJumpSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(jumpSound, 0.5f);
        }
    }

    public void PlayPlayerCollectCoinSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(coinSound, 0.65f);
        }
    }

    public void PlayPlayerCollectHeartSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(heartSound, 0.65f);
        }
    }

    public void PlayPlayerCollectPowerUpSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(pickPowerUpSound, 0.65f);
        }
    }
}
