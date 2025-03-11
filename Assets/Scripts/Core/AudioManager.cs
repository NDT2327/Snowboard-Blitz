using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //singleton: đảm bảo chỉ có một AudioManager trong game
    //có thể gọi bất kỳ đâu mà không cần tham chiếu trực tiếp
    public static AudioManager instance;

    //Background Music
    [Header("Background Music")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip tutorialMusic;

    //Sfx
    [Header("Sound Effect")]
    [SerializeField] private AudioSource sfxSource;
    //button click
    [SerializeField] private AudioClip buttonClick;
    //snow slide
    [SerializeField] private AudioClip snowSlide;
    //speed boost
    [SerializeField] private AudioClip speedBoost;
    //crash sound
    [SerializeField] private AudioClip crashSound;
    //game over sound
    [SerializeField] private AudioClip gameOverSound;
    //congratulation sound
    [SerializeField] private AudioClip winSound;
    //jump sound
    [SerializeField] private AudioClip jumpSound;
    //on land sound
    [SerializeField] private AudioClip onLandSound;
    //get score sound
    [SerializeField] private AudioClip scoreSound;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);//giu Object nay khi chuyen scene
        }
        else
        {
            Destroy(gameObject); //Huy Object du thua neu da co Instance
            return;
        }
    }

    void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
        if (bgmSource == null || sfxSource == null)
        {
            Debug.LogError("AudioSource is NULL in AudioManager!");
        }
    }

    //cập nhật nhạc nền theo scene
    //khi bắt đầu game thì phải mở nhạc nền hợp với Scene hiện tại

    //khi component duoc bbat
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //huy khi component tat
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //hàm load scene lên để có thể cập nhật
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    //Ham chon va phat nhac nen dua tren scene
    private void UpdateMusic(string sceneName)
    {
        AudioClip newClip = null;

        //gan nhac tuong ung voi tung scene
        if (sceneName == "Menu")
        {
            newClip = menuClip;
        }
        else if (sceneName == "Control") newClip = menuClip;
        else if (sceneName == "Game 1") newClip = gameMusic;
        else if (sceneName == "Game") newClip = gameMusic;

        //chi thay doi va phat nhac nen neu clip moi khac clip hien tai
        if (newClip != null && bgmSource.clip != newClip)
        {
            bgmSource.clip = newClip;
            bgmSource.Play();
        }
    }

    //phat hieu ung am thanh mot lan
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);//phat am thanh khong ghi de len clip hien tai
        }
    }

    //cac ham de phat hieu am thanh cu the

    // button click
    public void PlayButtonSound()
    {
        PlaySFX(buttonClick);
    }

    //snow slide
    public void PlaySnowSlide()
    {
        PlaySFX(snowSlide);
    }
    //speed boost
    public void PlayBoostSpeed()
    {
        PlaySFX(speedBoost);
    }
    //crash sound
    public void PlayCrashSound()
    {
        PlaySFX(crashSound);
    }
    //game over 
    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
    }

    //congratulation
    public void PlayWinSound()
    {
        PlaySFX(winSound);
    }

    //jump
    public void PlayJumSound()
    {
        PlaySFX(jumpSound);
    }

    //on land sound
    public void PlayOnLandSound()
    {
        PlaySFX(onLandSound);
    }

    //stop all sound
    public void StopAllSound()
    {
        bgmSource.Stop();
    }

    public void RestartGameBGMusic()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void PlayScoreSound()
    {
        PlaySFX(scoreSound);
    }
}
