using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var a = animator.GetCurrentAnimatorStateInfo(0);
        if (a.IsName("FinalScene")) Destroy(this.gameObject);
    }
}
