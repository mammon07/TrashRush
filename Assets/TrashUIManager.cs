using UnityEngine;

public class TrashUIManager : MonoBehaviour
{
    public GameObject infoUI;

    private void Start()
    {
        if (infoUI != null)
            infoUI.SetActive(false);
    }

    public void ShowInfo()
    {
        if (infoUI != null)
            infoUI.SetActive(true);
    }

    public void HideInfo()
    {
        if (infoUI != null)
            infoUI.SetActive(false);
    }
}
