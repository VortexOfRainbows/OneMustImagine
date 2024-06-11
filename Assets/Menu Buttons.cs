using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    private int bibooNumber;
    [SerializeField] private GameObject MenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        bibooNumber = Random.Range(1, 100);
        if (bibooNumber == 69)
        {
            MenuCanvas.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("(Decorated) Level Design Part A");
    }

}
