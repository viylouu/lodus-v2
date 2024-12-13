using SimulationFramework.Input;
using SimulationFramework;
using System.Numerics;

using thrustr.utils;

partial class lodus {
    static void camera() {
        if(inui)
            return;

        float center_x = math.round(precalc_half_window_width),
              center_y = math.round(precalc_half_window_height);

        pitch += (math.round(Mouse.Position.X) - center_x) * .125f;
        yaw += (math.round(Mouse.Position.Y) - center_y) * .125f;

        yaw = math.clamp(yaw, -90, 90);

        pitchs += ease.dyn(pitchs, pitch, 6);
        yaws += ease.dyn(yaws, yaw, 6);

        pitchr = math.rad(pitchs);
        yawr = math.rad(yaws);

        Mouse.Position = new(center_x, center_y);
    }

    static void movement() {
        if(inui)
            return;

        float speed = 16;

        float cos_pitchr = math.cos(pitchr);
        float sin_pitchr = math.sin(pitchr);

        if (Keyboard.IsKeyDown(Key.W))
            cam -= new Vector3(math.cos(pitchr + math.hpi), 0, math.sin(pitchr + math.hpi)) * Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.S))
            cam += new Vector3(math.cos(pitchr + math.hpi), 0, math.sin(pitchr + math.hpi)) * Time.DeltaTime * speed;

        if (Keyboard.IsKeyDown(Key.A))
            cam -= new Vector3(cos_pitchr, 0, sin_pitchr) * Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.D))
            cam += new Vector3(cos_pitchr, 0, sin_pitchr) * Time.DeltaTime * speed;

        if (Keyboard.IsKeyDown(Key.Space))
            cam.Y += Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.LeftShift))
            cam.Y -= Time.DeltaTime * speed;
    }
}