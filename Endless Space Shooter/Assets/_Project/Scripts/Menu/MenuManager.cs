using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject settings;
        
        public void OnClickPlay()
        {
            SceneManager.LoadScene(1);
        }

        public void OnClickExit()
        {
            Application.Quit();
        }
        
        public void OnClickSettings()
        {
            settings.SetActive(true);
        }
        public void OnClickCloseSettings()
        {
            settings.SetActive(false);
        }
    }
}
