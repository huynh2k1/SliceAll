using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataLevel
{
    public List<LevelData> levels = new List<LevelData>();
}

[Serializable]
public class LevelData
{
    public int OrderId;
    public int MapId;
    public PlayerData PlayerData;

    public List<EnemyData> listEnemy;
    public List<ObstacleData> listObstacle; // thêm dòng này
}

[Serializable]
public class PlayerData
{
    public TransformObj Transform;
}

[Serializable]
public class EnemyData
{
    public TransformObj Transform;
    public EnemyType Type;
}

[Serializable]
public enum EnemyType
{
    E0,
    E1,
    E2,
}

[Serializable]
public class ObstacleData
{
    public TransformObj Transform;
    public ObstacleType Type;
}

[Serializable]
public struct TransformObj
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 LocalScale;

    public static TransformObj FromTransform(Transform t)
    {
        return new TransformObj
        {
            Position = t.position,
            Rotation = t.rotation,
            LocalScale = t.localScale
        };
    }

    public void ApplyTo(Transform t)
    {
        t.position = Position;
        t.rotation = Rotation;
        t.localScale = LocalScale;
    }
}



