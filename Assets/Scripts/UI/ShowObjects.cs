using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjects : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private GameObject[] objectsToShow;
    
    private bool isWaitingForFullCycle = false;
    private bool hasInitialShown = false;
    private bool fullCycleCompleted = false;
    private float lastManaValue = 0f;
    private SelectableObject lastSelectedObject = null;

    private void Start()
    {
        // Lần đầu ẩn, chỉ hiện khi lên 50 lần đầu
        SetObjects(false);
        hasInitialShown = false;
        stats.OnManaChanged += OnManaChanged;
    }

    private void OnManaChanged(float current, float max)
    {
        // Lần đầu: khi mana đạt 50 từ dưới lên, hiện object
        if (!hasInitialShown && current >= 50f && lastManaValue < 50f)
        {
            SetObjects(true);
            hasInitialShown = true;
            lastManaValue = current;
            return;
        }

        // Sau lần đầu, cho phép hiện lại khi đã hoàn thành full cycle 100->0 và sau đó lên 50
        if (hasInitialShown && fullCycleCompleted && current >= 50f && lastManaValue < 50f)
        {
            SetObjects(true);
            fullCycleCompleted = false;
            lastManaValue = current;
            return;
        }

        // Bắt đầu chu kỳ khi đạt max (100)
        if (current >= max)
        {
            isWaitingForFullCycle = true;
            SetObjects(false);
        }

        // Chu kỳ hoàn thành khi từ 100 về 0
        if (isWaitingForFullCycle && lastManaValue > 0f && current <= 0f)
        {
            isWaitingForFullCycle = false;
            fullCycleCompleted = true;
            if (lastSelectedObject != null)
                lastSelectedObject.ResetToBlack();
            SetObjects(false); // Hiện khi đạt 50 tiếp theo
            lastManaValue = current;
            return;
        }

        // Nếu đang nạp mana mà chưa full cycle thì ẩn tiếp (trừ trường hợp chưa đạt lần đầu hoặc chưa hoàn thành full cycle)
        if (hasInitialShown && !isWaitingForFullCycle && !fullCycleCompleted && current > 0f && current < max)
        {
            SetObjects(false);
        }

        lastManaValue = current;
    }

    private void SetObjects(bool value)
    {
        foreach (var obj in objectsToShow)
        {
            obj.SetActive(value);
        }
    }

    private void ResetAllSelectablesToBlack()
    {
        foreach (var obj in objectsToShow)
        {
            SelectableObject selectable = obj.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                selectable.ResetToBlack();
            }
        }
    }

    public void HandleChoose(int index)
    {
        GameObject chosen = objectsToShow[index];
        SelectableObject selectable = chosen.GetComponent<SelectableObject>();
        if (selectable != null && !selectable.IsReady())
        {
            Debug.Log("Object đang cooldown!");
            return;
        }
        Debug.Log("Chọn object: " + index);
        selectable?.Activate();
        lastSelectedObject = selectable;  // Track object được chọn
        SetObjects(false);
        Time.timeScale = 1f;
    }

    public bool IsObjectSelected()
    {
        return lastSelectedObject != null && !lastSelectedObject.IsReady();
    }
}
