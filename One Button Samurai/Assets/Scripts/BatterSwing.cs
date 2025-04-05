using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BatterSwing : MonoBehaviour
{
    public float maxX = 13f;
    public float minX = -3f;
    public float speed = 10f;

    private Animator anim;
    private int animLayer = 0;
    private Dictionary<int, AnimationClip> hashToClip = new Dictionary<int, AnimationClip>();
    public AnimationClip swingClip;
    int swingClipHash = Animator.StringToHash("Base Layer" + ".tempPlayer_swing");

    public float previousTimeIndex = 0f;

    public Transform mouseFollow;

    private void Awake()
    {
        hashToClip.Add(swingClipHash, swingClip);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0f;
    }

    void Update()
    {
        // paused
        if (PauseHandler.instance.pauseMenu.activeSelf) { return; }

        // unpaused

        // get new progress to lerp to based on mouse follow position
        float mousePosX = mouseFollow.position.x;
        float newProgress = Mathf.InverseLerp(minX, maxX, mousePosX);

        // get the animation's current progress
        float currentProgress = GetCurrentAnimatorTime();

        float distance = newProgress - currentProgress;

        // find next part based on current and new progress
        // and also the frickin distance so it can go backwards too
        float journey;
        if(distance >= 0f)
        {
            journey = (currentProgress + Time.deltaTime * speed) / Mathf.Abs(distance);
        }
        else
        {
            journey = (currentProgress - Time.deltaTime * speed) / Mathf.Abs(distance);
        }
        float timeIndex = Mathf.Lerp(currentProgress, newProgress, journey);
        previousTimeIndex = timeIndex;

        // play animation at the time index
        anim.Play(swingClip.name, animLayer, timeIndex);
    }

    float GetCurrentAnimatorTime()
    {
        // gets the state of the animator
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(animLayer);
        // dont know what a hash is but we get that
        int currentAnimHash = animState.fullPathHash;
        // finds the current animation clip
        AnimationClip clip = GetClipFromHash(currentAnimHash);

        // get the current time
        float currentTime = clip.length * animState.normalizedTime;
        return currentTime;
    }

    AnimationClip GetClipFromHash(int hash)
    {
        AnimationClip clip;
        if (hashToClip.TryGetValue(hash, out clip))
        {
            return clip;
        }
        else
        {
            return null;
        }
    }

    public bool IsAtMaxSwing(out float battingSpeed)
    {
        float currentTimeIndex = GetCurrentAnimatorTime();

        battingSpeed = currentTimeIndex - previousTimeIndex;

        if(currentTimeIndex > 0.9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
