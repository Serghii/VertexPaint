using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class BrushView : MonoBehaviour
{
    private Contexts _context;

    [SerializeField] private float brushSpeed = 100;
    [SerializeField] private float brushNormal = 60;
    [SerializeField] private Color DrawColor = new Color(0, 103, 255, 39);
    [SerializeField] private Color PeekColor = new Color(219, 0, 255, 29);

    [SerializeField] private Transform brushVisual;
    [SerializeField] private ParticleSystem brushParticle;
    [SerializeField] private ParticleSystem selected;
    [SerializeField] private Transform brushOpasity;

    private IGroup<GameEntity> _mouseHit;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        _context = Contexts.sharedInstance;
        _mouseHit = _context.game.GetGroup(GameMatcher.MouseHit);

        _context.game.brushEntity.OnComponentReplaced += BrushChange;
        _context.game.brushPositionEntity.OnComponentReplaced += BrushPosionChange;

        _context.game.brushEntity.OnDestroyEntity += OnDestroyEntity;
        _context.game.brushPositionEntity.OnDestroyEntity += OnDestroyEntity;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && _mouseHit?.GetEntities().Length > 0 && !selected.gameObject.activeSelf)
            selected.gameObject.SetActive(true);

        if (Input.GetMouseButtonUp(0) || _mouseHit?.GetEntities().Length == 0)
            selected.gameObject.SetActive(false);
    }


    private void BrushPosionChange(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
    {
        if (_context.game.brushPosition.normal == Vector3.zero)
        {
            brushVisual.gameObject.SetActive(false);
            return;
        }

        if (!brushVisual.gameObject.activeSelf)
        {
            transform.position = _context.game.brushPosition.position;
            transform.up = _context.game.brushPosition.normal;
            brushVisual.localPosition = new Vector3(0, 0.03f, 0);
            brushVisual.gameObject.SetActive(true);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, _context.game.brushPosition.position,
            brushSpeed * Time.deltaTime);
        transform.up = Vector3.Lerp(transform.up, _context.game.brushPosition.normal, brushNormal * Time.deltaTime);
    }

    private void BrushChange(IEntity entity, int index, IComponent previouscomponent, IComponent newcomponent)
    {
        UpdateBrushScale();
        UpdateBrushOpacity();
        UpdateBrushType();
    }

    private void OnDestroy()
    {
        OnDestroyEntity(_context.game.brushEntity);
        OnDestroyEntity(_context.game.brushPositionEntity);
    }

    private void OnDestroyEntity(IEntity entity)
    {
        entity.OnComponentReplaced -= BrushChange;
        entity.OnComponentReplaced -= BrushPosionChange;
        entity.OnDestroyEntity -= OnDestroyEntity;
    }

    private void UpdateBrushScale()

    {
        var brushScale = Vector3.one * (_context.game.brush.brushSize + 0.02f);
        brushScale = Vector3.one * (_context.game.brush.brushSize + 0.02f);
        if (brushVisual.localScale != brushScale)
            brushVisual.localScale = brushScale;
    }

    private void UpdateBrushOpacity()
    {
        var opasity = Mathf.InverseLerp(Constants.BRUSHOPACITYMIN, Constants.BRUSHOPACITYMMAX,
            _context.game.brush.gui_BrushOpacity);
        var position = new Vector3(brushOpasity.localPosition.x, brushOpasity.localPosition.y, -opasity);

        if (brushOpasity.localPosition != position)
            brushOpasity.localPosition = position;
    }

    private void UpdateBrushType()
    {
        var main = brushParticle.main;
        var startColor = _context.game.brush.brushType == BrushType.draw ? DrawColor : PeekColor;
        if (main.startColor.color != startColor)
        {
            var sc = main.startColor;
            sc.color = startColor;
            main.startColor = sc;
        }
    }
}