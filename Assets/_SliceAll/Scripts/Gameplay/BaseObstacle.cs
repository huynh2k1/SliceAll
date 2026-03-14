using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour
{
    public ObstacleType obstacleType;

    public virtual void OnCollisionWithBullet()
    {

    }
}

[Serializable]
public enum ObstacleType
{
    OBSTACLE,
    BARREL,
    O1,
}
