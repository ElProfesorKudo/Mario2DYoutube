using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{
    public static IEnumerator ExecuteCallbackDuringAmountOfTimeWaned(Action callbackAction, Action callbackActionAfterCoroutine, float timeWanted = 0.5f)
    {
        float localTime = timeWanted;
        while (localTime > 0)
        {
            callbackAction?.Invoke();
            localTime = localTime - Time.deltaTime;
            yield return null;
        }
        callbackActionAfterCoroutine?.Invoke();
    }

    public static IEnumerator CheckCurrentAnimationFinish(Animator animator, string animationNameHash, Action callbackAction)
    {
        float normalizedTimeAnimation = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // Wait until the next frame to ensure the animation has started
        yield return null;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationNameHash))
        {
            Debug.LogWarning(animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
            Debug.LogWarning("You are trying toc check the animation " + animationNameHash + " but it is not this one who is currently playing");
            yield break;
        }
        // Check if we check the good animation
        while (normalizedTimeAnimation >= 1.0 && animator.GetCurrentAnimatorStateInfo(0).IsName(animationNameHash))
        {
            normalizedTimeAnimation = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            yield return null;
        }
        callbackAction?.Invoke();
    }

    public static IEnumerator ExecuteCallbackAfterTimeWaned(Action callback, int frameWait = 1)
    {
        for (int i = 0; i <= frameWait; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        callback?.Invoke();
    }

    public static IEnumerator ExecuteCallbackAfterTimeWaned(Action callback, float timeWanted)
    {
        float localTime = timeWanted;
        while (localTime > 0)
        {
            localTime = localTime - Time.deltaTime;
            yield return null;
        }
        callback?.Invoke();
    }

    public static IEnumerator MoveAToB(GameObject myGameObject, Vector3 pointToReach, float speedTranslation, Action callback = null)
    {
        while (myGameObject.transform.position != pointToReach)
        {
            myGameObject.transform.position = Vector3.MoveTowards(myGameObject.transform.position, pointToReach, speedTranslation * Time.deltaTime);
            yield return null;
        }
        if (callback != null)
        {
            callback?.Invoke();
        }
    }

    public static IEnumerator ExecuteCallbacksSequentiallyWithDelay(Action callbackOne, Action callbackTwo, float timeWanted = 1.0f)
    {
        callbackOne?.Invoke();
        float localTime = timeWanted;
        while (localTime > 0)
        {
            localTime = localTime - Time.deltaTime;
            yield return null;
        }
        callbackTwo?.Invoke();
    }
}
