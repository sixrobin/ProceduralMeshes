namespace ProceduralMeshes
{
    using UnityEngine;

    public class ProceduralCylinder : ProceduralMesh
    {
        [SerializeField]
        private float _radius = 0.5f;
        [SerializeField]
        private float _height = 3f;
        [SerializeField, Min(2)]
        private int _heightResolution = 8;
        [SerializeField, Min(3)]
        private int _circleResolution = 16;
        
        [ContextMenu("Generate")]
        public override void Generate()
        {
            Mesh mesh = GetNewMesh();
            _meshFilter.mesh = mesh;
            
            // TODO: Triangulate caps.
            
            Vector3[] vertices = new Vector3[_circleResolution * _heightResolution];
            int[] triangles = new int[(_circleResolution * _heightResolution - 1) * 6];

            for (int i = 0, v = 0; i < _heightResolution; ++i)
            {
                Vector3 circleCenter = Vector3.up * (i / (float)(this._heightResolution - 1) * this._height);

                for (int j = 0; j < _circleResolution; ++j, ++v)
                {
                    float theta = (Mathf.PI * 2f * j) / _circleResolution;
                    Vector3 pointLocalSpace = new Vector3(Mathf.Sin(theta) * _radius, 0f, Mathf.Cos(theta) * _radius);
                    vertices[v] = circleCenter + pointLocalSpace;
                }
            }
            
            for (int i = 0, t = 0; i < _heightResolution - 1; ++i)
            {
                int s = i * _circleResolution;
                
                for (int j = 0; j < _circleResolution; ++j, t += 6)
                {
                    // TODO: Fix triangulation not working on circle resolution loop.
                    
                    triangles[t] = s + j;
                    triangles[t + 1] = (s + j + 1) % vertices.Length;
                    triangles[t + 2] = (s + j + _circleResolution) % vertices.Length;
                    triangles[t + 3] = (s + j + 1) % vertices.Length;
                    triangles[t + 4] = (s + j + _circleResolution + 1) % vertices.Length;
                    triangles[t + 5] = (s + j + _circleResolution) % vertices.Length;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}