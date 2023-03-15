using UnityEngine;
using Assets.Source.Services;
using Source.Root.Services;
using Root.Services.PersistentProgress;
using Source.Data;
using UnityEngine.SceneManagement;

public class HeroMove : MonoBehaviour, ISavedProgress
{
    private const float Epsilon = 0.001f;

    [SerializeField] private CharacterController _characterController;
    [SerializeField]private float _movespeed;
    
    private IInputService _inputService;
    private Camera _camera;

    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 movementVector = Vector3.zero;
        
        if(_inputService.Axis.sqrMagnitude > Epsilon)
        {
            movementVector = _camera.transform.TransformDirection(_inputService.Axis);
            movementVector.y = 0;
            movementVector.Normalize();

            transform.forward = movementVector;
        }

        movementVector += Physics.gravity;

        _characterController.Move(_movespeed * movementVector * Time.deltaTime);
    }
   
    
    private string CurrentLevel() => SceneManager.GetActiveScene().name;

    public void UpdateProgress(PlayerProgress progress) => 
        progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

    public void LoadProgress(PlayerProgress progress)
    {
        if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
        {
            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;

            if (savedPosition != null)
            {
                Warp(to: savedPosition);
            }
        }
    }

    private void Warp(Vector3Data to)
    {
        _characterController.enabled = false;

        transform.position = to.AsUnityVector().AddY(_characterController.height);

        _characterController.enabled = true;
    }
}
