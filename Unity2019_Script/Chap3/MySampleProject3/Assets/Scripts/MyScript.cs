using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    string clip1 = "clip1";
    string localPosition_z = "localPosition.z";
    string anim1 = "anim1";
    string anim2 = "anim2";
    bool flg3_5;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize3_1_2();
        // Initialize3_3_4();
    }

    // Update is called once per frame
    void Update()
    {
        // Method3_1();
        // Method3_2();
        // Method3_3();
        // Method3_4();
        // Method3_5();
        Method3_12();
    }

    /// <summary>
    /// Animation for cube using animation clip
    /// </summary>
    private void Initialize3_1_2()
    {
        AnimationClip clip = new AnimationClip { legacy = true };
        AnimationCurve curve = AnimationCurve.Linear(0f, 3f, 3f, 3f);
        Keyframe key = new Keyframe(1.5f, 7f);
        curve.AddKey(key);
        clip.SetCurve(string.Empty, typeof(Transform), localPosition_z, curve);
        clip.wrapMode = WrapMode.Loop;
        Animation animation = CreateAnimationComponent();
        animation.AddClip(clip, clip1);
        animation.Play(clip1);
    }
    private void Method3_1()
    {
        SetTransformRotate();
    }

    /// <summary>
    /// Stop animation by key
    /// </summary>
    private void Method3_2()
    {
        SetTransformRotate();
        Animation animation = GetComponent<Animation>();
        if (Input.GetKeyDown(KeyCode.Space)) animation.Stop();
        if (Input.GetKeyUp(KeyCode.Space)) animation.Play(clip1);
    }

    /// <summary>
    /// Initialize for change of animation clip
    /// </summary>
    private void Initialize3_3_4()
    {
        
        Animation animation = CreateAnimationComponent();

        // Animation clip for the first
        AnimationClip clipA = new AnimationClip { legacy = true };
        AnimationCurve curveA = AnimationCurve.Linear(0f, 3f, 3f, 3f);
        Keyframe keyA = new Keyframe(1.5f, 10f);
        curveA.AddKey(keyA);
        clipA.SetCurve(string.Empty, typeof(Transform), localPosition_z, curveA);
        clipA.wrapMode = WrapMode.Loop;
        animation.AddClip(clipA, anim1);

        // Animation clip for the second
        AnimationClip clipB = new AnimationClip();
        clipB.legacy = true;
        AnimationCurve curveB = AnimationCurve.Linear(0f, 2f, 2f, 2f);
        Keyframe keyB1 = new Keyframe(0.5f, 7f);
        curveB.AddKey(keyB1);
        Keyframe keyB2 = new Keyframe(1.0f, 3f);
        curveB.AddKey(keyB2);
        Keyframe keyB3 = new Keyframe(1.5f, 7f);
        curveB.AddKey(keyB3);
        clipB.SetCurve(string.Empty, typeof(Transform), localPosition_z, curveB);
        clipB.wrapMode = WrapMode.Loop;
        animation.AddClip(clipB, anim2);

        animation.Play(anim1);
    }

    /// <summary>
    /// Change of animation clip
    /// </summary>
    private void Method3_3()
    {
        SetTransformRotate();
        Animation animation = GetComponent<Animation>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animation.IsPlaying(anim1)) animation.PlayQueued(anim2, QueueMode.PlayNow);
            else animation.PlayQueued(anim1, QueueMode.PlayNow);
        }
    }

    /// <summary>
    /// Change of animation clip using CrossFade
    /// </summary>
    private void Method3_4()
    {
        SetTransformRotate();
        Animation animation = GetComponent<Animation>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animation.IsPlaying(anim1)) animation.CrossFade(anim2, 5.0f);
            else animation.CrossFade(anim1, 5.0f);
        }
    }

    /// <summary>
    /// Control animation clip using animator
    /// </summary>
    private void Method3_5()
    {
        SetTransformRotate();
        Animator animator = GetComponent<Animator>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (flg3_5) animator.CrossFade(anim2, 5.0f);
            else animator.CrossFade(anim1, 5.0f);
            flg3_5 = !flg3_5;
        }
    }

    private void Method3_12()
    {
        SetTransformRotate();
        Animator animator = GetComponent<Animator>();
        int flag = animator.GetInteger("flag");
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (flag > 2) flag = 2;
            else flag++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (flag < -2) flag = -2;
            else flag--;
        }
        animator.SetInteger("flag", flag);
    }

    /// <summary>
    /// Create and return an animation component
    /// </summary>
    private Animation CreateAnimationComponent()
    {
        return gameObject.AddComponent<Animation>();
    }

    /// <summary>
    /// Set Transform.Rotate(1f, 1f, 1f)
    /// </summary>
    private void SetTransformRotate()
    {
        transform.Rotate(1f, 1f, 1f);
    }
}
