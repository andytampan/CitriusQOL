using Quintessential;
using Quintessential.Settings;
using System;
using System.Diagnostics;
using static Quintessential.PartCycleDelegate;

namespace wtwhtdfn
{
    internal class wtwhtdfn : QuintessentialMod, IDataMod
    {
        public static wtwhtdfn Instance { get; }

        
        public override string ModId => "wtwhtdfn";

        public override void Load() {}

        public override void LoadContent()
        {

            Logger.Log("am i sane load");

            QuintessentialMod mod = wtwhtdfn.Instance;
            string id = "debugger";
            Resource.GlyphOfDebugger = new()
            {
                cost = 30,
                isFullHexCover = true, // Is a glyph
                glowTexture = Assets.textures.select.single_glow, // Shadow/glow
                strokeTexture = Assets.textures.select.single_stroke, // Stroke/outline
                baseTexture = Assets.textures.parts.icons.marker, // Panel icon
                hoverTexture = Assets.textures.parts.icons.marker_hover, // Hovered panel icon
                glyphHexes = new HexIndex[]
            {
                new HexIndex(0, 0),

            },
                permissionCategory = PuzzlePermissions.None,

            };
            QApi.AddPartType(Instance, Resource.GlyphOfDebugger, "debuggerGlyph");
            mod.AddPartTypesRenderer(Resource.DebuggerRender, (Part part) => part.GetType() == Resource.GlyphOfDebugger);
            QApi.AddCycleEvent(new(CycleEvent.CycleEventExecutionType.AfterInstructions, Resource.debuggingGlyphCode));
        }



        public override void LoadCompatContent() { }
        public override void FinaliseContent() { }

        public override void PostLoad() { }
        public override void Unload() { }

        public override Type SettingsType => typeof(MQSettings);

        public class MQSettings
        {
            public static MQSettings Instance => wtwhtdfn.Instance.Settings as MQSettings;
            [SettingsLabelKey("wtwhtdfn.settings.enableOverlap")]
            public bool enableOverlap = false;

            [SettingsLabelKey("wtwhtdfn.settings.disableUnique")]
            public bool disableUnique = false;

            [SettingsLabelKey("wtwhtdfn.settings.keybinds.disjointSplit")]
            public Keybinding disjointSplit = new ("\\");

            [SettingsLabelKey("wtwhtdfn.settings.keybinds.regularSplit")]
            public Keybinding regularSplit = new("/");

            [SettingsLabelKey("wtwhtdfn.settings.keybinds.reverseTrack")]
            public Keybinding mirrorTrack = new(";");
            [SettingsLabelKey("wtwhtdfn.settings.keybinds.mergeTrack")]
            public Keybinding mergeTrack = new("'");
        }



    }
}
