using UnityEngine;
using UnityEngine.UI;
public class UIEnemyHealth : MonoBehaviour
{
    public GameObject UIEnemyHealthSliderPrefab;
    public GameObject WorldSpaceCanvas;
    public Enemy TargetEnemy;
    private Slider _uiEnemyHealthSlider;
    public Vector3 Offset = new Vector3(0, 1f, 0);

    private void Start()
    {
        WorldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas");
        TargetEnemy = this.GetComponent<Enemy>();
        _uiEnemyHealthSlider = Instantiate(UIEnemyHealthSliderPrefab, WorldSpaceCanvas.transform).GetComponent<Slider>();
        TargetEnemy.OnDamaged += UpdateHealth;
    }

    private void Update()
    {
        _uiEnemyHealthSlider.transform.position = transform.position + Offset;
    }

    public void UpdateHealth()
    {
        _uiEnemyHealthSlider.value = TargetEnemy.Health / TargetEnemy.MaxHealth;
    }


}
