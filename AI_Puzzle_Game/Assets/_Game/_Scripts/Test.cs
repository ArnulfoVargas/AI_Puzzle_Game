using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var inputs = Resources.Load<PlayerInputsReader>("PlayerInputsReader");

        inputs.OnMove += (dir) => {
            transform.position += dir;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
