using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour
{
    private RectTransform rectComponent;
    private float rotateSpeed = -200f;
    public Image loading;

    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        loading = GetComponent<Image>();
    }

    private void Update()
    {
        rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        loading.fillAmount = Mathf.SmoothStep(0, 1, Mathf.PingPong(Time.time, 1));
        if (loading.fillAmount >= .99 && loading.fillClockwise)
        {
            loading.fillClockwise = !loading.fillClockwise;
        }
        if (loading.fillAmount <= .01 && !loading.fillClockwise)
        {
            loading.fillClockwise = !loading.fillClockwise;
        }
    }
}