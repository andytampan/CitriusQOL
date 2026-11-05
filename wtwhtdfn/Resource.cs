using Quintessential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtwhtdfn
{
    public static class Resource
    {

        public static bool debuggerGlyphActive;
        public static bool HexSide(HexIndex a, HexIndex b)
        {
            int x = a.Q - b.Q;
            int y = a.R - b.R;
            return Math.Abs(x) <= 1 && Math.Abs(y) <= 1 && x != y;
        }

        public static Texture speed_button = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/speed_button");

        public static void RenderCenter(Texture texture, Vector2 position)
        {
            TextureRenderer.Render(texture, position - texture.GetHalfSize());
        }

        public static PartType GlyphOfDebugger;
        public static PartType GlyphOfSurrender;
        internal static PartType[] Glyph =
        {
            GlyphOfDebugger, GlyphOfSurrender
        };

        public static void DebuggerRender(Part part, Vector2 position, SolutionEditorBase editor, PartRenderer partRenderer)
        {
            Texture marker_base = Assets.textures.parts.marker_base;
            Vector2 vector14 = (marker_base.size.ToVector2() / 2).Rounded() + new Vector2(0f, 1f);
            partRenderer.RenderBase(marker_base, vector14);
            partRenderer.RenderRotating(Assets.textures.parts.marker_lighting, new HexIndex(0, 0), Vector2.Zero);
            // partRenderer.RenderBase(Assets.textures.parts.marker_details, vector14); //find a good icon to use for this glyph

            
            ISim Isim = editor.GetSimulation();


            foreach (Molecule m in Isim.GetMolecules())
            {
                if (m.GetAtoms().TryGetValue(part.InFrontBy(new HexIndex(0,0)), out Atom _))
                {
                    var sim = (patch_Sim)Isim;
                    patch_SolutionEditorScreen_DebuggerGlyph SEC = (patch_SolutionEditorScreen_DebuggerGlyph)sim.solutionEditor;
                    if (SEC.simulationState == SimPlayState.Playing)
                    {
                        SEC.ToSteppingTransition();
                    }
                }
            }
            
        }
        internal static void debuggingGlyphCode(Sim sim, CycleEvent.CycleEventExecutionType executionType)
        {
            foreach (Part part in sim.simulationDict.Keys)
            {
                if (part.GetType() == GlyphOfDebugger)
                {

                    var reference = sim.FindAtomRelative(part, new HexIndex(0, 0));
                    if (reference.HasValue())
                    {
                        debuggerGlyphActive = false;
                        patch_SolutionEditorScreen_DebuggerGlyph sec = (patch_SolutionEditorScreen_DebuggerGlyph)sim.solutionEditor;
                        sec.ToSteppingTransition();

                    }
                }
            }


        }

        internal static void SurrenderRender(Part part, Vector2 position, SolutionEditorBase editor, PartRenderer partRenderer)
        {
            Texture marker_base = Assets.textures.parts.marker_base;
            Vector2 vector14 = (marker_base.size.ToVector2() / 2).Rounded() + new Vector2(0f, 1f);
            partRenderer.RenderBase(marker_base, vector14);
            partRenderer.RenderRotating(Assets.textures.parts.marker_lighting, new HexIndex(0, 0), Vector2.Zero);
        }
    }
}
