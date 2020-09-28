using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    protected override void OnDie()
    {
        base.OnDie();
        StartCoroutine(GoToGameOverCoroutine());
    }

    /// <summary>
    /// Coroutine of GameOverScene
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoToGameOverCoroutine()
    {
        // Go to GameOverScene after 3 sec
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOverScene");
    }
}
