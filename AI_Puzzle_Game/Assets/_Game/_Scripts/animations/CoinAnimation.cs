
using UnityEngine;

public class CoinAnimation : BaseBehaviour
{
    const float DEGREES = 60, OFFSET = .25f;
    private bool Collected { get; set; } = false;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AnimationCurve curve;
    private float evaluationSpeed = 1.5f;
    private float time;

    public void OnCollect()
    {
        Collected = true;
        particles.Stop();
    }

    protected override void OnGameplayUpdate()
    {
        Oscilate();
        Evaluate();
    }

    private void Evaluate() {
        if (Collected) {
            time += Time.fixedDeltaTime * evaluationSpeed;

            var t = curve.Evaluate(time);
            transform.localScale = new Vector3(t,t,t);

            if (time >= 1) {
                gameObject.SetActive(false);
                particles.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void Oscilate() {
        var newPosition = transform.position;
        newPosition.y = (Mathf.Sin(Time.fixedTime) * OFFSET) + 1;
        transform.position = newPosition;
        transform.Rotate(Vector3.up * (DEGREES * Time.fixedDeltaTime));
    }
}
