using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState
{
    void OnEnter(GameObject gameObject);
    void OnUpdate();
    void OnExit();

}
