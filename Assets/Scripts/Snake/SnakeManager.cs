using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SnakeManager
{
    private AISnakeFactory _iaFactory;    
    private List<ISnake> _snakes;
    private SnakeView _aiViewPfb;
    private LevelConfig _levelConfig;
    
    private SnakeView _mainSnake;
    private SnakeSettings _mainSnakeSettings;
    private IJoystick _joystick;
    private FoodSpawnSystem _foodSpawnSystem;

    public IReadOnlyList<ISnake> Snakes => _snakes;

    public SnakeManager(SnakeView snakePfbAI, LevelConfig levelConfig, SnakeView mainSnake, SnakeSettings mainSnakeSettings, IJoystick joystick, FoodSpawnSystem foodSpawnSystem)
    {
        _iaFactory = new AISnakeFactory();
        _snakes = new List<ISnake>();

        _aiViewPfb = snakePfbAI;
        _levelConfig = levelConfig;
        _mainSnake = mainSnake;
        _mainSnakeSettings = mainSnakeSettings;
        _joystick = joystick;
        _foodSpawnSystem = foodSpawnSystem;
    }

    public MainSnake InitMainSnake()
    {
        MainSnake mainSnake = new MainSnake(_mainSnake, _mainSnakeSettings, _joystick);
        _snakes.Add(mainSnake);
        return mainSnake;
    }

    public async UniTask SpawnAI(MeshFilter meshFilter)
    {
        for (int i = 0; i < _levelConfig.EnemiesSettings.Count; i++)
        {
            Vector3 point;
            Vector3 normal;
            GetRandomPointAndNormalOnMesh(meshFilter.transform, meshFilter.mesh, out point, out normal);

            var _aISnake = _iaFactory.Create(new AISnakeFactoryArg(_aiViewPfb, _levelConfig.EnemiesSettings[i], point, normal));
            _snakes.Add(_aISnake);
            _aISnake.IsDead.AsObservable().Subscribe(state => OnSnakeDead(state)).AddTo(_aISnake.View);

            await UniTask.WaitForSeconds(1f);
        }
    }

    private void OnSnakeDead(bool state)
    {
        var deadSnake = _snakes.Find(x=>x.IsDead.Value);
        if (deadSnake != null)
        {
            _snakes.Remove(deadSnake);
            List<Vector3> tilePos = new List<Vector3>();
            for (int i = 0; i < deadSnake.TailParts.Count; i++)
            {
                tilePos.Add(deadSnake.TailParts[i].transform.position);               
            }

            _foodSpawnSystem.SpawnFoodInPositions(tilePos);
        }
    }

    private void GetRandomPointAndNormalOnMesh(Transform transform, Mesh mesh, out Vector3 point, out Vector3 normal)
    {        
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        int randomTriangleIndex = Random.Range(0, triangles.Length / 3) * 3;
        
        Vector3 v1 = vertices[triangles[randomTriangleIndex]];
        Vector3 v2 = vertices[triangles[randomTriangleIndex + 1]];
        Vector3 v3 = vertices[triangles[randomTriangleIndex + 2]];

        Vector3 n1 = normals[triangles[randomTriangleIndex]];
        Vector3 n2 = normals[triangles[randomTriangleIndex + 1]];
        Vector3 n3 = normals[triangles[randomTriangleIndex + 2]];
        
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f);

        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }
        
        point = v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
        
        normal = (n1 + n2 + n3).normalized; // or n1 * (1 - r1 - r2) + n2 * r1 + n3 * r2
        
        point = transform.TransformPoint(point);
        normal = transform.TransformDirection(normal).normalized;
    }
}
