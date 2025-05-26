using UnityEngine;

public class InfoUIZone : MonoBehaviour
{
    public RigidbodyMovement playerMovement;
    public GameObject infoUI;

    private bool isPlayerInZone = false;

    void Start()
    {
        if (infoUI != null)
            infoUI.SetActive(false);
    }

    void Update()
    {
        if (playerMovement == null || infoUI == null) return;

        bool isHoldingTrash = playerMovement.currentHoldingStatus == HoldingStatus.Organik ||
                              playerMovement.currentHoldingStatus == HoldingStatus.Anorganik;

        // Hanya aktifkan infoUI jika player pegang sampah DAN TIDAK berada di zona
        if (!isPlayerInZone && isHoldingTrash)
        {
            infoUI.SetActive(true);
        }
        else
        {
            infoUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}
