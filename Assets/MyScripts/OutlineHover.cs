using UnityEngine;
using UnityEngine.UI;

public class DogClickOutline : MonoBehaviour
{
    private Outline outline; // ��������� �������
    public GameObject dogNamePanel; // ������ ����� �����
    public GameObject chooseButton; // ������ "�������"

    private bool isSelected = false;
    private static DogClickOutline currentSelected = null;

    void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;

        if (chooseButton != null) chooseButton.SetActive(false);
    }

    void OnMouseDown()
    {
        if (dogNamePanel != null && dogNamePanel.activeSelf && currentSelected != null && currentSelected != this)
        {
            dogNamePanel.SetActive(false);
        }

        if (currentSelected != null && currentSelected != this)
        {
            currentSelected.Deselect();
        }

        isSelected = !isSelected;

        if (!isSelected && dogNamePanel != null && dogNamePanel.activeSelf && currentSelected == this)
        {
            dogNamePanel.SetActive(false);
        }

        if (outline != null) outline.enabled = isSelected;

        if (chooseButton != null)
        {
            chooseButton.SetActive(isSelected);
            if (isSelected)
            {
                Button btn = chooseButton.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(OnChooseButtonClicked);
                }
            }
        }

        currentSelected = isSelected ? this : null;
    }

    void OnChooseButtonClicked()
    {
        if (chooseButton != null) chooseButton.SetActive(false);

        if (dogNamePanel != null)
        {
            DogNamePanelController panelController = dogNamePanel.GetComponent<DogNamePanelController>();
            if (panelController != null)
            {
                string dogID = gameObject.name; // ������ ������� ��� �������-������!
                dogID = dogID.Replace(".ready", "");
                panelController.ShowPanel(dogID);

                // ��������� ���� ������ ������
                PlayerPrefs.SetInt("DogSelected", 1);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogWarning("DogNamePanelController �� ������ �� dogNamePanel");
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        if (outline != null) outline.enabled = false;
        if (chooseButton != null) chooseButton.SetActive(false);
    }
}
