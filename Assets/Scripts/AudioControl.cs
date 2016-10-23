using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    private static AudioSource audioPlayer;

    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip heartSound;
    public AudioClip pickPowerUpSound;
    public AudioClip shootFireballSound;
    public AudioClip enemyDeathSound;
    public AudioClip playerDeathSound;
    public AudioClip springsound;
    public AudioClip smokeSound;
    public AudioClip brickSound;
    public AudioClip finishSound;

    void Start() {
        audioPlayer = gameObject.AddComponent<AudioSource>();

        AudioControl.onPlayerJump += PlayPlayerJumpSound;
        AudioControl.onPlayerCollectCoin += PlayPlayerCollectCoinSound;
        AudioControl.onPlayerCollectHeart += PlayPlayerCollectHeartSound;
        AudioControl.onPlayerCollectPowerUp += PlayPlayerCollectPowerUpSound;
        AudioControl.onPlayerShootFireball += PlayPlayerShootFireballSound;
        AudioControl.onEnemyDeath += PlayEnemyDeathSound;
        AudioControl.onPlayerDeath += PlayPlayerDeathSound;
        AudioControl.onSpringJump += PlaySpringSound;
        AudioControl.onSmokePuff += PlaySmokeSound;
        AudioControl.onBrickBreak += PlayBrickSound;
        AudioControl.onLevelFinish += PlayFinishSound;
    }

    void OnDestroy() {
        AudioControl.onPlayerJump -= PlayPlayerJumpSound;
        AudioControl.onPlayerCollectCoin -= PlayPlayerCollectCoinSound;
        AudioControl.onPlayerCollectHeart -= PlayPlayerCollectHeartSound;
        AudioControl.onPlayerCollectPowerUp -= PlayPlayerCollectPowerUpSound;
        AudioControl.onPlayerShootFireball -= PlayPlayerShootFireballSound;
        AudioControl.onEnemyDeath -= PlayEnemyDeathSound;
        AudioControl.onPlayerDeath -= PlayPlayerDeathSound;
        AudioControl.onSpringJump -= PlaySpringSound;
        AudioControl.onSmokePuff -= PlaySmokeSound;
        AudioControl.onBrickBreak -= PlayBrickSound;
        AudioControl.onLevelFinish -= PlayFinishSound;
    }

    //Delegate:
    public delegate void OnAudioEvent(GameObject unit);

    //Eventtien linkitys:
    public static event OnAudioEvent onPlayerJump;
    public static event OnAudioEvent onPlayerCollectCoin;
    public static event OnAudioEvent onPlayerCollectHeart;
    public static event OnAudioEvent onPlayerCollectPowerUp;
    public static event OnAudioEvent onPlayerShootFireball;
    public static event OnAudioEvent onEnemyDeath;
    public static event OnAudioEvent onPlayerDeath;
    public static event OnAudioEvent onSpringJump;
    public static event OnAudioEvent onSmokePuff;
    public static event OnAudioEvent onBrickBreak;
    public static event OnAudioEvent onLevelFinish;

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

    public static void PlayerShootFireball(GameObject unit) {
        if (onPlayerShootFireball != null) {
            onPlayerShootFireball(unit);
        }
    }

    public static void EnemyDeath(GameObject unit) {
        if (onEnemyDeath != null) {
            onEnemyDeath(unit);
        }
    }

    public static void PlayerDeath(GameObject unit) {
        if (onPlayerDeath != null) {
            onPlayerDeath(unit);
        }
    }

    public static void SpringJump(GameObject unit) {
        if (onSpringJump != null) {
            onSpringJump(unit);
        }
    }

    public static void SmokePuff(GameObject unit) {
        if (onSmokePuff != null) {
            onSmokePuff(unit);
        }
    }

    public static void BrickBreak(GameObject unit) {
        if (onBrickBreak != null) {
            onBrickBreak(unit);
        }
    }

    public static void LevelFinish(GameObject unit) {
        if (onLevelFinish != null) {
            onLevelFinish(unit);
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

    public void PlayPlayerShootFireballSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(shootFireballSound, 0.65f);
        }
    }

    public void PlayEnemyDeathSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(enemyDeathSound, 0.65f);
        }
    }

    public void PlayPlayerDeathSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(playerDeathSound, 0.65f);
        }
    }

    public void PlaySpringSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(springsound, 0.65f);
        }
    }

    public void PlaySmokeSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(smokeSound, 0.25f);
        }
    }

    public void PlayBrickSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(brickSound, 0.65f);
        }
    }

    public void PlayFinishSound(GameObject unit) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(finishSound, 0.65f);
        }
    }
}
