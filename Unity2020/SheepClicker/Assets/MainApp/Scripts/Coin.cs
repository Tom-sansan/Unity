using UnityEngine;

/// <summary>
/// Coin Class
/// </summary>
public class Coin : MonoBehaviour
{
    /// <summary>
    /// Wallet object, Destination and addition target
    /// </summary>
    public Wallet wallet;
    /// <summary>
    /// Amount to be added
    /// </summary>
    public int value;
    /// <summary>
    /// Waiting time for move
    /// </summary>
    private float waitTime;

    private void Start()
    {
        waitTime = Random.Range(0.1f, 0.3f);
    }

    private void Update()
    {
        // Countdown
        waitTime -= Time.deltaTime;
        if (waitTime > 0) return;
        // Vector to advance from the current position to the Wallet object
        var v = wallet.transform.position - transform.position;
        transform.position += v * Time.deltaTime * 20;
        // If get close enough, considered to arrive
        if (v.magnitude < 0.5f)
        {
            wallet.money += value;
            Destroy(gameObject);
        }
    }
}
