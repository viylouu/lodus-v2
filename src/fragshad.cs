using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

public class fragshad : CanvasShader {
    [VertexShaderOutput]
    Vector2 vert_uv;

    [VertexShaderOutput]
    Vector3 vert_pos;

    public Vector3 cam;

    public ITexture tex;

    public override ColorF GetPixelColor(Vector2 pos) {
        float depth = 1-Distance(vert_pos, -cam) / 128f;

        depth += .65f;

        if(depth <= 0)
            Discard();

        ColorF x = tex.SampleUV(vert_uv);
        if(x.A < .1f)
            Discard();

        depth = Clamp(depth, 0, 1);

        return new ColorF(
            Lerp(100/255f, x.R, depth),
            Lerp(149/255f, x.G, depth),
            Lerp(237/255f, x.B, depth)
        );
    }
}