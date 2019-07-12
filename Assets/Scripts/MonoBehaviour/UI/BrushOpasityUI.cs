using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class BrushOpasityUI : MonoBehaviour
{
    private Contexts _context;
    [SerializeField] private Slider slider;

    private void Start()
    {
        _context = Contexts.sharedInstance;
        _context.game.brushEntity.OnComponentReplaced += BrushChange;

        slider.onValueChanged.AddListener(SliderChanged);
        UpdateSlider();
    }

    private void SliderChanged(float value)
    {
        var brush = _context.game.brush;
        float newOpacity = Mathf.Lerp(Constants.BRUSHOPACITYMIN, Constants.BRUSHOPACITYMMAX, value);
        if (newOpacity != brush.gui_BrushOpacity)
        {
            brush = new Brush
            {
                brushSize = brush.brushSize,
                gui_BrushColor = brush.gui_BrushColor,
                gui_BrushOpacity = newOpacity,
                brushType = brush.brushType
            };
            _context.game.ReplaceBrush(brush.gui_BrushColor, brush.brushSize, brush.gui_BrushOpacity, brush.brushType);
        }
    }

    private void BrushChange(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        var curSize = Math.DeLerpFromTo(Constants.BRUSHOPACITYMIN, Constants.BRUSHOPACITYMMAX,
            _context.game.brush.gui_BrushOpacity);
        if (curSize != slider.value)
        {
            slider.value = curSize;
        }
    }
}