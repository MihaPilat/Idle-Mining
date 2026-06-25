using UnityEngine;
using Zenject;

public class AppLifecycleManager : MonoBehaviour
{
    private SaveLoadService _saveLoadService;

    [Inject]
    private void Construct(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            _saveLoadService.Save();
        }
    }

    private void OnApplicationQuit()
    {
        _saveLoadService.Save();
    }
}
