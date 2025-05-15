using UnityEngine;
using UnityEngine.SceneManagement;

public class HalfHealthCondition : MonoBehaviour
{
    //half health condition is when enemy gets to half health we load a different scene
    private Health enemyHealth;
    private bool hasTriggeredSceneChange = false;
    public string DialogueTag;
    public bool toDeath;

    void Start()
    {
        enemyHealth = GetComponent<Health>();

    }

    void Update()
    {
        if (!toDeath)
        {
            if (!hasTriggeredSceneChange && enemyHealth != null)
            {
                if (enemyHealth.currentHealth <= enemyHealth.maxHealth / 2)
                {
                    hasTriggeredSceneChange = true;
                    LoadNextScene();
                }
            }
        }
        else
        {
            if (enemyHealth.currentHealth <= 50)
            {
                hasTriggeredSceneChange = true;
                LoadNextScene();
                return;
            }
        }

    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Dialogue 1");
        if (DialogueTag == "Battle 0")
        {
            SceneManager.LoadScene("Dialogue 1");
        }
        else if (DialogueTag == "Battle P1")
        {
            SceneManager.LoadScene("Dialogue P2");
        }
        else if (DialogueTag == "Battle P2")
        {
            SceneManager.LoadScene("Dialogue 0");
        }
        else if (DialogueTag == "LuBu")
        {
            SceneManager.LoadScene("Defeat");
        }
        else if (DialogueTag == "Battle 1")
        {
            SceneManager.LoadScene("Victory");
        }
        else if (DialogueTag == "Tutorial D2")
        {
            SceneManager.LoadScene("Tutorial D2");
        }
    }
}
