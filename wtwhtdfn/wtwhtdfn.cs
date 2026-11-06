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

            Resource.markerSquare = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/marker");
            Resource.markerDebugger = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/marker_debugging");
            Resource.markerSurrender = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/marker_surrender");
            Resource.markerDisposal = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/marker_disposal");

            QuintessentialMod mod = wtwhtdfn.Instance;
            Resource.GlyphOfDebugger = new()
            {
                cost = 0,
                isFullHexCover = true, // Is a glyph
                glowTexture = Assets.textures.select.single_glow, // Shadow/glow
                strokeTexture = Assets.textures.select.single_stroke, // Stroke/outline
                baseTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/icon_debugging"), // Panel icon
                hoverTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/hover_debugging"), // Hovered panel icon
                glyphHexes = new HexIndex[] { new HexIndex(0, 0) },
                permissionCategory = PuzzlePermissions.None,

            };
            QApi.AddPartType(Instance, Resource.GlyphOfDebugger, "debuggerGlyph");
            mod.AddPartTypesRenderer(Resource.DebuggerRender, (Part part) => part.GetType() == Resource.GlyphOfDebugger);

            Resource.GlyphOfSurrender = new()
            {
                cost = 0,
                isFullHexCover = true, // Is a glyph
                glowTexture = Assets.textures.select.single_glow, // Shadow/glow
                strokeTexture = Assets.textures.select.single_stroke, // Stroke/outline
                baseTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/icon_surrender"), // Panel icon
                hoverTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/hover_surrender"), // Hovered panel icon
                glyphHexes = new HexIndex[] { new HexIndex(0, 0) },
                permissionCategory = PuzzlePermissions.None,

            };
            QApi.AddPartType(Instance, Resource.GlyphOfSurrender, "surrenderGlyph");
            mod.AddPartTypesRenderer(Resource.SurrenderRender, (Part part) => part.GetType() == Resource.GlyphOfSurrender);

            Resource.PocketDisposal = new()
            {
                cost = 0,
                isFullHexCover = true, // Is a glyph
                glowTexture = Assets.textures.select.single_glow, // Shadow/glow
                strokeTexture = Assets.textures.select.single_stroke, // Stroke/outline
                baseTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/icon_disposal"), // Panel icon
                hoverTexture = AssetLoaderHelper.LoadTexture("textures/wtwhtdfn/hover_disposal"), // Hovered panel icon
                glyphHexes = new HexIndex[] { new HexIndex(0, 0) },
                permissionCategory = PuzzlePermissions.None,

            };
            QApi.AddPartType(Instance, Resource.PocketDisposal, "pocketDisposal");
            QApi.BindGlyphCycle(Resource.PocketDisposal, new PartCycleDelegate(PartCycleExecutionType.AfterBonder, Resource.PocketDisposalCode));
            QApi.AddRecipe(wtwhtdfn.Instance, new GlyphRecipe(), Resource.PocketDisposal.Id, Resource.PocketDisposal.Id);
            mod.AddPartTypesRenderer(Resource.PocketDisposalRender, (Part part) => part.GetType() == Resource.PocketDisposal);
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
