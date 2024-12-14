using System.ComponentModel.Design;
using System.Numerics;
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

using thrustr.basic;
using thrustr.utils;

partial class lodus {
    /* chunk data */

    static Dictionary<Vector3, chunk> chunks = new();

    /* shaders */

    static vertshad vertex_shader = new();
    static fragshad fragment_shader = new();

    /* textures */

    static ITexture atlas;

    /* rendering variables */

    static IDepthMask dmask;

    /* camera variables */

    static Vector3 cam;
    static float pitch, yaw, pitchr, yawr, pitchs, yaws;

    /* misc variables */

    static bool fullscreen;

    static bool inui;

    static int render_dist = 4;

    static int ax, ay, az, bx, by, bz;

    static void rend(ICanvas c) {
        c.Clear(Color.CornflowerBlue);
        dmask.Clear(1);

        // if (!intro.introplayed)
        // { intro.dointro(c, fontie.dfont); return; }

        movement();
        camera();

        misc_keybinds();

        pausemenu();

        vertex_shader.view = Matrix4x4.CreateTranslation(-cam) * Matrix4x4.CreateRotationY(pitchr) * Matrix4x4.CreateRotationX(yawr);
        vertex_shader.proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, c.Width / (float)c.Height, 0.1f, 1024f);

        vertex_shader.time = Time.TotalTime;

        fragment_shader.cam = cam;

        fragment_shader.time = Time.TotalTime;

        fragment_shader.rend_dist = render_dist;
        fragment_shader.chunk_size = chunk_size;

        fragment_shader.fog_scaling_factor = precalc_fog_scaling_factor;

        c.Fill(fragment_shader, vertex_shader);
        c.Mask(dmask);
        c.WriteMask(dmask, null);

        Vector3 cams = cam * precalc_1div_chunk_size;

        ax = (int)(cams.X-render_dist);
        bx = (int)(cams.X+render_dist);
        ay = (int)(cams.Y-render_dist);
        by = (int)(cams.Y+render_dist);
        az = (int)(cams.Z-render_dist);
        bz = (int)(cams.Z+render_dist);

        for(int x = ax; x < bx; x++)
            for(int y = ay; y < by; y++)
                for(int z = az; z < bz; z++) {
                    Vector3 pos = new(x,y,z);

                    chunks.TryGetValue(pos, out chunk? cur);

                    if(cur == null) {
                        gen_new_chunk(pos);

                        continue;
                    }

                    if(cur.genning)
                        continue;

                    if(math.sqrdist(cam, pos * chunk_size) < precalc_max_chunk_dist) {
                        vertex_shader.world = Matrix4x4.CreateTranslation(pos * chunk_size + precalc_chunk_offset);
                        vertex_shader.chunk_pos = pos;

                        c.DrawTriangles<vsdata>(cur.mesh_data, cur.mesh_inds);
                    }
                }

        c.ResetState();

        fontie.rendertext(c, fontie.dfont, $"{math.round(1f / Time.DeltaTime)} fps", 3, 3, ColorF.White);
        fontie.rendertext(c, fontie.dfont, $"{chunks.Count} chunks", 3, 4+fontie.dfont.charh, ColorF.White);
    }

    static void misc_keybinds() {
        if (Keyboard.IsKeyPressed(Key.F11)) {
            fullscreen = !fullscreen;

            if (fullscreen)
                Window.EnterFullscreen();
            else
                Window.ExitFullscreen();
        }

        if (Keyboard.IsKeyPressed(Key.Escape))
            Environment.Exit(0);

        if(Keyboard.IsKeyPressed(Key.Tab))
            inui = !inui;
    }

    static void pausemenu() {
        if(inui) {
            Mouse.Visible = true;

            ImGui.Begin("pausemenu");

            int p_rend_dist = render_dist;
            ImGui.InputInt("render distance", ref render_dist);

            if(render_dist != p_rend_dist)
                calc_precalcs();

            ImGui.End();
        }
        else
            Mouse.Visible = false;
    }
} 