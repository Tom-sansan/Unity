using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private Ball Ball;

    [SerializeField]
    private Player Player;

    [SerializeField]
    private TextMesh GameStart;

    [SerializeField]
    private TextMesh LifeText;

    [SerializeField]
    private TextMesh GameOverText;

    [SerializeField]
    private TextMesh GameClearText;

    [SerializeField]
    private Transform BlockRoot;

    [SerializeField]
    private int Life = 3;

    private void Awake()
    {
        // Ball.enabled = false;
        GameOverText.gameObject.SetActive(false);
        GameClearText.gameObject.SetActive(false);
        foreach (Transform block in BlockRoot)
        {
            var onHitCollision = block.gameObject.AddComponent<OnHitCollisition>();
            onHitCollision.ObjName = Ball.name;
            onHitCollision.OnEnter = new UnityEngine.Events.UnityEvent();
            onHitCollision.OnEnter.AddListener(this.CheckBlocks);
        }
    }
    private IEnumerator Start()
    {
        Player.Stop();
        Ball.Stop();
        GameStart.gameObject.SetActive(true);
        LifeText.gameObject.SetActive(false);
        LifeText.text = $"Life：{Life}";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        Player.enabled = true;
        Ball.enabled = true;
        GameStart.gameObject.SetActive(false);
        LifeText.gameObject.SetActive(true);
    }

    public void ChangeLife(int num)
    {
        Life += num;
        LifeText.text = $"Life：{Life}";
        if (Life <= 0)
        {
            GameOverText.gameObject.SetActive(true);
            Ball.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(Start());
        }
    }

    public void CheckBlocks()
    {
        if (BlockRoot.childCount <= 1)
        {
            GameClearText.gameObject.SetActive(true);
            Ball.gameObject.SetActive(false);
        }
    }
}
