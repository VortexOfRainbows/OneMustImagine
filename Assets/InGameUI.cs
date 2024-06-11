using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private Text giveUpText;
    private float saveTimeScale = 1f;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            if(!PauseMenu.activeSelf)
            {
                Time.timeScale = saveTimeScale;
            }
            else
            {
                saveTimeScale = Time.timeScale;
                Time.timeScale = 0.0f;
            }
        }
        giveUpText.text = Player.HasWon ? "Exit" : "Give Up";
    }
    public void GiveUp()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
