using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public string SceneName { get; private set; }
    [field: SerializeField] public Sprite NextLevelIcon { get; private set; }
    [field: SerializeField] public int CountFoodForOpenNextLevel { get; private set; }    

    [field: SerializeField] public List<SnakeSettings> EnemiesSettings { get; private set; }
    
}
