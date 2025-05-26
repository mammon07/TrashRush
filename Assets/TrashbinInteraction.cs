using UnityEngine;

public class TrashbinInteraction : MonoBehaviour
{
    public enum TrashType { Anorganik, Organik }
    public TrashType trashbinType;

    public RigidbodyMovement playerMovement;
    private bool isInZone = false;

    public GameObject pressEUI;  // Tampilkan saat dalam zona dan pegang sampah
    public GameObject infoUI;    // Tampilkan saat pegang sampah tapi di luar zona

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);

        if (infoUI != null)
            infoUI.SetActive(false);
    }

    private void Update()
    {
        HoldingStatus holding = playerMovement.currentHoldingStatus;

        bool isHoldingTrash = (holding == HoldingStatus.Organik || holding == HoldingStatus.Anorganik);

        // Tampilkan pressE UI jika di zona dan sedang pegang sampah
        if (isInZone && isHoldingTrash)
        {
            if (pressEUI != null) pressEUI.SetActive(true);
            if (infoUI != null) infoUI.SetActive(false);
        }
        // Jika tidak di zona dan sedang pegang sampah → tampilkan info UI
        else if (!isInZone && isHoldingTrash)
        {
            if (pressEUI != null) pressEUI.SetActive(false);
            if (infoUI != null) infoUI.SetActive(true);
        }
        else // Tidak pegang sampah
        {
            if (pressEUI != null) pressEUI.SetActive(false);
            if (infoUI != null) infoUI.SetActive(false);
        }

        // Proses buang sampah jika tekan E dan sedang dalam zona
        if (isInZone && isHoldingTrash && Input.GetKeyDown(KeyCode.E))
        {
            if ((trashbinType == TrashType.Anorganik && holding == HoldingStatus.Anorganik) ||
                (trashbinType == TrashType.Organik && holding == HoldingStatus.Organik))
            {
                Debug.Log("Sampah berhasil dibuang ke tempat yang benar.");
            }
            else
            {
                Debug.Log("Sampah salah tempat! Tetap dibuang.");
            }

            playerMovement.currentHoldingStatus = HoldingStatus.None;
            playerMovement.HideAllVariantVisuals();

            if (pressEUI != null) pressEUI.SetActive(false);
            if (infoUI != null) infoUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = true;
            Debug.Log($"Masuk zona tempat sampah {trashbinType}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = false;

            if (pressEUI != null) pressEUI.SetActive(false);
            Debug.Log($"Keluar zona tempat sampah {trashbinType}");
        }
    }
}
