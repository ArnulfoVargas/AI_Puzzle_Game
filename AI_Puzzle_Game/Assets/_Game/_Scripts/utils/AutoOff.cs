using UnityEngine;

public class AutoOff : BaseBehaviour
{
    public float timeToDesactive;
    float counter;

    public void SetNewTime(float _time)
    {
        timeToDesactive = _time;
        counter = _time;
    }

    protected override void OnGameplayUpdate()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}