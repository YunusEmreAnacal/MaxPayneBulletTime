using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Default Settings")]
    public float defaultTimeScale = 1f;
    private float defaultFixedDeltaTime;

    private float targetTimeScale;
    private bool isInBulletTime = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        defaultFixedDeltaTime = Time.fixedDeltaTime;
        targetTimeScale = defaultTimeScale;
    }

    private void Update()
    {
        // Her frame Time.timeScale'i zorla bizim istediðimiz deðere çekiyoruz
        if (Time.timeScale != targetTimeScale)
        {
            Time.timeScale = targetTimeScale;
            Time.fixedDeltaTime = defaultFixedDeltaTime * targetTimeScale;
        }
    }

    public void SetSlowMotion(float factor)
    {
        targetTimeScale = factor;
        isInBulletTime = true;
    }

    public void ResetTime()
    {
        targetTimeScale = defaultTimeScale;
        isInBulletTime = false;
    }

    public bool IsInBulletTime()
    {
        return isInBulletTime;
    }
}
