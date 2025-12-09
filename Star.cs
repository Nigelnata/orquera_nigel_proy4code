using UnityEngine;
public class Star : MonoBehaviour
{
    void Update()
    {
        if (GalagaManager.Instance != null && GalagaManager.Instance.running)
        {
            if (transform.position.y < GalagaManager.Instance.CameraY - GalagaManager.Instance.WorldHeight / 2f - 5f)
            {
                Destroy(gameObject);
            }
        }
    }
}