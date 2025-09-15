using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    [Tooltip("Aðýr çekimde zamanýn ne kadar yavaþlayacaðý")]
    public float slowMotionFactor = 0.2f;

    [Tooltip("Aðýr çekime geçiþin ne kadar yumuþak olacaðý")]
    public float slowdownSpeed = 5f;

    private float normalTimeScale = 1.0f;

    void Update()
    {
        // Zamaný yavaþça hedef deðere doðru yaklaþtýrýr (yumuþak geçiþ için)
        Time.timeScale = Mathf.Lerp(Time.timeScale, normalTimeScale, Time.unscaledDeltaTime * slowdownSpeed);

        // Fizik motorunun tutarlýlýðý için fixedDeltaTime'ý da güncelle
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void StartSlowMotion()
    {
        // Hedef zaman ölçeðini aðýr çekim faktörü olarak ayarla
        normalTimeScale = slowMotionFactor;
        Debug.Log("Aðýr Çekim BAÞLADI!");
    }

    public void StopSlowMotion()
    {
        // Hedef zaman ölçeðini normale döndür
        normalTimeScale = 1.0f;
        Debug.Log("Aðýr Çekim BÝTTÝ!");
    }
}