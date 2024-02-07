using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComboSystem
{
    event System.Action OnAnimationFinished;
    void Initialize(AnimationController controller);
    void OnCrossPunch();
    void OnLightJab();
    
}