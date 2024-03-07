namespace ProceduralMeshes
{
    using UnityEngine;

    public class ProceduralPlane : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _size = Vector2.one;
        [SerializeField, Min(0.01f)]
        private Vector2 _spacing = new Vector2(0.1f, 0.1f);

        [ContextMenu("Generate")]
        private void Generate()
        {
            UnityEngine.Mesh mesh = new() {name = "ProceduralPlane" };
            this.GetComponent<MeshFilter>().mesh = mesh;

            Vector2Int density = new(Mathf.CeilToInt(_size.x / _spacing.x), Mathf.CeilToInt(_size.y / _spacing.y));
            Vector2 spacing = new(_size.x / density.x, _size.y / density.y);
            
            Vector3[] vertices = new Vector3[(density.x + 1) * (density.y + 1)];
            Vector2[] uv = new Vector2[vertices.Length];
            Vector4[] tangents = new Vector4[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];
            int[] triangles = new int[density.x * density.y * 6];

            for (int i = 0, y = 0; y <= density.y; ++y)
            {
                for (int x = 0; x <= density.x; ++x, ++i)
                {
                    float posX = x * spacing.x - _size.x * 0.5f;
                    float posZ = y * spacing.y - _size.y * 0.5f;
                    
                    vertices[i] = new Vector3(posX, 0f, posZ);
                    uv[i] = new Vector2((float)x / density.x, (float)y / density.y);
                    tangents[i] = new Vector4(0f, 1f, 0f, -1f);
                    normals[i] = Vector3.up;
                }
            }
            
            for (int ti = 0, vi = 0, y = 0; y < density.y; ++y, ++vi)
            {
                for (int x = 0; x < density.x; ++x, ti += 6, ++vi)
                {
                    triangles[ti] = vi;
                    triangles[ti + 1] = vi + density.x + 1;
                    triangles[ti + 2] = vi + 1;
                    triangles[ti + 3] = vi + 1;
                    triangles[ti + 4] = vi + density.x + 1;
                    triangles[ti + 5] = vi + density.x + 2;
                }
            }
            
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.tangents = tangents;
            mesh.triangles = triangles;
            mesh.normals = normals;
        }
    }
}
