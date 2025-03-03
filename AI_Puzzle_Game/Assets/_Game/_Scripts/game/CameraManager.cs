using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    [SerializeField] BindedLevels _bindedLevels;
    [SerializeField] private GameObject cameraControllerObject;
    private Transform _cameraControllerTransform;
    private static CameraManager instace;
    public static CameraManager Instace => instace;
    public LevelIslands levelIslands;
    
    void Awake()
    {
        if (instace == null)
        {
            instace = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        
        if (!_bindedLevels)
        {
            _bindedLevels = Resources.Load<BindedLevels>("BindedLevels");
        }

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            Initialize();
        };
    }

    private void Initialize()
    {
        _cameraControllerTransform = GameObject.Find("CameraController").transform;
        
        if (_cameraControllerTransform == null)
        {
            _cameraControllerTransform = Instantiate(cameraControllerObject).transform;
            _cameraControllerTransform.position = new Vector3(0, 0, 0);
        }

        var scene = SceneManager.GetActiveScene().buildIndex;

        if (_bindedLevels.IsSceneBinded(scene))
        {
            levelIslands = _bindedLevels.GetLevelIslands(scene);
        }
        else
        {
            levelIslands = null;
        }
    }

    public Tweener TravelToIsland(int islandIndex, out Vector3 endPosition)
    {
        if (levelIslands)
        {
            endPosition = levelIslands.TravelTo(islandIndex);
            return _cameraControllerTransform.DOMove(endPosition, 2f);
        }

        endPosition = Vector3.zero;
        return null;
    }
}
