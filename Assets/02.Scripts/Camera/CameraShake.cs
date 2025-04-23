using System.Collections;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public float ShakeIntensity = 0.2f;
    public float ShakeDuration = 0.3f;
    // 반동 효과
    public float RecoilIntensity = -0.2f;

    public void ShakeCamera(Vector3 originalPosition)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(originalPosition));
    }

    private IEnumerator Shake(Vector3 originalPosition)
    {
        float elapsed = 0.0f;
        while (elapsed < ShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * ShakeIntensity;
            float y = Random.Range(-1f, 1f) * ShakeIntensity;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}
