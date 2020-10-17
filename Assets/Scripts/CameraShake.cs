using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Animator cameraAnimator;
    public static CameraShake instance;

    public void ShakeCamera()
    {
        cameraAnimator.SetTrigger("Shake");
    }

}
