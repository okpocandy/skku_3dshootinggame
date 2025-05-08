using UnityEngine;
using UnityEngine.UI;
public class UIEnemyHealth : MonoBehaviour
{
    public GameObject UIEnemyHealthSliderPrefab;
    public GameObject WorldSpaceCanvas;
    public Enemy TargetEnemy;
    protected Slider _uiEnemyHealthSlider;
    public Vector3 Offset = new Vector3(0, 1f, 0);

    protected virtual void Start()
    {
        WorldSpaceCanvas = GameObject.FindWithTag("WorldSpaceCanvas");
        TargetEnemy = this.GetComponent<Enemy>();
        _uiEnemyHealthSlider = Instantiate(UIEnemyHealthSliderPrefab, WorldSpaceCanvas.transform).GetComponent<Slider>();
        TargetEnemy.OnDamaged += UpdateHealth;
        TargetEnemy.OnDie += OnTargetDie;
    }

    protected virtual void Update()
    {
        if(_uiEnemyHealthSlider != null)
        {
            _uiEnemyHealthSlider.transform.position = transform.position + Offset;
        }
    }

    public virtual void UpdateHealth()
    {
        _uiEnemyHealthSlider.value = TargetEnemy.Health / TargetEnemy.MaxHealth;
    }

    protected virtual void OnTargetDie()
    {
        Destroy(_uiEnemyHealthSlider.gameObject);
    }
}
