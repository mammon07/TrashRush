using UnityEngine;

public enum HoldingStatus
{
    None,
    TrashBag,
    Organik,
    Anorganik
}

public class RigidbodyMovement : MonoBehaviour
{
    private Animator animator;
    private Vector3 PlayerMovementInput;

    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float rotationSpeed = 1440f;

    public HoldingStatus currentHoldingStatus = HoldingStatus.None;

    // UI/Visual item
    public GameObject trashBag;

    public GameObject organik1UI, organik2UI, organik3UI;
    public GameObject anorganik1UI, anorganik2UI, anorganik3UI;

    // Object 3D yang akan muncul sesuai varian
    public GameObject organik1Object, organik2Object, organik3Object;
    public GameObject anorganik1Object, anorganik2Object, anorganik3Object;

    // Nama varian visual yang sedang aktif (untuk UI & Object)
    public string currentVisualVariant = "";

    public bool isHolding
    {
        get { return currentHoldingStatus != HoldingStatus.None; }
    }

    public MejaInteraction mejaInteraction;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();

        if (trashBag != null)
            trashBag.SetActive(false);

        HideAllVariantVisuals();
    }

    void Update()
    {
        // Ambil trashbag
        if (mejaInteraction != null && mejaInteraction.pressEAmbil.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isHolding)
            {
                if (currentHoldingStatus == HoldingStatus.None)
                {
                    currentHoldingStatus = HoldingStatus.TrashBag;
                    HideAllVariantVisuals();
                }
                else
                {
                    currentHoldingStatus = HoldingStatus.None;
                    HideAllVariantVisuals();
                }
            }
        }

        UpdateTrashBagVisibility();

        PlayerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        HandleMovement();

        // Auto-hide UI saat tidak pegang organik/anorganik
        if (currentHoldingStatus != HoldingStatus.Organik && currentHoldingStatus != HoldingStatus.Anorganik)
        {
            HideAllVariantVisuals();
        }
    }


    void HandleMovement()
    {
        bool holding = isHolding;

        if (PlayerMovementInput.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(PlayerMovementInput.x, PlayerMovementInput.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 moveDir = PlayerMovementInput * Speed;
            PlayerBody.velocity = new Vector3(moveDir.x, PlayerBody.velocity.y, moveDir.z);

            animator.SetBool("isRunning", !holding);
            animator.SetBool("isRunningGun", holding);
            animator.SetBool("isRifleIdle", false);
        }
        else
        {
            PlayerBody.velocity = new Vector3(0f, PlayerBody.velocity.y, 0f);
            animator.SetBool("isRunning", false);
            animator.SetBool("isRunningGun", false);
            animator.SetBool("isRifleIdle", holding);
        }
    }

    void UpdateTrashBagVisibility()
    {
        if (trashBag != null)
        {
            trashBag.SetActive(currentHoldingStatus == HoldingStatus.TrashBag);
        }
    }

    public void UpdateVisualVariant()
    {
        HideAllVariantVisuals();

        switch (currentVisualVariant)
        {
            case "organik1":
                organik1UI?.SetActive(true);
                organik1Object?.SetActive(true);
                break;
            case "organik2":
                organik2UI?.SetActive(true);
                organik2Object?.SetActive(true);
                break;
            case "organik3":
                organik3UI?.SetActive(true);
                organik3Object?.SetActive(true);
                break;
            case "anorganik1":
                anorganik1UI?.SetActive(true);
                anorganik1Object?.SetActive(true);
                break;
            case "anorganik2":
                anorganik2UI?.SetActive(true);
                anorganik2Object?.SetActive(true);
                break;
            case "anorganik3":
                anorganik3UI?.SetActive(true);
                anorganik3Object?.SetActive(true);
                break;
        }
    }

    public void HideAllVariantVisuals()
    {
        // UI
        organik1UI?.SetActive(false);
        organik2UI?.SetActive(false);
        organik3UI?.SetActive(false);
        anorganik1UI?.SetActive(false);
        anorganik2UI?.SetActive(false);
        anorganik3UI?.SetActive(false);

        // Object
        organik1Object?.SetActive(false);
        organik2Object?.SetActive(false);
        organik3Object?.SetActive(false);
        anorganik1Object?.SetActive(false);
        anorganik2Object?.SetActive(false);
        anorganik3Object?.SetActive(false);
    }
}
