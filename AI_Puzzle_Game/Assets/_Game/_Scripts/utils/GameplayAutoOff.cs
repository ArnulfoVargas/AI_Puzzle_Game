using UnityEngine;

public class GameplayAutoOff : BaseBehaviour, IAutoOff
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
        ReduceCounter();
    }

    private void ReduceCounter() {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}