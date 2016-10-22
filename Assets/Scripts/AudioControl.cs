using UnityEngine;
using System.Collections;

//Delegate:
public delegate void OnAudioEvent(Transform eventTransform);

public class AudioControl : MonoBehaviour {

    //Eventit:
    public static event OnAudioEvent onPlayerJump;
    public static event OnAudioEvent onPlayerCollectCoin;

    //Metodit:
    public static void PlayerJump(Transform eventTransform) {
        if (onPlayerJump != null) {
            onPlayerJump(eventTransform);
        }
    }

    public static void PlayerCollectCoin(Transform eventTransform) {
        if (onPlayerCollectCoin != null) {
            onPlayerCollectCoin(eventTransform);
        }
    }
}
