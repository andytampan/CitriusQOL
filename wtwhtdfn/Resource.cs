using System;
using System.Collections.Generic;
using System.Text;

namespace wtwhtdfn
{
    public static class Resource
    {
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

        
    }
}
