
using UnityEngine;

public class CoinAnimation : BaseBehaviour
{
    const float DEGREES = 60, OFFSET = .25f;
    protected override void OnGameplayUpdate()
    {
        var newPosition = transform.position;
        newPosition.y = (Mathf.Sin(Time.fixedTime) * OFFSET) + 1;
        transform.position = newPosition;
        transform.Rotate(Vector3.up * (DEGREES * Time.fixedDeltaTime));
    }
}
