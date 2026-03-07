using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    public int OrderId;

    [SerializeField] Transform _playerTransform;
    [SerializeField] BaseEnemy _enemyPrefab;
    [SerializeField] private Transform _mapRoot; // Gán MapTransform trong Inspector    

    const string pathSave = "GameConfigs/DataLevel";
    const string fileName = "AllLevel.json";

    [SerializeField] TextAsset _data;

    public void InitLevel(int idLevel)
    {
        OrderId = idLevel;

    }

    public int GetMaxLevelNumber()
    {
        string json = _data.text;
        DataLevel dataContainer = JsonUtility.FromJson<DataLevel>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
            return 0;

        return dataContainer.levels.Max(l => l.OrderId);
    }

    #region EDITOR ONLY
    string GetFilePath()
    {
        string directory = Path.Combine(Application.dataPath, pathSave);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log("Created directory: " + directory);
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

                Debug.LogError("Map chưa có MapIdentifier: " + map.name);
            }
        }

        Debug.LogWarning("Không có map nào active, mặc định MapId = 0");
        return 0;
    }
    #endregion

    #region SAVE LEVEL TO JSON
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
                Transform = TransformObj.FromTransform(_playerTransform)
            },
            listEnemy = new List<EnemyData>()
        };

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
            Debug.Log($"Level {OrderId} đã được ghi đè");
        }
        else
        {
            dataContainer.levels.Add(newLevel);
            Debug.Log($"Level {OrderId} đã được thêm mới");
        }

        string json = JsonUtility.ToJson(dataContainer, true);
        File.WriteAllText(path, json);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif

        Debug.Log("Level saved to: " + path);
    }
    #endregion

    #region LOAD LEVEL FROM JSON    
    [Button("Load Level To Json")]
    public void LoadLevel()
    {
        string json = _data.text;

        DataLevel dataContainer = JsonUtility.FromJson<DataLevel>(json);

        if (dataContainer == null || dataContainer.levels == null || dataContainer.levels.Count == 0)
        {
            Debug.LogError("File JSON không hợp lệ hoặc không có dữ liệu level!");
            return;
        }

        LevelData levelData = dataContainer.levels.FirstOrDefault(l => l.OrderId == OrderId);
        if (levelData == null)
        {
            Debug.LogError($"Không tìm thấy Level có OrderId = {OrderId}");
            return;
        }

        // ================= MAP =================
        ApplyMap(levelData.MapId);

        // ================= PLAYER =================
        if (levelData.PlayerData != null)
        {
            levelData.PlayerData.Transform.ApplyTo(_playerTransform);
        }
        else
        {
            Debug.LogWarning("PlayerData hoặc Transform bị null, bỏ qua load player.");
        }

        // ================= CLEAR ENEMY =================
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

        // ================= SPAWN ENEMY =================
        if (levelData.listEnemy == null || levelData.listEnemy.Count == 0)
        {
            Debug.LogWarning("Level không có enemy nào để load.");
            return;
        }

        foreach (EnemyData enemyData in levelData.listEnemy)
        {
            //if (enemyData == null || enemyData.Transform == null)
            //{
            //    Debug.LogWarning("EnemyData hoặc Transform bị null, bỏ qua enemy này.");
            //    continue;
            //}

            // Tạo enemy mới (không set transform ở Instantiate để tránh set 2 lần)
            BaseEnemy newEnemy = Instantiate(_enemyPrefab, this.transform);
            enemyData.Transform.ApplyTo(newEnemy.transform);

        }

        Debug.Log($"Load Level {OrderId} thành công. Enemy count: {levelData.listEnemy.Count}");
    }
    #endregion
}
