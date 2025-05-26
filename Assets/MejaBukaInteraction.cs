using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MejaBukaInteraction : MonoBehaviour
{
    public Image HoldEOpen;
    public Image CanEOpen;
    public Image ProgressBar;
    public Image Progress;
    public RigidbodyMovement playerMovement;

    private bool isInBukaZone = false;
    private bool isProgressRunning = false;

    public int SampahMeja = 0;
    public GameObject SampahMejaUI;
    public TextMeshProUGUI SampahMejaText;

    private void Start()
    {
        HideAllUI();
        if (SampahMejaUI != null)
            SampahMejaUI.SetActive(false);
    }

    private void Update()
    {
        if (SampahMejaText != null)
            SampahMejaText.text = SampahMeja.ToString();

        if (isProgressRunning) return;

        HideAllUI();

        if (playerMovement.currentHoldingStatus == HoldingStatus.TrashBag && isInBukaZone)
        {
            HoldEOpen.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(ProgressBarCoroutine());
            }
            return;
        }

        if (playerMovement.currentHoldingStatus == HoldingStatus.TrashBag && !isInBukaZone)
        {
            CanEOpen.gameObject.SetActive(true);
            return;
        }

        if (!playerMovement.isHolding && SampahMeja > 0 && isInBukaZone)
        {
            HoldEOpen.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                SampahMeja = Mathf.Max(0, SampahMeja - 1);

                // Random status
                HoldingStatus randomStatus = Random.Range(0, 2) == 0 ? HoldingStatus.Organik : HoldingStatus.Anorganik;
                playerMovement.currentHoldingStatus = randomStatus;

                // Random sub-kategori visual
                int variant = Random.Range(1, 4);
                playerMovement.currentVisualVariant = (randomStatus == HoldingStatus.Organik ? "organik" : "anorganik") + variant;

                playerMovement.UpdateVisualVariant();
                Debug.Log("Player mengambil sampah: " + playerMovement.currentVisualVariant);
            }
            return;
        }

        if (!isInBukaZone && SampahMeja > 0)
        {
            if (SampahMejaUI != null)
                SampahMejaUI.SetActive(true);
        }
        else
        {
            if (SampahMejaUI != null)
                SampahMejaUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInBukaZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInBukaZone = false;
        }
    }

    private IEnumerator ProgressBarCoroutine()
    {
        isProgressRunning = true;
        HideAllUI();

        ProgressBar.gameObject.SetActive(true);
        Progress.gameObject.SetActive(true);

        playerMovement.currentHoldingStatus = HoldingStatus.None;
        playerMovement.currentVisualVariant = "";
        playerMovement.HideAllVariantVisuals();

        float elapsedTime = 0f;
        float duration = 2f;

        ProgressBar.fillAmount = 1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            ProgressBar.fillAmount = progress;
            yield return null;
        }

        ProgressBar.fillAmount = 0f;
        ProgressBar.gameObject.SetActive(false);
        Progress.gameObject.SetActive(false);

        SampahMeja += 3;
        Debug.Log("Selesai proses buka. Sampah meja: " + SampahMeja);

        isProgressRunning = false;
    }

    private void HideAllUI()
    {
        HoldEOpen?.gameObject.SetActive(false);
        CanEOpen?.gameObject.SetActive(false);
        ProgressBar?.gameObject.SetActive(false);
        Progress?.gameObject.SetActive(false);
        SampahMejaUI?.SetActive(false);
    }
}
