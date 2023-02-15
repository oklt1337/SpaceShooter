using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public void OnClickPlay()
        {
            SceneManager.LoadScene(1);
        }
    }
}
