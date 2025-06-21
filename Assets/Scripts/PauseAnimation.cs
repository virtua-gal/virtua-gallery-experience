using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimation : MonoBehaviour
{
    public Animator anim;
    public void PauseAnim()
    {
        anim.StopPlayback();
    }
}
