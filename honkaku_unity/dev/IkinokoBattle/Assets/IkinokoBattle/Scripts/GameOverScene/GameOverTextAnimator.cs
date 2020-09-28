using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTextAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var transformCache = transform;
        // Keep initial coordinate for end point
        var defaultPosition = transformCache.localPosition;
        // Move above
        transformCache.localPosition = new Vector3(0, 300);
        // Start moving animation
        transformCache.DOLocalMove(defaultPosition, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("GAME OVER");
                // Shake animation
                transformCache.DOShakePosition(1.5f, 100);
            });

        DOVirtual.DelayedCall(10, () =>
        {
            // Go to TitleScene after 10 sec
            SceneManager.LoadScene("TitleScene");
        });
    }
}
