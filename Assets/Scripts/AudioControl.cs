using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    private static AudioSource audioPlayer;

    public AudioClip jumpSound;
    public AudioClip coinSound;

    void Start() {
        audioPlayer = gameObject.AddComponent<AudioSource>();

        AudioControl.onPlayerJump += PlayPlayerJumpSound;
        AudioControl.onPlayerCollectCoin += PlayPlayerCollectCoinSound;
    }

    //Delegate:
    public delegate void OnAudioEvent(GameObject unit);

    //Eventtien linkitys:
    public static event OnAudioEvent onPlayerJump;
    public static event OnAudioEvent onPlayerCollectCoin;

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
}
