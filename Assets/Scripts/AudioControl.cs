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
        PickupControl.OnCoinCollected += PlayPlayerCollectCoinSound;
        PickupControl.OnHeartCollected += PlayPlayerCollectHeartSound;
        PickupControl.OnPowerUpCollected += PlayPlayerCollectPowerUpSound;
        AudioControl.onPlayerShootFireball += PlayPlayerShootFireballSound;
        Health.OnEnemyKilled += PlayEnemyDeathSound;
        AudioControl.onPlayerDeath += PlayPlayerDeathSound;
        AudioControl.onSpringJump += PlaySpringSound;
        AudioControl.onSmokePuff += PlaySmokeSound;
        AudioControl.onBrickBreak += PlayBrickSound;
        AudioControl.onLevelFinish += PlayFinishSound;
    }

    void OnDestroy() {
        AudioControl.onPlayerJump -= PlayPlayerJumpSound;
        PickupControl.OnCoinCollected -= PlayPlayerCollectCoinSound;
        PickupControl.OnHeartCollected -= PlayPlayerCollectHeartSound;
        PickupControl.OnPowerUpCollected -= PlayPlayerCollectPowerUpSound;
        AudioControl.onPlayerShootFireball -= PlayPlayerShootFireballSound;
        Health.OnEnemyKilled -= PlayEnemyDeathSound;
        AudioControl.onPlayerDeath -= PlayPlayerDeathSound;
        AudioControl.onSpringJump -= PlaySpringSound;
        AudioControl.onSmokePuff -= PlaySmokeSound;
        AudioControl.onBrickBreak -= PlayBrickSound;
        AudioControl.onLevelFinish -= PlayFinishSound;
    }

    //Delegate:
    public delegate void OnAudioEvent();

    //Eventtien linkitys:
    public static event OnAudioEvent onPlayerJump;
    public static event OnAudioEvent onPlayerShootFireball;
    public static event OnAudioEvent onPlayerDeath;
    public static event OnAudioEvent onSpringJump;
    public static event OnAudioEvent onSmokePuff;
    public static event OnAudioEvent onBrickBreak;
    public static event OnAudioEvent onLevelFinish;

    public static void PlayerJump() {
        onPlayerJump();
    }

    public static void PlayerShootFireball() {
        onPlayerShootFireball();
    }

    public static void PlayerDeath() {
        onPlayerDeath();
    }

    public static void SpringJump() {
        onSpringJump();
    }

    public static void SmokePuff() {
        onSmokePuff();
    }

    public static void BrickBreak() {
        onBrickBreak();
    }

    public static void LevelFinish() {
        onLevelFinish();
    }

    //Metodit jotka toistavat äänet
    public void PlayPlayerJumpSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(jumpSound, 0.5f);
        }
    }

    public void PlayPlayerCollectCoinSound(GameObject e) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(coinSound, 0.65f);
        }
    }

    public void PlayPlayerCollectHeartSound(GameObject e) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(heartSound, 0.65f);
        }
    }

    public void PlayPlayerCollectPowerUpSound(GameObject e) {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(pickPowerUpSound, 0.65f);
        }
    }

    public void PlayPlayerShootFireballSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(shootFireballSound, 0.65f);
        }
    }

    public void PlayEnemyDeathSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(enemyDeathSound, 0.65f);
        }
    }

    public void PlayPlayerDeathSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(playerDeathSound, 0.65f);
        }
    }

    public void PlaySpringSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(springsound, 0.65f);
        }
    }

    public void PlaySmokeSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(smokeSound, 0.25f);
        }
    }

    public void PlayBrickSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(brickSound, 0.65f);
        }
    }

    public void PlayFinishSound() {
        if (audioPlayer != null) {
            audioPlayer.PlayOneShot(finishSound, 0.65f);
        }
    }
}
