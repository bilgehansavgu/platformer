﻿using Platformer.Core.FSM;
using UnityEngine;

namespace Platformer.Core.CharacterController
{
    public abstract class PlayerState_Base : StateBase<PlayerController.StateID>
    {
        protected PlayerController parent;

        protected PlayerState_Base(PlayerController parent)
        {
            this.parent = parent;
        }

        protected void PlayClip(string clipName)
        {
            parent.Animator.Play(clipName);
        }

        protected void PlayClip(string clipName, float index, int totalFramesInAnimation)
        {
            parent.Animator.Play(clipName, 0, (1f / totalFramesInAnimation) * index);
        }
        
        protected void HandleSpriteDirection(float xAxis)
        {
            if (xAxis > 0)
                parent.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (xAxis < 0)
                parent.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
