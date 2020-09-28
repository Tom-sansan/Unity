using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Menu Control
/// </summary>
public class Menu : MonoBehaviour
{
    [SerializeField] private ItemsDialog itemsDialog;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button itemsButton;
    [SerializeField] private Button recipeButton;

    void Start()
    {
        // Disable pause panel
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        itemsButton.onClick.AddListener(ToggleItemsDialog);
        recipeButton.onClick.AddListener(ToggleRecipeDialog);
    }

    /// <summary>
    /// Pause game
    /// </summary>
    private void Pause()
    {
        // Control/Stop time passing to set Time.timeScale to 0 
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    /// <summary>
    /// Resume game
    /// </summary>
    private void Resume()
    {
        // Start time passing to set 1
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Open/Close item window
    /// </summary>
    private void ToggleItemsDialog()
    {
        itemsDialog.Toggle();
    }

    /// <summary>
    /// Open/Close recipe window
    /// </summary>
    private void ToggleRecipeDialog()
    {
        // TODO
    }
}
