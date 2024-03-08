namespace ProceduralMeshes
{
    using UnityEngine;

    public class ProceduralCone : ProceduralMesh
    {
        [SerializeField]
        private float _height = 2f;
        [SerializeField]
        private float _radius = 1f;
        [SerializeField, Min(3)]
        private int _resolution = 32;

        public override void Generate()
        {
            Mesh mesh = GetNewMesh();
            _meshFilter.mesh = mesh;

            Vector3[] vertices = new Vector3[_resolution * 2 + 2]; // +2 is for cap vertices.
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[_resolution * 6];

            Vector3 tipVertex = new Vector3(0f, _height);
            Vector3 capVertex = Vector3.zero;
            vertices[^2] = tipVertex;
            vertices[^1] = capVertex;
            uv[^2] = Vector2.up;
            uv[^1] = Vector2.zero;

            for (int i = 0; i < _resolution; ++i)
            {
                float theta = (Mathf.PI * 2f * i) / _resolution;
                Vector3 point = new Vector3(Mathf.Sin(theta), 0f, Mathf.Cos(theta)) * _radius;
                    
                vertices[i * 2] = point;
                vertices[i * 2 + 1] = point;
                uv[i * 2] = new Vector2(i / (float)_resolution, 0f);
                uv[i * 2 + 1] = new Vector2(i / (float)_resolution, 0f);

                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = (i * 2 + 2) % (vertices.Length - 2);
                triangles[i * 6 + 2] = vertices.Length - 2;
                
                triangles[i * 6 + 3] = i * 2 + 1;
                triangles[i * 6 + 4] = vertices.Length - 1;
                triangles[i * 6 + 5] = (i * 2 + 3) % (vertices.Length - 2);
            }
            
            // TODO: Cap UV should not be radial.
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}