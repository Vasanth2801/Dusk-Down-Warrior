using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    void Awake()
    {
       if(instance == null)
       {
            instance = this;
            DontDestroyOnLoad(gameObject);
       }
       else
       {
            Destroy(gameObject);
       }
    }

    private void Start()
    {
        if(audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
