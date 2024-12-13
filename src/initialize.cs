using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;
using System.Numerics;

using thrustr.basic;
using thrustr.utils;

partial class lodus {
    static void Main() {
        Simulation sim = Simulation.Create(init, rend);
        sim.Run();
    }

    static void init() {
        Window.Title = "lodus";
        Simulation.SetFixedResolution(960,540,Color.Black,false,true,false);

        intro.loadintro();
        intro.playintro();

        atlas = Graphics.LoadTexture(@"assets\sprites\voxels\atlas.png");

        fragment_shader.tex = atlas;
        vertex_shader.chunk_size = chunk_size;

        dmask = Graphics.CreateDepthMask(960,540);

        Parallel.For(0, 24, x => {
            for(int y = 0; y < 6; y++)
                for(int z = 0; z < 24; z++)
                    gen_new_chunk(new(x,y,z));
        });

        Mouse.Visible = false;

        cam = new(12*chunk_size,6*chunk_size+4,12*chunk_size);

        //precalculate values
        precalc_fog_scaling_factor = 1f/((render_dist-1)*chunk_size*.5f);
        precalc_max_chunk_dist = (int)math.sqr(chunk_size*render_dist*2);
        precalc_chunk_offset = Vector3.One*-.5f*chunk_size;
        precalc_1div_chunk_size = 1f/chunk_size;
        precalc_half_window_width = 960/2f;
        precalc_half_window_height = 540/2f;
    }

    /* precalculated values */

    static float precalc_fog_scaling_factor;
    static int precalc_max_chunk_dist;
    static Vector3 precalc_chunk_offset;
    static float precalc_1div_chunk_size;
    static float precalc_half_window_width;
    static float precalc_half_window_height;
}