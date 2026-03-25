using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Image fillBar;

    private Stats targetStats;
    private RectTransform rectTransform;
    private Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        if (target != null)
        {
            targetStats = target.GetComponentInChildren<Stats>();

            if (targetStats == null)
            {
                Debug.LogWarning("⚠ HealthBar: Không tìm thấy Stats trong target!");
                return;
            }

            // Subscribe to health change event
            targetStats.OnHealthChanged += OnHealthChanged;

            // Cập nhật ngay lần đầu
            OnHealthChanged(targetStats.GetCurrentHealth(), targetStats.GetMaxHealth());
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        UpdateBar((int)current, (int)max);
    }

    public void UpdateBar(int currentValue, int maxValue)
    {
        if (fillBar != null)
            fillBar.fillAmount = Mathf.Clamp01((float)currentValue / maxValue);
    }

    private void OnDestroy()
    {
        if (targetStats != null)
            targetStats.OnHealthChanged -= OnHealthChanged;
    }
}
