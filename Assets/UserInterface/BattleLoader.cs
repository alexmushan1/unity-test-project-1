using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("Load Battle").GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Battle");
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
