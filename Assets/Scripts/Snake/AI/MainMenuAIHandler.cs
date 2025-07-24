using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class MainMenuAIHandler : MonoBehaviour
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private SnakeView _snakePfb;
    [SerializeField] private MeshFilter _earth;
    [SerializeField] private MenuFoodHandler _menuFoodHandler;
    
    private FoodSpawnSystem _foodSpawnSystem;
    private CancellationTokenSource _cancelationTokenSource; 
    private AISnakeFactory _snakeFactory;
    private List<AISnake> _snakes = new List<AISnake>(); 
    private void Awake()
    {
        _cancelationTokenSource = new CancellationTokenSource();
        _snakeFactory = new AISnakeFactory();
        _foodSpawnSystem = _menuFoodHandler.foodSpawn;

        SpawnSnakes().Forget() ;
    }
    private void OnDestroy()
    {
        _cancelationTokenSource.Cancel();
        _cancelationTokenSource.Dispose();
    }

    private async UniTaskVoid SpawnSnakes()
    {
        
        if(_snakes == null) _snakes = new List<AISnake>();

        for (int i = 0; i < _levelConfig.EnemiesSettings.Count; i++)
        {
            Vector3 point;
            Vector3 normal;
            GetRandomPointAndNormalOnMesh(_earth.transform, _earth.mesh, out point, out normal);

            AISnakeFactoryArg aISnakeFactoryArg = new AISnakeFactoryArg(_snakePfb, _levelConfig.EnemiesSettings[i], point, normal);

            var snake = _snakeFactory.Create(aISnakeFactoryArg);
            snake.IsDead.AsObservable().Subscribe(state => OnSnakeDead(state)).AddTo(this.gameObject);
            _snakes.Add(snake);

            await UniTask.WaitForSeconds(1f,cancellationToken: _cancelationTokenSource.Token);
        }
    }

    private void OnSnakeDead(bool state)
    {
        if (!state) return;
        var deadSnake = _snakes.Find(x=>x.IsDead.Value);
       
        if (deadSnake != null)
        {
            _snakes.Remove(deadSnake);

            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < deadSnake.TailParts.Count; i++)
            {
                positions.Add(deadSnake.TailParts[i].transform.position);
            }
            _foodSpawnSystem.SpawnFoodInPositions(positions);
        }

        if (_snakes.Count == 1)
        {
            SpawnSnakes();
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
