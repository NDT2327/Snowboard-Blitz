using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public static BGMusic Instance;

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip tutorialMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip game2Music;
    private AudioSource audioSource;

    private void Awake()
    {
        // Đảm bảo BackgroundMusicManager tồn tại duy nhất trong game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true; // Lặp lại nhạc nền


    }

    private void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    private void UpdateMusic(string sceneName)
    {
        AudioClip newClip = null;

        if (sceneName == "Menu")
        {
            newClip = menuMusic;
        }
        else if (sceneName == "Control")
        {
            newClip = tutorialMusic;
        }
        else if (sceneName == "Game")
        {
            newClip = gameMusic;
        }
        else if (sceneName == "Game 1") // Nếu có thêm Game 2
        {
            newClip = game2Music;
        }
        if (newClip != null && audioSource.clip != newClip)
        {
            Debug.Log("Playing new music: " + newClip.name);
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No new clip or same clip is already playing.");
        }
    }
}
