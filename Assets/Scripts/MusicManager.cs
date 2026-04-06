using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    void Awake()
    {
        if (instance == null) //if game is just opened begin playing music
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If music is already playing, do not play a new music on top of this
            Destroy(gameObject);
        }
    }
}