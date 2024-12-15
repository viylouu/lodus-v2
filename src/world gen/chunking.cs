using System.Numerics;
using SimulationFramework.Drawing.Shaders.Compiler.Expressions;
using thrustr.utils;

partial class lodus {
    static byte chunk_size = 16;

    static FastNoiseLite fnl;
    static Random r = new();

    static int asyncs = 0;
    static int maxasyncs = 256;

    static void init_chunk_gen() {
        fnl = new();
        fnl.SetSeed(r.Next(int.MinValue, int.MaxValue));
        fnl.SetFractalType(FastNoiseLite.FractalType.FBm);
        fnl.SetFrequency(.04f);
    }

    static async void gen_new_chunk(Vector3Int pos) {
        lock(chunks) { 
            if(chunks.ContainsKey(pos)) {
                Console.WriteLine($"there is already a chunk at ({pos.X}, {pos.Y}, {pos.Z})");
                return;
            }

            Console.WriteLine($"generating a chunk at ({pos.X}, {pos.Y}, {pos.Z})");
            chunks.Add(pos, new() { genning = true });
        } 

        chunk c = new() { data = new byte[chunk_size,chunk_size,chunk_size] };

        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++) {
                    c.data[x,y,z] = 0xFF;

                    if(fnl.GetNoise(x+pos.X*chunk_size,y+pos.Y*chunk_size,z+pos.Z*chunk_size) >= 0)
                        c.data[x,y,z] = 0x20;

                    asyncs++;

                    if(asyncs >= maxasyncs) {
                        asyncs = 0;
                        await Task.Delay(1);
                    }
                }

        int i = 0;

        List<uint> inds = new();
        List<vsdata> vsdat = new();

        for (int x = 0; x < chunk_size; x++)
            for (int y = 0; y < chunk_size; y++)
                for (int z = 0; z < chunk_size; z++)
                    if (c.data[x, y, z] != 0xFF) {
                        // a little complicated so heres some comments

                        //go through each face
                        for (int face = 0; face < 6; face++) {
                            Vector3 neighborOffset = face_normals[face]; // get the normals at the face
                            int nx = x + (int)neighborOffset.X;
                            int ny = y + (int)neighborOffset.Y;
                            int nz = z + (int)neighborOffset.Z;

                            // check if the neighbor is "oob" or air, if so, add the face
                            if (nx < 0 || nx >= chunk_size ||
                                ny < 0 || ny >= chunk_size ||
                                nz < 0 || nz >= chunk_size ||
                                c.data[nx, ny, nz] == 0xFF) {
                                
                                // add the verts for the face
                                for (int a = 0; a < 4; a++) {
                                    vsdat.Add(new() {
                                        vert = face_verts[face, a] + new Vector3(x, y, z),

                                        uv = (
                                            face_uvs[face, a] +
                                            new Vector2(
                                                math.floor(c.data[x, y, z] / 16f),
                                                c.data[x, y, z] % 16
                                            )
                                        ) / 16f
                                    });
                                }

                                // add the inds
                                for (int a = 0; a < 6; a++)
                                    inds.Add((uint)(face_inds[face, a] + i));
                                
                                i += 4;

                                asyncs++;

                                if(asyncs >= maxasyncs) {
                                    asyncs = 0;
                                    await Task.Delay(1);
                                }
                            }
                        }
                    }

        c.mesh_inds = inds.ToArray();
        c.mesh_data = vsdat.ToArray();

        c.genning = false;

        lock(chunks)
            chunks[pos] = c;
    }
}