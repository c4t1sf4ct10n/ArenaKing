using UnityEngine;
using UnityEngine.UI;

public class ScrollNavigation : MonoBehaviour
{
    public ScrollRect scrollRect;  // Reference au ScrollRect
    public RectTransform[] panels; // Liste des panels à afficher
    private int currentPanelIndex = 0; // Index du panel actuellement affiché

    void Start()
    {
        // S'assurer que l'on commence sur le panel "Main Menu"
        SetPanel(2); // Main Menu correspond au panel avec l'index 2
    }

    public void ScrollToPanel(int panelIndex)
    {
        Debug.Log("ScrollToPanel appelé avec index : " + panelIndex); // Message de débogage

        // Limite l'index au nombre de panels disponibles
        panelIndex = Mathf.Clamp(panelIndex, 0, panels.Length - 1);
        currentPanelIndex = panelIndex;

        // Calculer la position cible dans le ScrollRect
        float targetX = (float)panelIndex / (panels.Length - 1);

        // Appliquer le deplacement en modifiant l'ancre horizontale du ScrollRect
        scrollRect.horizontalNormalizedPosition = targetX;
    }

    private void SetPanel(int index)
    {
        currentPanelIndex = index;
        ScrollToPanel(index);
    }
}
