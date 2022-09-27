using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeEventTestWindow : EditorWindow
{
    private Toggle m_MyToggle;

    [MenuItem("Window/UI Toolkit/Change Event Test Window")]
    public static void ShowExample()
    {
        ChangeEventTestWindow wnd = GetWindow<ChangeEventTestWindow>();
        wnd.titleContent = new GUIContent("Change Event Test Window");
    }

    public void CreateGUI()
    {
        // Create a toggle
        m_MyToggle = new Toggle("Test Toggle") { name = "My Toggle" };
        rootVisualElement.Add(m_MyToggle);

        // Register a callback on the toggle
        m_MyToggle.RegisterValueChangedCallback(OnTestToggleChanged);

        // Register a callback on the parent
        rootVisualElement.RegisterCallback<ChangeEvent<bool>>(OnBoolChangedEvent);
    }

    private void OnBoolChangedEvent(ChangeEvent<bool> evt)
    {
        Debug.Log($"Toggle changed. Old value: {evt.previousValue}, new value: {evt.newValue}");
    }

    private void OnTestToggleChanged(ChangeEvent<bool> evt)
    {
        Debug.Log($"A bool value changed. Old value: {evt.previousValue}, new value: {evt.newValue}");
    }
}