using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private AudioSource audioSource;

    private void Awake()
    {
        // ����ģʽ��ʵ��
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        // ��Ҫ�ڳ����л�ʱ���� AudioManager ��Ϸ����
        DontDestroyOnLoad(this.gameObject);

        // ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();
    }

    // ��������
    public void PlayMusic(AudioClip musicClip)
    {
        audioSource.Stop();
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    // ֹͣ����
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // ��������Ƿ����ڲ���
    public bool IsMusicPlaying()
    {
        return audioSource.isPlaying;
    }
}
