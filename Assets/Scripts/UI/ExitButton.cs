using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
   private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null) return;
        button.onClick.AddListener(CloseGame);
    }

    private void OnDestroy()
    {

        if (button != null)
        {
            button.onClick.RemoveListener(CloseGame);
        }
    }

    private void CloseGame()
    {
        Application.Quit();
    }
}
