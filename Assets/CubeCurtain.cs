using UnityEngine;
using System.Collections;
using Reaktion;

[ExecuteInEditMode]
public class CubeCurtain : MonoBehaviour
{
    [SerializeField]
    int _columnCount = 10;

    [SerializeField]
    int _rowCount = 10;

    [Space(10)]
    [SerializeField]
    Material _material;

    Mesh _mesh;
    MaterialPropertyBlock _materialOption;

    XXHash _hashSelect = new XXHash(4384);

    void Update()
    {
        if (_mesh == null)
            _mesh = BuildMesh();

        if (_materialOption == null)
            _materialOption  = new MaterialPropertyBlock();

        for (var ri = 0; ri < _rowCount; ri++)
        {
            for (var ci = 0; ci < _columnCount; ci++)
            {
                var x = ci - 0.5f * (_columnCount - 1);
                var y = ri - 0.5f * (_rowCount - 1);

                //var z = Perlin.Noise(new Vector2(x + Time.time * 1.5f, y) * 0.37f);
                var z = Perlin.Noise(new Vector3(x, y, Time.time * 0.9f) * 0.57f);

                var p = new Vector3(x, y, 0);
                var matrix = Matrix4x4.TRS(p, Quaternion.identity, new Vector3(1, 1, (1.0f + z) * 2));

                matrix = transform.localToWorldMatrix * matrix;

            var select = _hashSelect.Range(0, 16, ci + ri * _columnCount);
            var sv = new Vector2(
                (select / 4) * 0.25f,
                (select % 4) * 0.25f
            );
            _materialOption.SetVector("_select",
                z > 0.5f ? sv : -Vector2.one);

            _materialOption.SetFloat("_emission", Mathf.Clamp01( (z - 0.5f) * 4));

                Graphics.DrawMesh(
                    _mesh, matrix, _material,
                    0, null, 0, _materialOption);
            }
        }
    }

    Mesh BuildMesh()
    {
        Vector3[] vertices =
        {
            new Vector3(-0.5f, +0.5f, -1),
            new Vector3(+0.5f, +0.5f, -1),
            new Vector3(-0.5f, -0.5f, -1),
            new Vector3(+0.5f, -0.5f, -1),

            new Vector3(+0.5f, +0.5f, 0),
            new Vector3(+0.5f, +0.5f, -1),
            new Vector3(-0.5f, +0.5f, 0),
            new Vector3(-0.5f, +0.5f, -1),

            new Vector3(+0.5f, -0.5f, -1),
            new Vector3(+0.5f, -0.5f, 0),
            new Vector3(-0.5f, -0.5f, -1),
            new Vector3(-0.5f, -0.5f, 0),

            new Vector3(+0.5f, +0.5f, 0),
            new Vector3(+0.5f, -0.5f, 0),
            new Vector3(+0.5f, +0.5f, -1),
            new Vector3(+0.5f, -0.5f, -1),

            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(-0.5f, +0.5f, 0),
            new Vector3(-0.5f, -0.5f, -1),
            new Vector3(-0.5f, +0.5f, -1)
        };

        Vector2[] uvs =
        {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),

            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),

            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),

            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),

            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(-1, -1)
        };

        int[] indices = new int[vertices.Length / 4 * 6];

        var vi = 0;
        for (var ii = 0; ii < indices.Length;)
        {
            indices[ii++] = vi;
            indices[ii++] = vi + 1;
            indices[ii++] = vi + 2;
            indices[ii++] = vi + 1;
            indices[ii++] = vi + 3;
            indices[ii++] = vi + 2;
            vi += 4;
        }

        var mesh = new Mesh();
        mesh.hideFlags = HideFlags.DontSave;
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
