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
    E1,
    E2,
    E3,
    E4,
    E5,
    E6,
    E7,
    E8,
    E9,
    E10,
    E11,
    E12,
    E13,
    E14,
    E15,
    E16, 
    E17,
    E18,    
    E19,
    E20,
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



