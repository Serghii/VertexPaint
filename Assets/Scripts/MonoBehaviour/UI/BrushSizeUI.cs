using System;
using Entitas;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BrushSizeUI : MonoBehaviour
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
        float newSize = Mathf.Lerp(Constants.BRUSHSIZEMIN, Constants.BRUSHSIZEMAX, value);

        if (newSize != brush.brushSize)
        {
            brush = new Brush
            {
                brushSize = newSize,
                gui_BrushColor = brush.gui_BrushColor,
                gui_BrushOpacity = brush.gui_BrushOpacity,
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
        var value = Math.DeLerpFromTo(Constants.BRUSHSIZEMIN, Constants.BRUSHSIZEMAX, _context.game.brush.brushSize);
        if (slider.value != value)
            slider.value = value;
    }
}