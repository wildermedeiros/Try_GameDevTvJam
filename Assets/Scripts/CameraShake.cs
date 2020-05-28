using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Animator animator;

    public enum CameraTrigger { Default, Shake }

    private void Start() 
    {
        animator = GetComponent<Animator>();    
    }

    public IEnumerator ShakeCamera(float duration)
    {
        TriggerCamera(CameraTrigger.Shake);

        yield return new WaitForSeconds(duration);

        TriggerCamera(CameraTrigger.Default);
    }

    public void TriggerCamera(CameraTrigger trigger)
    {
        animator.SetTrigger(trigger.ToString());
    }
}
