using SimulationFramework;
using SimulationFramework.Drawing;
using thrustr.utils;

namespace thrustr.basic;

public class intro {
    static ITexture enginetex;
    static ITexture sftex;

    static ISound fadesfx;

    public static bool introplayed = true;

    static byte introstate = 255;

    static float introstart = 0;

    public static void loadintro() {
        enginetex.trydispose();
        sftex.trydispose();
        fadesfx.trydispose();

        enginetex = Graphics.LoadTexture(@"thrustr\assets\sprites\icons\engine logo.png");
        sftex = Graphics.LoadTexture(@"thrustr\assets\sprites\icons\sf logo.png");

        fadesfx = Audio.LoadSound(@"thrustr\assets\audio\introfade.wav");
    }

    public static void playintro() {
        introstate = 1; 
        introplayed = false;
        introstart = Time.TotalTime+1.5f;
    }

    public static void dointro(ICanvas c, font dfont) {
        byte previntro = introstate;
        introstate = (byte)math.floor((Time.TotalTime-introstart)/2f);

        if(previntro!=introstate && introstate < 3) 
            fadesfx.Play();

        float lerp = ease.oqnt(math.abs(((Time.TotalTime-introstart)/2f-introstate>=.5f?1:0)-((Time.TotalTime-introstart)/2f-introstate)));

        switch(introstate) {
            case 0:
                fontie.rendertext(
                    c, dfont,
                    "a game by ???",
                    math.round(Window.Width/2 - fontie.predicttextwidth(dfont, "a game by ???") /2), 
                    math.floor(Window.Height/2 - dfont.charh/2), 
                    ColorF.Lerp(ColorF.Black,ColorF.White,lerp)
                );
                break;
            case 1:
                c.DrawTexture(
                    enginetex,
                    new (
                        Window.Width/2, 
                        Window.Height/2,
                        28,50,
                        Alignment.Center
                    ),
                    ColorF.Lerp(ColorF.Black,ColorF.White,lerp)
                );
                fontie.rendertext(
                    c, dfont,
                    "made with thrustr engine", 
                    math.round(Window.Width/2 - fontie.predicttextwidth(dfont, "made with thrustr engine") /2),
                    math.floor(Window.Height/2 + 49/2f + dfont.charh/2),
                    ColorF.Lerp(ColorF.Black,ColorF.White,lerp)
                );
                fontie.rendertext(
                    c, dfont,
                    "v 0.1.2",
                    math.round(Window.Width/2 - fontie.predicttextwidth(dfont, "v 0.1.2")/2),
                    math.floor(Window.Height/2 + 49/2f + dfont.charh+4),
                    ColorF.Lerp(ColorF.Black, ColorF.White, lerp)
                );
                break;
            case 2:
                c.DrawTexture(
                   sftex,
                   new(
                       Window.Width / 2,
                       Window.Height / 2,
                       48,48,
                       Alignment.Center
                   ),
                   ColorF.Lerp(ColorF.Black, ColorF.White, lerp)
                );
                fontie.rendertext(
                    c, dfont,
                    "powered by simulation framework", 
                    math.round(Window.Width/2 - fontie.predicttextwidth(dfont, "powered by simulation framework") /2),
                    math.floor(Window.Height/2 + 24 + dfont.charh/2), 
                    ColorF.Lerp(ColorF.Black,ColorF.White,lerp)
                );
                break;
            case 3:
                introplayed = true;
                break;
        }
    }
}