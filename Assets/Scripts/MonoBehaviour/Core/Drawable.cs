using System;
using Entitas.Unity;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(Renderer))]
    public class Drawable : MonoBehaviour
    {
        private Contexts _context;
        private void Start()
        {
            _context = Contexts.sharedInstance;
            
            var e =_context.game.CreateEntity();
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            var vertices = mesh.vertices;
            var colors = mesh.colors.Length != 0? mesh.colors: new Color[vertices.Length];
            e.AddDrawable(transform,transform.GetInstanceID(), mesh, colors, colors, vertices  );
            gameObject.Link(e);
        }
    }
