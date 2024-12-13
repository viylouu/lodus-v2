using System.Numerics;
using thrustr.utils;

partial class lodus {
    static byte chunk_size = 16;

    /*static void init_chunk(Vector3 position) {
        chunks.Add(new() { data = new byte[chunk_size,chunk_size,chunk_size], pos = position });
    }*/

    static int init_chunk_out(Vector3 position) {
        chunks.Add(new() { data = new byte[chunk_size,chunk_size,chunk_size], pos = position });
        return chunks.Count-1;
    }

    static void gen_new_chunk(Vector3 pos) {
        int cur_chunk = init_chunk_out(pos);

        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++) {
                    chunks[cur_chunk].data[x,y,z] = 0x00;
                }

        int i = 0;

        List<uint> inds = new();
        List<vsdata> vsdat = new();

        for (int x = 0; x < chunk_size; x++)
            for (int y = 0; y < chunk_size; y++)
                for (int z = 0; z < chunk_size; z++)
                    if (chunks[cur_chunk].data[x, y, z] != 0xFF) {
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
                                chunks[cur_chunk].data[nx, ny, nz] == 0xFF) {
                                
                                // add the verts for the face
                                for (int a = 0; a < 4; a++) {
                                    vsdat.Add(new() {
                                        vert = face_verts[face, a] + new Vector3(x, y, z),

                                        uv = (
                                            face_uvs[face, a] +
                                            new Vector2(
                                                math.floor(chunks[cur_chunk].data[x, y, z] / 16f),
                                                chunks[cur_chunk].data[x, y, z] % 16
                                            )
                                        ) / 16f
                                    });
                                }

                                // add the inds
                                for (int a = 0; a < 6; a++)
                                    inds.Add((uint)(face_inds[face, a] + i));
                                
                                i += 4;
                            }
                        }
                    }

        chunks[cur_chunk].mesh_inds = inds.ToArray();
        chunks[cur_chunk].mesh_data = vsdat.ToArray();
    }

    /*static void gen_existing_chunk(int chunk_id) {
        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++) {
                    chunks[chunk_id].data[x,y,z] = 1;
                }
    }*/
}