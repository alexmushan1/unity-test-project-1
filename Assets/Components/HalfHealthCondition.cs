using UnityEngine;
using UnityEngine.SceneManagement;

public class HalfHealthCondition : MonoBehaviour
{
    //half health condition is when enemy gets to half health we load a different scene
    private Health enemyHealth;
    private bool hasTriggeredSceneChange = false;

    void Start()
    {
        enemyHealth = GetComponent<Health>();
    }

    void Update()
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

    void LoadNextScene()
    {
        SceneManager.LoadScene("Dialogue 1");
    }
}
