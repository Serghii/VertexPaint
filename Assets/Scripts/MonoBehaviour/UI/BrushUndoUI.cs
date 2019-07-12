using Entitas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrushUndoUI : MonoBehaviour
{
    private Contexts _context;
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI UndoText;

    private void Start()
    {
        _context = Contexts.sharedInstance;
        btn.onClick.AddListener(UndoBtnClick);
        _context.game.strokeStepsEntity.OnComponentReplaced += StrokeStepsReplaced;
        UpdatestrokeSteps();
    }

    private void StrokeStepsReplaced(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
    {
        UpdatestrokeSteps();
    }

    private void UndoBtnClick()
    {
        var undo = _context.game.CreateEntity();
        undo.isUndo = true;
        undo.isDestroy = true;
    }

    private void UpdatestrokeSteps()
    {
        int count = _context.game.strokeSteps.value.count;
        UndoText.text = count.ToString();
    }
}