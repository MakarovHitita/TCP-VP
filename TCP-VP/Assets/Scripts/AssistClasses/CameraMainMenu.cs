using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    [SerializeField] private float _animationDuration;

    [SerializeField] private Vector3 _initPosition;
    [SerializeField] private Vector3 _initRotation;

    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _targetRotation;

    private AudioListener AudioListener { get; set; }

    private float MasterSound { get; set; }
    private float MusicSound { get; set; }
    private float EffectsSound { get; set; }
    private float ChatSound { get; set; }

    private void Awake()
    {
        MasterSound = ConfigBehaviour.Singleton.Options.MasterSound;
        MusicSound = ConfigBehaviour.Singleton.Options.MusicSound;
        EffectsSound = ConfigBehaviour.Singleton.Options.EffectsSound;
        ChatSound = ConfigBehaviour.Singleton.Options.ChatSound;

        AudioListener = GetComponent<AudioListener>();
        //ConfigBehaviour.AddOnNotifyMasterSoundChangesEvent()
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeMasterSound()
    {

    }

    public bool DoAnimation(int steps)
    {
        Vector3 posDisplacement = (_targetPosition - _initPosition) / steps;
        Vector3 rotDisplacement = (_targetRotation - _initRotation) / steps;
        if (transform.position + posDisplacement == _targetPosition)
        {
            transform.SetPositionAndRotation(_targetPosition, Quaternion.Euler(_targetRotation));
            return true;
        }
        transform.position += posDisplacement;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotDisplacement);
        return false;
    }

    public void RestartCamera() => transform.SetPositionAndRotation(_initPosition, Quaternion.Euler(_initRotation));
}
