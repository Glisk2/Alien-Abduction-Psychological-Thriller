using SojaExiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GamePlayScene");
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
