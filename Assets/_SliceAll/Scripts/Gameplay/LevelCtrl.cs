using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    public int OrderId;

    [SerializeField] PlayerCtrl _playerTransform;

    [Header("Enemy")]
    [SerializeField] List<BaseEnemy> enemyPrefabs;

    [Header("Obstacle")]
    [SerializeField] List<BaseObstacle> obstaclePrefabs;

    [SerializeField] private Transform _mapRoot;

    const string pathSave = "GameConfigs/DataLevel";
    const string fileName = "AllLevel.json";

    [SerializeField] TextAsset _data;

    public event Action OnClearEnemyAction;

    int _enemyAliveCount;

    public static Action<int> OnEnemyAliveChange;

    void OnEnable()
    {
        BaseEnemy.OnEnemyDeadAction += RemoveEnemy;
    }

    void OnDisable()
    {
        BaseEnemy.OnEnemyDeadAction -= RemoveEnemy;
    }

    public void InitLevel(int idLevel)
    {
        DestroyCurLevel();
        OrderId = idLevel;
        LoadLevel();
    }

    public int GetMaxLevelNumber()
    {
        string json = _data.text;
        DataLevel dataContainer = JsonUtility.FromJson<DataLevel>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
            return 0;

        return dataContainer.levels.Max(l => l.OrderId);
    }

    public void OnNextGame()
    {
        if (DataPrefs.CurrentLevel < GetMaxLevelNumber())
        {
            DataPrefs.CurrentLevel++;
        }
        else
        {
            DataPrefs.CurrentLevel = 0;
        }
    }

    [Button("Initialize")]
    public void DestroyCurLevel()
    {
        _playerTransform.gameObject.SetActive(false);

        foreach (Transform map in _mapRoot)
        {
            map.gameObject.SetActive(false);
        }

        List<BaseObstacle> existingObstacles = transform.GetComponentsInChildren<BaseObstacle>().ToList();
        foreach (BaseObstacle obstacle in existingObstacles)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(obstacle.gameObject);
            else
                Destroy(obstacle.gameObject);
#else
            Destroy(obstacle.gameObject);
#endif
        }

        List<BaseEnemy> existingEnemies = transform.GetComponentsInChildren<BaseEnemy>().ToList();
        foreach (BaseEnemy enemy in existingEnemies)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(enemy.gameObject);
            else
                Destroy(enemy.gameObject);
#else
            Destroy(enemy.gameObject);
#endif
        }
    }

    #region EDITOR

    string GetFilePath()
    {
        string directory = Path.Combine(Application.dataPath, pathSave);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return Path.Combine(directory, fileName);
    }

    private void ApplyMap(int mapId)
    {
        foreach (Transform map in _mapRoot)
        {
            map.gameObject.SetActive(false);
        }

        foreach (Transform map in _mapRoot)
        {
            MapIdentify id = map.GetComponent<MapIdentify>();
            if (id != null && id.MapId == mapId)
            {
                map.gameObject.SetActive(true);
                return;
            }
        }

        Debug.LogError("Không tìm thấy map có MapId = " + mapId);
    }

    private int GetCurrentMapId()
    {
        foreach (Transform map in _mapRoot)
        {
            if (map.gameObject.activeSelf)
            {
                MapIdentify id = map.GetComponent<MapIdentify>();
                if (id != null)
                    return id.MapId;
            }
        }

        return 0;
    }

    #endregion

    #region SAVE LEVEL

    [Button("Save Level To Json")]
    public void SaveLevel()
    {
        string path = GetFilePath();

        DataLevel dataContainer = new DataLevel();

        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            dataContainer = JsonUtility.FromJson<DataLevel>(existingJson);
        }

        if (dataContainer.levels == null)
        {
            dataContainer.levels = new List<LevelData>();
        }

        LevelData newLevel = new LevelData
        {
            OrderId = this.OrderId,
            MapId = GetCurrentMapId(),

            PlayerData = new PlayerData
            {
                Transform = TransformObj.FromTransform(_playerTransform.transform)
            },

            listEnemy = new List<EnemyData>(),
            listObstacle = new List<ObstacleData>()
        };

        List<BaseObstacle> obstacles = transform.GetComponentsInChildren<BaseObstacle>().ToList();

        foreach (BaseObstacle o in obstacles)
        {
            ObstacleData obstacleData = new ObstacleData
            {
                Transform = TransformObj.FromTransform(o.transform),
                Type = o.obstacleType
            };

            newLevel.listObstacle.Add(obstacleData);
        }

        List<BaseEnemy> enemies = transform.GetComponentsInChildren<BaseEnemy>().ToList();

        foreach (BaseEnemy e in enemies)
        {
            EnemyData enemyData = new EnemyData
            {
                Transform = TransformObj.FromTransform(e.transform),
                Type = e.Type
            };

            newLevel.listEnemy.Add(enemyData);
        }

        int existingIndex = dataContainer.levels.FindIndex(l => l.OrderId == OrderId);

        if (existingIndex >= 0)
        {
            dataContainer.levels[existingIndex] = newLevel;
        }
        else
        {
            dataContainer.levels.Add(newLevel);
        }

        string json = JsonUtility.ToJson(dataContainer, true);

        File.WriteAllText(path, json);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("Level saved to: " + path);
    }

    #endregion

    #region LOAD LEVEL

    [Button("Load Level")]
    public void LoadLevel()
    {
        string json = _data.text;

        DataLevel dataContainer = JsonUtility.FromJson<DataLevel>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
        {
            Debug.LogError("JSON không hợp lệ");
            return;
        }

        LevelData levelData = dataContainer.levels.FirstOrDefault(l => l.OrderId == OrderId);

        if (levelData == null)
        {
            Debug.LogError($"Không tìm thấy Level {OrderId}");
            return;
        }

        ApplyMap(levelData.MapId);

        if (levelData.PlayerData != null)
        {
            _playerTransform.gameObject.SetActive(true);
            levelData.PlayerData.Transform.ApplyTo(_playerTransform.transform);
            _playerTransform.Init(levelData.PlayerData.Transform.Rotation);
        }

        List<BaseObstacle> existingObstacles = transform.GetComponentsInChildren<BaseObstacle>().ToList();

        foreach (BaseObstacle obstacle in existingObstacles)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(obstacle.gameObject);
            else
                Destroy(obstacle.gameObject);
#else
            Destroy(obstacle.gameObject);
#endif
        }

        if (levelData.listObstacle != null)
        {
            foreach (ObstacleData obstacleData in levelData.listObstacle)
            {
                BaseObstacle prefab = obstaclePrefabs.FirstOrDefault(x => x.obstacleType == obstacleData.Type);

                if (prefab == null)
                {
                    Debug.LogError("Missing obstacle prefab: " + obstacleData.Type);
                    continue;
                }

                BaseObstacle newObstacle = Instantiate(prefab, transform);

                obstacleData.Transform.ApplyTo(newObstacle.transform);
            }
        }

        List<BaseEnemy> existingEnemies = transform.GetComponentsInChildren<BaseEnemy>().ToList();

        foreach (BaseEnemy enemy in existingEnemies)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(enemy.gameObject);
            else
                Destroy(enemy.gameObject);
#else
            Destroy(enemy.gameObject);
#endif
        }

        if (levelData.listEnemy == null || levelData.listEnemy.Count == 0)
        {
            Debug.LogWarning("Level không có enemy");
            return;
        }

        _enemyAliveCount = 0;

        foreach (EnemyData enemyData in levelData.listEnemy)
        {
            BaseEnemy prefab = enemyPrefabs.FirstOrDefault(x => x.Type == enemyData.Type);

            if (prefab == null)
            {
                Debug.LogError("Missing enemy prefab: " + enemyData.Type);
                continue;
            }

            BaseEnemy newEnemy = Instantiate(prefab, transform);

            enemyData.Transform.ApplyTo(newEnemy.transform);

            _enemyAliveCount++;
        }
        OnEnemyAliveChange?.Invoke(_enemyAliveCount);

        Debug.Log($"Load Level {OrderId} thành công. Enemy count: {_enemyAliveCount}");
    }

    #endregion

    public void RemoveEnemy(BaseEnemy e)
    {
        _enemyAliveCount--;
        OnEnemyAliveChange?.Invoke(_enemyAliveCount);

        if (_enemyAliveCount <= 0)
        {
            OnClearEnemyAction?.Invoke();
        }
    }
}