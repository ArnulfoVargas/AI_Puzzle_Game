using System.Collections;
using UnityEngine;

public class AutoOff : MonoBehaviour, IAutoOff
{
    IEnumerator timer;

    public void SetNewTime(float time)
    {
        if (timer != null) {
            StopCoroutine(timer);
        }
        timer = Timer(time);
        StartCoroutine(timer);
    }

    private void Off()
    {
        timer = null;
        gameObject.SetActive(false);
    }

    IEnumerator Timer(float time) {
        yield return new WaitForSeconds(time);
        Off();
    }
}