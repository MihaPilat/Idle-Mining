using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Analytics;
using Zenject;
using System;

public class FirebaseService : IInitializable
{
    private readonly SaveLoadService _saveLoadService;

    private DatabaseReference _dbReference;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    private bool _isInitialized = false;

    public event Action OnCloudSaveLoaded;

    public FirebaseService(SaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }

    public void Initialize()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"[Firebase] Не удалось запустить зависимости: {dependencyStatus}");
            }
        });
    }

    private void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;

        _dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Debug.Log("[Firebase] Инициализация успешна. Попытка входа...");

        LoginAnonymously();
    }

    private void LoginAnonymously()
    {
        _auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("[Firebase] Ошибка анонимного входа в систему!");
                return;
            }

            _user = task.Result.User;
            _isInitialized = true;
            Debug.Log($"[Firebase] Игрок успешно авторизован! Уникальный UID: {_user.UserId}");

            LogEvent("user_login_success");

            LoadFromCloud();
        });
    }

    public void SaveToCloud()
    {
        if (!_isInitialized || _user == null) return;

        string jsonSave = JsonUtility.ToJson(_saveLoadService.Data);

        _dbReference.Child("users").Child(_user.UserId).Child("progress").SetRawJsonValueAsync(jsonSave)
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("[Firebase] Прогресс успешно синхронизирован с облаком!");
                }
                else
                {
                    Debug.LogError("[Firebase] Ошибка синхронизации с облаком.");
                }
            });
    }

    public void LoadFromCloud()
    {
        if (!_isInitialized || _user == null) return;

        _dbReference.Child("users").Child(_user.UserId).Child("progress").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("[Firebase] Не удалось запросить данные из облака.");
                return;
            }

            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists && snapshot.Value != null)
            {
                string cloudJson = snapshot.GetRawJsonValue();
                Debug.Log("[Firebase] Облачный сейв найден! Загружаем в игру...");

                _saveLoadService.LoadFromRawJson(cloudJson);

                OnCloudSaveLoaded?.Invoke();
            }
            else
            {
                Debug.Log("[Firebase] Облако пустое (новый игрок). Используем локальный старт.");
            }
        });
    }

    public void LogEvent(string eventName)
    {
        if (!_isInitialized) return;
        FirebaseAnalytics.LogEvent(eventName);
    }

    public void LogMineUpgrade(string mineId, int level)
    {
        if (!_isInitialized) return;

        FirebaseAnalytics.LogEvent("mine_upgraded",
            new Parameter("mine_id", mineId),
            new Parameter("reached_level", level)
        );
    }
}