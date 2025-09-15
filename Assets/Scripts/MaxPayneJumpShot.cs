using Invector.vCharacterController;
using StarterAssets;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MaxPayneJumpShoot : MonoBehaviour
{
    [Header("Referanslar")]
    public Animator animator;
    public Rigidbody rb;
    public vThirdPersonController controller;

    [Header("Zıplama Ayarları")]
    public float jumpForce = 8f;
    public float horizontalJumpForce = 10f;
    public float jumpDuration = 1.2f;

    [Header("Manuel Yavaşlatma Ayarları")]
    [Range(0.01f, 1f)]
    public float slowMotionFactor = 0.25f;

    [Header("Hareket Kontrol Türü")]
    public bool useRootMotion = false;

    [Header("Root Motion Mesafe Çarpanı")]
    [Range(1f, 5f)]
    public float rootMotionDistanceMultiplier = 2f;

    private bool isDuringBulletTimeJump = false;
    private bool isManualSlowMoActive = false;
    public static bool inputLocked = false;

    // Orijinal ayarları saklamak için
    private bool originalControllerState;
    private float originalAnimatorSpeed;
    private bool originalApplyRootMotion;

    [SerializeField] private AudioSource slowMo;

    void Start()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody>();

        // Orijinal değerleri kaydet
        originalAnimatorSpeed = animator.speed;
        originalApplyRootMotion = animator.applyRootMotion;
    }

    void Update()
    {
        if (isDuringBulletTimeJump) return;
        HandleJumpInput();
    }

    void OnAnimatorMove()
    {
        if (isDuringBulletTimeJump && useRootMotion)
        {
            // Root motion'dan gelen hareketi yavaşlat ama mesafeyi arttır
            Vector3 deltaPosition = animator.deltaPosition * slowMotionFactor * rootMotionDistanceMultiplier;
            transform.position += deltaPosition;
        }
    }

    void FixedUpdate()
    {
        if (isManualSlowMoActive)
        {
            rb.AddForce(Physics.gravity * slowMotionFactor, ForceMode.Acceleration);
        }
    }

    private void HandleJumpInput()
    {
        bool isAiming = Input.GetMouseButton(1);
        if (!isAiming) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.C) && moveX != 0)
        {
            StartCoroutine(PerformManualSlowMoJump(moveX));
        }
    }

    private IEnumerator PerformManualSlowMoJump(float direction)
    {
        isDuringBulletTimeJump = true;
        isManualSlowMoActive = true;
        MaxPayneJumpShoot.inputLocked = true;

        // Dünya ağır çekime giriyor
        TimeManager.Instance.SetSlowMotion(slowMotionFactor);
        slowMo.Play();

        DisableCharacterMovement();

        if (direction > 0) animator.SetTrigger("JumpRight");
        else animator.SetTrigger("JumpLeft");

        animator.speed = 1f; // Animator hızını elle yavaşlatmaya gerek yok artık
        rb.useGravity = false;

        if (useRootMotion)
        {
            animator.applyRootMotion = true;
            rb.isKinematic = true;
        }
        else
        {
            Vector3 jumpDirection = (direction > 0 ? transform.right : -transform.right);
            rb.linearVelocity = Vector3.zero;
            rb.AddForce((Vector3.up * jumpForce) +
                        (jumpDirection * horizontalJumpForce), ForceMode.Impulse);
        }

        // Artık gerçek zamanla bekliyoruz
        yield return new WaitForSecondsRealtime(jumpDuration);

        rb.useGravity = true;

        if (useRootMotion)
        {
            animator.applyRootMotion = originalApplyRootMotion;
            rb.isKinematic = false;
        }

        EnableCharacterMovement();

        // Zamanı normale döndür
        TimeManager.Instance.ResetTime();

        MaxPayneJumpShoot.inputLocked = false;
        isDuringBulletTimeJump = false;
        isManualSlowMoActive = false;
    }

    private void DisableCharacterMovement()
    {
        if (controller != null)
        {
            originalControllerState = controller.enabled;
            controller.enabled = false;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void EnableCharacterMovement()
    {
        if (controller != null)
        {
            controller.enabled = originalControllerState;
        }
    }

    void OnDisable()
    {
        if (isDuringBulletTimeJump)
        {
            rb.useGravity = true;

            if (useRootMotion)
            {
                animator.applyRootMotion = originalApplyRootMotion;
                rb.isKinematic = false;
            }

            EnableCharacterMovement();
            TimeManager.Instance.ResetTime();

            isDuringBulletTimeJump = false;
            isManualSlowMoActive = false;
            MaxPayneJumpShoot.inputLocked = false;
        }
    }
}
