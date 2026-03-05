using UnityEngine;

// Constraint that T must be a class inheriting MonoBehaviour
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
    // Static variable holding the unique instance
    private static T instance;

    // Static property for external access
    public static T Instance {
        get {
            // If instance doesn't exist yet
            if(instance == null) {
                // Find T type object in scene
                instance = (T) FindFirstObjectByType(typeof(T));

                // If still not found, create new GameObject and attach
                if(instance == null) {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    // Called immediately after instance is created
    protected virtual void Awake() {
        // If instance already exists (duplicate prevention)
        if(instance != null && instance != this) {
            Debug.LogWarning($"[Singleton] Instance already exists, destroying {typeof(T).Name}.");
            Destroy(gameObject);
            return;
        }

        // Set self as the unique instance
        instance = (T) this;

        // Persist object across scene transitions
        // Only apply at runtime due to editor behavior considerations
        if(Application.isPlaying) {
            DontDestroyOnLoad(gameObject);
        }

        // Method to delegate initialization to child classes
        OnInitialize();
    }

    // Clear static reference on destruction
    protected virtual void OnDestroy() {
        if(instance == this) { instance = null; }
    }

    // Method for child classes to override for initialization
    protected virtual void OnInitialize() { }
}