using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeIntensity = 0.2f;
    public float ShakeDuration = 0.3f;
    // 반동 효과
    public float RecoilIntensity = -0.2f;

    private CameraFollow _cameraFollow;
    private Vector3 _originalPosition;

    private void Start()
    {
        _cameraFollow = GetComponent<CameraFollow>();
    }

    public void ShakeCamera(Vector3 originalPosition)
    {
        StopAllCoroutines();
        _originalPosition = originalPosition;
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        // CameraFollow 비활성화
        if (_cameraFollow != null)
        {
            _cameraFollow.enabled = false;
        }

        float elapsed = 0.0f;
        while (elapsed < ShakeDuration)
        {
            float x = Random.Range(-1f, 1f) * ShakeIntensity;
            float y = Random.Range(-1f, 1f) * ShakeIntensity;

            transform.position = new Vector3(_originalPosition.x + x, _originalPosition.y + y, _originalPosition.z);
            yield return null;
            elapsed += Time.deltaTime;
        }

        transform.position = _originalPosition;

        // CameraFollow 다시 활성화
        if (_cameraFollow != null)
        {
            _cameraFollow.enabled = true;
        }
    }
}
