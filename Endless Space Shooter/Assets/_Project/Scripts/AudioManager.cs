using UnityEngine;

namespace _Project.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else 
                Destroy(gameObject);
        }
        
        public void PlayClip(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
}
