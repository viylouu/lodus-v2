using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

using thrustr.basic;
using thrustr.utils;

partial class lodus {
    static Vector3[] verts;
    static uint[] inds;
    static Vector2[] uvs;

    static vertshad.vsdata[] vsdat;

    static vertshad vertex_shader = new();
    static fragshad fragment_shader = new();

    static ITexture grass;
    static IDepthMask dmask;

    static byte[,,] map = new byte[8,8,8];

    static Vector3 cam;
    static float pitch, yaw, pitchr, yawr;

    static void rend(ICanvas c) {
         c.Clear(Color.Black);
         dmask.Clear(1);

        //if (!intro.introplayed)
        //{ intro.dointro(c, fontie.dfont); return; }

        if(Keyboard.IsKeyDown(Key.W))
            cam += new Vector3(math.cos(pitchr+math.pi/2),0,math.sin(pitchr+math.pi/2))*Time.DeltaTime*2;
        if(Keyboard.IsKeyDown(Key.S))
            cam -= new Vector3(math.cos(pitchr+math.pi/2),0,math.sin(pitchr+math.pi/2))*Time.DeltaTime*2;

        if(Keyboard.IsKeyDown(Key.A))
            cam += new Vector3(math.cos(pitchr),0,math.sin(pitchr))*Time.DeltaTime*2;
        if(Keyboard.IsKeyDown(Key.D))
            cam -= new Vector3(math.cos(pitchr),0,math.sin(pitchr))*Time.DeltaTime*2;

        if(Keyboard.IsKeyDown(Key.Space))
            cam.Y -= Time.DeltaTime*2;
        if(Keyboard.IsKeyDown(Key.LeftShift))
            cam.Y += Time.DeltaTime*2;

        pitch += (Mouse.Position.X-math.round(Window.Width/2f)*2/2f)/8;
        yaw += (Mouse.Position.Y-math.round(Window.Height/2f)*2/2f)/8;

        yaw = math.clamp(yaw,-90,90);

        pitchr = math.rad(pitch);
        yawr = math.rad(yaw);

        Mouse.Position = new Vector2(Window.Width/2,Window.Height/2);

        vertex_shader.view = Matrix4x4.CreateTranslation(cam) * Matrix4x4.CreateRotationY(pitchr) * Matrix4x4.CreateRotationX(yawr);
        vertex_shader.proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, c.Width / (float)c.Height, 0.1f, 100f);

        for(int x = 0; x < 8; x++)
            for(int y = 0; y < 8; y++)
                for(int z = 0; z < 8; z++) {
                    vertex_shader.world = Matrix4x4.CreateScale(.5f) * Matrix4x4.CreateTranslation(new Vector3(x,y,z));

                    c.Fill(fragment_shader, vertex_shader);
                    c.Mask(dmask);
                    c.WriteMask(dmask, null);
                    c.DrawTriangles<vertshad.vsdata>(vsdat, inds);
                }

        fontie.rendertext(c, fontie.dfont, $"{math.round(1/Time.DeltaTime)} fps", 3,3, ColorF.White);
    }
}