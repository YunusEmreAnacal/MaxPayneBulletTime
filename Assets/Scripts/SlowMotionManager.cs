using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    [Tooltip("A��r �ekimde zaman�n ne kadar yava�layaca��")]
    public float slowMotionFactor = 0.2f;

    [Tooltip("A��r �ekime ge�i�in ne kadar yumu�ak olaca��")]
    public float slowdownSpeed = 5f;

    private float normalTimeScale = 1.0f;

    void Update()
    {
        // Zaman� yava��a hedef de�ere do�ru yakla�t�r�r (yumu�ak ge�i� i�in)
        Time.timeScale = Mathf.Lerp(Time.timeScale, normalTimeScale, Time.unscaledDeltaTime * slowdownSpeed);

        // Fizik motorunun tutarl�l��� i�in fixedDeltaTime'� da g�ncelle
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void StartSlowMotion()
    {
        // Hedef zaman �l�e�ini a��r �ekim fakt�r� olarak ayarla
        normalTimeScale = slowMotionFactor;
        Debug.Log("A��r �ekim BA�LADI!");
    }

    public void StopSlowMotion()
    {
        // Hedef zaman �l�e�ini normale d�nd�r
        normalTimeScale = 1.0f;
        Debug.Log("A��r �ekim B�TT�!");
    }
}