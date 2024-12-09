using UnityEngine;

public class AnimationPlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animatorPlayer;
    [SerializeField] private RuntimeAnimatorController _runtimeAnimatorPlayerControllerAnimator;
    [SerializeField] private RuntimeAnimatorController _runtimeAnimatorPlayerControllerAnimatorOverride;


    // Parameters
    private string _isPlayerRunning = "IsRunning";
    private string _isPlayerSliding = "IsSliding";
    private string _isPlayerGrounded = "IsGrounded";
    private string _animationSpeedMultiplier = "AnimationSpeedMultiplier";
    private string _statePlayer = "StatePlayer";

    // The name of all animations




    public void SetBoolIsRunningParameter(bool value)
    {
        _animatorPlayer.SetBool(_isPlayerRunning, value);
    }
    public void SetBoolIsSlidingParameter(bool value)
    {
        _animatorPlayer.SetBool(_isPlayerSliding, value);
    }
    public void SetBoolIsGroundedParameter(bool value)
    {
        _animatorPlayer.SetBool(_isPlayerGrounded, value);
    }

    public void SetFloatAnimationSpeedMultiplierParameter(float value)
    {
        _animatorPlayer.SetFloat(_animationSpeedMultiplier, value);
    }
    public void SetIntStatePlayerParameter(PlayerState playerState)
    {
        _animatorPlayer.SetInteger(_statePlayer, (int)playerState);
    }

}
