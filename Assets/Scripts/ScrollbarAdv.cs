using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAdv : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject textParent;
    private float lastValue = 0f;

    public void SoundIfNewValue () {
        if(lastValue != scrollbar.value) {
            SoundManager.PlayClickSound();
            lastValue = scrollbar.value;
            LevelManager.Instance.SetTimeHandSpeed(scrollbar.value);

            textParent.transform.position = new Vector3(Mathf.Round((textParent.transform.parent.transform.position.x - (scrollbar.value - 0.5f) * 2 * (gameObject.transform.GetComponent<RectTransform>().sizeDelta.x / scrollbar.numberOfSteps)) * 100) / 100,
                textParent.transform.position.y, textParent.transform.position.z);
        }
    }
}