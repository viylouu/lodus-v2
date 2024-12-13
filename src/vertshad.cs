using System.Numerics;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

public class vertshad : VertexShader {
    [VertexData]
    vsdata vsdat;

    public int chunk_size;

    public Matrix4x4 world;
    public Matrix4x4 view;
    public Matrix4x4 proj;

    public Vector3 chunk_pos;

    public float time;

    [VertexShaderOutput]
    Vector2 vert_uv;

    [VertexShaderOutput]
    Vector3 vert_pos;

    [UseClipSpace]
    public override Vector4 GetVertexPosition() {
        Vector4 res = new(vsdat.vert, 1);
        res = Vector4.Transform(res, world);
        res = Vector4.Transform(res, view);
        res = Vector4.Transform(res, proj);

        vert_uv = vsdat.uv;
        vert_pos = vsdat.vert + chunk_pos*chunk_size;

        return res;
    }
}