using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.VFX;

public class CharacterAiming : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform aimTarget;
    public Rig aimRig; // Animation Rigging -> Rig component

    [Header("Settings")]
    public float aimDistance = 20f;
    public float aimSmoothSpeed = 5f;

    private bool isAiming;
    [SerializeField] LayerMask aimMask; // Sadece d��man / environment i�in layer mask
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private AudioSource GunAudio;

    public GameObject crosshairUI; // Canvas i�indeki crosshair objesi
    
    


    void Update()
    {
        // Sa� t�k bas�l� m� kontrol et
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        // Rig weight'i yumu�ak ge�i�le a�/kapat
        float targetWeight = isAiming ? 1f : 0f;
        aimRig.weight = Mathf.Lerp(aimRig.weight, targetWeight, Time.deltaTime * aimSmoothSpeed);

        

        // E�er aim yap�yorsak hedef pozisyonu g�ncelle
        if (isAiming)
        {
            UpdateAimTarget();
            
        }
        crosshairUI.SetActive(isAiming);
        if (Input.GetMouseButtonDown(0) && isAiming)
        {
            Fire();
        }
    }

    void UpdateAimTarget()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimMask))
        {
            aimTarget.position = hit.point;
            
        }
        else
        {
            aimTarget.position = ray.origin + ray.direction * aimDistance;
        }

    }

    void Fire()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GunAudio.Play();
            if (muzzleFlash != null)
            {
                // Mevcut t�m partik�lleri durdur ve temizle
                muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                muzzleFlash.Clear();

                // Tekrar ba�lat
                muzzleFlash.Play();
            }

            // Hedef vuruldu
            Debug.Log("Hit: " + hit.collider.name);

            hit.collider.GetComponent<NPCHealth>()?.TakeDamage(100);
        }
    }

}
