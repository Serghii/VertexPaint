using System.Linq;
using Entitas;
using UnityEngine;

public class UserInputSystem : IExecuteSystem
{
    private readonly Contexts _contexts;
    private IGroup<GameEntity> _drawable;
    private IGroup<GameEntity> BrashHistory;

    private RaycastHit hit;
    private Ray _ray;
    private Camera _camera;
    private int steps;

    public UserInputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _drawable = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Drawable).NoneOf(GameMatcher.Destroy));
        BrashHistory = _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.HistoryBrash).NoneOf(GameMatcher.Destroy));
        _camera = Camera.main;
        steps = 0;
    }

    public void Execute()
    {
        CastRay();
        
        if (Input.GetMouseButtonUp(0))
        {
            UpdateHistory();
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            CreateUndoEntity();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0f && Input.GetKey(KeyCode.LeftControl))
        {
            UpdateBrushSize();
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0f && Input.GetKey(KeyCode.LeftAlt))
        {
            UpdateBrushOpasity();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetBrushTypeTo(BrushType.peek);
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SetBrushTypeTo(BrushType.draw);
        }
    }

    private void SetBrushTypeTo(BrushType type)
    {
        var brush = _contexts.game.brush ;
        brush = new Brush
        {
            brushSize = brush.brushSize,
            gui_BrushColor = brush.gui_BrushColor,
            gui_BrushOpacity = brush.gui_BrushOpacity,
            brushType = type
        };
        _contexts.game.ReplaceBrush(brush.gui_BrushColor,brush.brushSize,brush.gui_BrushOpacity, brush.brushType);
    }

    private void UpdateBrushOpasity()
    {
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel") / 10f ;
        var brush = _contexts.game.brush ;
        float newOpasity = brush.gui_BrushOpacity+ scrollAmount;
        newOpasity = Mathf.Clamp(newOpasity, Constants.BRUSHOPACITYMIN, Constants.BRUSHOPACITYMMAX);
            
        brush = new Brush
        {
            brushSize = brush.brushSize,
            gui_BrushColor = brush.gui_BrushColor,
            gui_BrushOpacity = newOpasity,
            brushType = brush.brushType
        };
        _contexts.game.ReplaceBrush(brush.gui_BrushColor,brush.brushSize,brush.gui_BrushOpacity,brush.brushType);
    }

    private void UpdateBrushSize()
    {
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel") / 3f ;
        var brush = _contexts.game.brush ;
        float newSize = brush.brushSize + scrollAmount;
        newSize = Mathf.Clamp(newSize, Constants.BRUSHSIZEMIN, Constants.BRUSHSIZEMAX);
        brush = new Brush
        {
            brushSize = newSize,
            gui_BrushColor = brush.gui_BrushColor,
            gui_BrushOpacity = brush.gui_BrushOpacity,
            brushType = brush.brushType
        };
        _contexts.game.ReplaceBrush(brush.gui_BrushColor,brush.brushSize,brush.gui_BrushOpacity, brush.brushType);
    }

    private void CreateUndoEntity()
    {
        var undo = _contexts.game.CreateEntity();
        undo.isUndo = true;
        undo.isDestroy = true;
    }

    private void UpdateHistory()
    {
        GameEntity[] historyEntitys = BrashHistory.GetEntities();
        if (historyEntitys.Length == 0)
            return;
            
        HistoryBrash[] historyBrash = new HistoryBrash[historyEntitys.Length];

        for (int i = 0; i < historyBrash.Length; i++)
        {
            historyBrash[i] = historyEntitys[i].historyBrash.value;
            historyEntitys[i].isDestroy = true;
        }
        _contexts.game.historyStrokes.value.Push(historyBrash);
    }

    private void CastRay()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out hit, int.MaxValue))
        {
            var e = _drawable.GetEntities().FirstOrDefault(i => i.drawable.transform == hit.transform);
            if (e != null)
            {
                if (Input.GetMouseButtonDown(0) )
                {
                    int step = _contexts.game.strokeSteps.value.count;
                    step++;
                    _contexts.game.ReplaceStrokeSteps(new StrokeSteps {count = step});
                }
                _contexts.game.ReplaceBrushPosition(hit.point, hit.normal);
                var hitPos = Vector3.Scale(hit.transform.InverseTransformPoint(hit.point), hit.transform.localScale);
                e.ReplaceMouseHit(hitPos);
            }
            else
                RemoveBrushHit();
        }
        else
            RemoveBrushHit();
    }

    private void RemoveBrushHit()
    {
        foreach (var e in _drawable)
        {
            if (e.hasMouseHit)
                e.RemoveMouseHit();
        }

        if (_contexts.game.brushPosition.position != Vector3.zero ||
            _contexts.game.brushPosition.normal != Vector3.zero)
            _contexts.game.ReplaceBrushPosition(Vector3.zero, Vector3.zero);
    }
}