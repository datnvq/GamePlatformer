using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    public void FinishRespawn() => player.RespawnFinished(true);
}
