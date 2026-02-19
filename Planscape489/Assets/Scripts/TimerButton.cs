using UnityEngine;

public class TimerButton : MonoBehaviour
{
    [SerializeField] TimeHandSprite timeHand;
    [SerializeField] private AudioClip clickSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeHand = FindFirstObjectByType<TimeHandSprite>();
    }

    public void OnMouseDown() {
        AudioSource.PlayClipAtPoint(clickSound, gameObject.transform.position, 1.0f);
        timeHand.timer = 0;
    }
}