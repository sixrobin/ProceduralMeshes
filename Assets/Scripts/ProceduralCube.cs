namespace ProceduralMeshes
{
    using UnityEngine;

    public class ProceduralCube : ProceduralMesh
    {
        [SerializeField]
        private Vector3 _size = Vector3.one;
        [SerializeField]
        private Vector3 _pivotOffset = Vector3.zero;

        [ContextMenu("Generate")]
        public override void Generate()
        {
            Mesh mesh = GetNewMesh();
            _meshFilter.mesh = mesh;
            
            Vector3[] vertices =
            {
                // Front.
                new Vector3(0f, 0f, 0f),
                new Vector3(_size.x, 0f, 0f),
                new Vector3(0f, _size.y, 0f),
                new Vector3(_size.x, _size.y, 0f),
                // Right.
                new Vector3(_size.x, 0f, 0f),
                new Vector3(_size.x, 0f, _size.z),
                new Vector3(_size.x, _size.y, 0f),
                new Vector3(_size.x, _size.y, _size.z),
                // Back.
                new Vector3(_size.x, 0f, _size.z),
                new Vector3(0f, 0f, _size.z),
                new Vector3(_size.x, _size.y, _size.z),
                new Vector3(0f, _size.y, _size.z),
                // Left.
                new Vector3(0f, 0f, _size.z),
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, _size.y, _size.z),
                new Vector3(0f, _size.y, 0f),
                // Top.
                new Vector3(0f, _size.y, 0f),
                new Vector3(_size.x, _size.y, 0f),
                new Vector3(0f, _size.y, _size.z),
                new Vector3(_size.x, _size.y, _size.z),
                // Bottom.
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, 0f, _size.z),
                new Vector3(_size.x, 0f, 0f),
                new Vector3(_size.x, 0f, _size.z),
            };
            
            for (int i = 0; i < vertices.Length; ++i)
                vertices[i] += _pivotOffset - _size * 0.5f;

            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0; i < uv.Length; i += 4)
            {
                uv[i + 1] = Vector2.right;
                uv[i + 2] = Vector2.up;
                uv[i + 3] = Vector2.one;
            };
            
            int[] triangles =
            {
                0, 2, 1,
                1, 2, 3,
                4, 6, 5,
                5, 6, 7,
                8, 10, 9,
                9, 10, 11,
                12, 14, 13,
                13, 14, 15,
                16, 18, 17,
                17, 18, 19,
                20, 22, 21,
                21, 22, 23,
            };
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}