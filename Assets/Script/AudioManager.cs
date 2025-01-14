using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private AudioSource audioSource;

    private void Awake()
    {
        // 单例模式的实现
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        // 不要在场景切换时销毁 AudioManager 游戏对象
        DontDestroyOnLoad(this.gameObject);

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
    }

    // 播放音乐
    public void PlayMusic(AudioClip musicClip)
    {
        audioSource.Stop();
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    // 停止音乐
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // 检查音乐是否正在播放
    public bool IsMusicPlaying()
    {
        return audioSource.isPlaying;
    }
}
