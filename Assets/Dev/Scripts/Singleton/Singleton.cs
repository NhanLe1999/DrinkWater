using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}


/// <summary>
/// Singleton mono alway exist through all screnes. 
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static bool IsInstanceInvalid()
    {
        return instance != null;
    }
    // IsInstanceInvalid()

    //void Awake() {
    //    singleton = gameObject.GetComponent<T>();
    //    DontDestroyOnLoad(this);
    //} // Awake ()

    public static T Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<T>();

            if (instance != null)
                return instance;
#if UNITY_EDITOR
            //Debug.LogError($"Not found Object for type {typeof(T).Name}");
#endif
            return instance;
        }


    } // Instance

}// SingletonMono
