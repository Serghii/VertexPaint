using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class BrushPeekUI : MonoBehaviour
{
    private Contexts _context;
    [SerializeField] private Button btn;
    [SerializeField] private Image imageIndicator;
    [SerializeField] private Color indicatorOff = new Color(0.9f, 0.9f, 0.9f, 1f);
    [SerializeField] private Color indicatorOn = new Color(1f, 0.8f, 0.3f, 1f);

    private void Start()
    {
        _context = Contexts.sharedInstance;
        btn.onClick.AddListener(PeekBtnClick);
        _context.game.brushEntity.OnComponentReplaced += BrushEntityReplaced;
        UpdateImageIndicator();
    }

    private void BrushEntityReplaced(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
    {
        UpdateImageIndicator();
    }

    private void PeekBtnClick()
    {
        var brush = _context.game.brush;
        var curPeek = _context.game.brush.brushType == BrushType.draw ? BrushType.peek : BrushType.draw;
        if (brush.brushType != curPeek)
        {
            brush = new Brush
            {
                brushSize = brush.brushSize,
                gui_BrushColor = brush.gui_BrushColor,
                gui_BrushOpacity = brush.gui_BrushOpacity,
                brushType = _context.game.brush.brushType == BrushType.draw ? BrushType.peek : BrushType.draw
            };
            _context.game.ReplaceBrush(brush.gui_BrushColor, brush.brushSize, brush.gui_BrushOpacity, brush.brushType);
        }
    }

    private void UpdateImageIndicator()
    {
        var curColor = _context.game.brush.brushType == BrushType.draw ? indicatorOff : indicatorOn;
        if (imageIndicator.color != curColor)
            imageIndicator.color = curColor;
    }
}