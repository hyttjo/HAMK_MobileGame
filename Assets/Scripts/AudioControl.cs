using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    void Start() {
        //Debug.Log("Loaded");
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
    public static void PlayPlayerJumpSound(GameObject unit) {
        Debug.Log("JUMP!");
    }

    public void PlayPlayerCollectCoinSound(GameObject unit) {
        Debug.Log("COIN!");
    }
}
