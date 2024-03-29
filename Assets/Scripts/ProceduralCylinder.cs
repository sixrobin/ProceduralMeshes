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
            // TODO: UV.
            
            Vector3[] vertices = new Vector3[_circleResolution * _heightResolution];
            int[] triangles = new int[(_circleResolution * _heightResolution - 1) * 6];

            for (int i = 0, v = 0; i < _heightResolution; ++i)
            {
                Vector3 circleCenter = Vector3.up * (i / (float)(_heightResolution - 1) * _height);

                for (int j = 0; j < _circleResolution; ++j, ++v)
                {
                    float theta = (Mathf.PI * 2f * j) / _circleResolution;
                    Vector3 pointLocalSpace = new Vector3(Mathf.Sin(theta) * _radius, 0f, Mathf.Cos(theta) * _radius);
                    vertices[v] = circleCenter + pointLocalSpace;
                }
            }
            
            for (int t = 0; t < _heightResolution * _circleResolution - _circleResolution; ++t)
            {
                if ((t + 1) % _circleResolution > 0)
                {
                    triangles[t * 6] = t;
                    triangles[t * 6 + 1] = t + 1;
                    triangles[t * 6 + 2] = t + 1 + _circleResolution;
                    triangles[t * 6 + 3] = t;
                    triangles[t * 6 + 4] = t + 1 + _circleResolution;
                    triangles[t * 6 + 5] = t + _circleResolution;
                }
                else
                {
                    triangles[t * 6] = t;
                    triangles[t * 6 + 1] = t + 1 - _circleResolution;
                    triangles[t * 6 + 2] = t + 1;
                    triangles[t * 6 + 3] = t;
                    triangles[t * 6 + 4] = t + 1;
                    triangles[t * 6 + 5] = t + _circleResolution;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }
    }
}