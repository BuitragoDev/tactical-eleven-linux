using UnityEngine;
using UnityEngine.UIElements;

namespace TacticalEleven.Scripts
{
    public class Credits : MonoBehaviour
    {
        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;
            var btnVolver = root.Q<Button>(Constants.BACK_BUTTON);

            btnVolver.clicked += () =>
            {
                SceneLoader.Instance.LoadScene(Constants.MAIN_MENU_SCENE);
            };
        }
    }
}