using UnityEngine;
using UnityEngine.UIElements;

public class ModernSlider : VisualElement
{
    public VisualElement root;
    public VisualElement track;
    public VisualElement fill;
    public VisualElement thumb;

    public float Value { get; private set; } = 0.2f;   // 0–1
    public System.Action<float> OnValueChanged;

    bool dragging = false;

    public ModernSlider(VisualElement parent)
    {
        root = parent;

        track = new VisualElement();
        fill = new VisualElement();
        thumb = new VisualElement();

        root.Add(track);
        track.Add(fill);
        root.Add(thumb);

        // ---------- ROOT ----------
        root.style.backgroundColor = Color.clear;
        root.style.position = Position.Relative;

        // ---------- TRACK ----------
        track.style.backgroundColor = new Color(0.5f, 0.5f, 0.5f);
        track.style.height = 8;
        track.style.width = Length.Percent(100);
        track.style.top = 12;
        track.style.position = Position.Absolute;

        // Border radius (Unity 6 exige 4 propiedades)
        track.style.borderTopLeftRadius = 3;
        track.style.borderTopRightRadius = 3;
        track.style.borderBottomLeftRadius = 3;
        track.style.borderBottomRightRadius = 3;

        // ---------- FILL ----------
        fill.style.backgroundColor = new Color(0.2196f, 0.3059f, 0.2471f); // verde
        fill.style.height = 8;
        fill.style.width = Length.Percent(Value * 100);
        fill.style.borderTopLeftRadius = 3;
        fill.style.borderTopRightRadius = 3;
        fill.style.borderBottomLeftRadius = 3;
        fill.style.borderBottomRightRadius = 3;

        // ---------- THUMB ----------
        thumb.style.backgroundColor = new Color(0.0f, 0.2f, 0.0f);
        thumb.style.width = 20;
        thumb.style.height = 20;

        thumb.style.borderTopLeftRadius = 10;
        thumb.style.borderTopRightRadius = 10;
        thumb.style.borderBottomLeftRadius = 10;
        thumb.style.borderBottomRightRadius = 10;

        thumb.style.position = Position.Absolute;
        thumb.style.left = Length.Percent(Value * 100);
        thumb.style.top = 6;
        thumb.style.translate = new Translate(-10, 0);

        // ---------- Eventos ----------
        root.RegisterCallback<PointerDownEvent>(OnPointerDown);
        root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        root.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    // ---------------------------------------
    // Actualizar valor visual y callback
    // ---------------------------------------
    public void SetValue(float p)
    {
        Value = Mathf.Clamp01(p);

        fill.style.width = Length.Percent(Value * 100f);
        thumb.style.left = Length.Percent(Value * 100f);

        OnValueChanged?.Invoke(Value);
    }

    // ---------------------------------------
    // Interacción de ratón
    // ---------------------------------------
    void OnPointerDown(PointerDownEvent evt)
    {
        dragging = true;
        UpdateFromPointer(evt.position);
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (!dragging) return;
        UpdateFromPointer(evt.position);
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        dragging = false;
    }

    void UpdateFromPointer(Vector2 pointer)
    {
        var rect = root.worldBound;

        float localX = Mathf.Clamp(pointer.x - rect.x, 0, rect.width);
        float percent = localX / rect.width;

        SetValue(percent);
    }
}