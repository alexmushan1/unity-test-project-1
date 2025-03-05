using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Restart").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        transform.Find("Quit").GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
        transform.Find("Back").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Start");
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
