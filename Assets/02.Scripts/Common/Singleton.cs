using UnityEngine;

/// <summary>
/// 싱글톤 패턴을 구현하는 기본 클래스
/// </summary>
/// <typeparam name="T">싱글톤으로 만들 클래스 타입</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance != null) return _instance;
                
                // 씬에서 인스턴스 찾기
                _instance = (T)FindObjectOfType(typeof(T));

                // 씬에 인스턴스가 없으면 새로 생성
                if (_instance == null)
                {
                    // 새 게임 오브젝트 생성
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = $"{typeof(T)} (Singleton)";

                    // 씬 전환 시에도 파괴되지 않도록 설정
                    //DontDestroyOnLoad(singletonObject);
                }
                else
                {
                    // 이미 인스턴스가 있으면 씬 전환 시에도 파괴되지 않도록 설정
                    //DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"[Singleton] Another instance of {typeof(T)} already exists! Destroying this instance.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        _applicationIsQuitting = true;
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}