using System.Numerics;
using SimulationFramework.Drawing.Shaders;

public class vertshad : VertexShader {
    public struct vsdata {
        public Vector3 vert;
        public Vector2 uv;
    }

    [VertexData]
    vsdata vsdat;

    public Matrix4x4 world;
    public Matrix4x4 view;
    public Matrix4x4 proj;

    [VertexShaderOutput]
    Vector2 vert_uv;

    [UseClipSpace]
    public override Vector4 GetVertexPosition() {
        Vector4 res = new(vsdat.vert, 1);
        res = Vector4.Transform(res, world);
        res = Vector4.Transform(res, view);
        res = Vector4.Transform(res, proj);

        vert_uv = vsdat.uv;

        return res;
    }
}