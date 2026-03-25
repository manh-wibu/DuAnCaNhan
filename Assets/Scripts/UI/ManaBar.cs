using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2.5f, 0);
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
                Debug.LogWarning("⚠ ManaBar: Không tìm thấy Stats trong target!");
                return;
            }

            // Subscribe to mana change event
            targetStats.OnManaChanged += OnManaChanged;

            // Cập nhật ngay lần đầu
            OnManaChanged(targetStats.GetCurrentMana(), targetStats.GetMaxMana());
        }
    }

    private void OnManaChanged(float current, float max)
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
            targetStats.OnManaChanged -= OnManaChanged;
    }
}
