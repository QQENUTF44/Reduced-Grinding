using Terraria;
using Terraria.World.Generation;
using Terraria.ObjectData;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.ID;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Events;
using Terraria.GameContent.Achievements;
using Terraria.Enums;
using Terraria.DataStructures;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ReducedGrinding.Tiles
{
	public class Moon_Ball : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = false;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(1, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
			TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Moon Ball");
			AddMapEntry(new Color(191, 191, 255), name);
			dustType = mod.DustType("Sparkle");
			disableSmartCursor = true;
			dresser = "Moon Ball";
			Main.tileLighted[Type] = true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("Moon_Ball"));
		}

		public override bool HasSmartInteract()
		{
			return true;
		}

		public override void RightClick(int i, int j)
		{
			Main.moonPhase++;
			if (Main.moonPhase >= 8)
			{
				Main.moonPhase = 0;
			}
			Main.PlaySound(SoundID.Item4); //Crystal Ball
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.showItemIcon = true;
			player.showItemIcon2 = mod.ItemType("Moon_Ball");
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			float lightStrength = Main.rand.NextFloat(0.125f, 0.25f);
			r = lightStrength;
			g = lightStrength;
			b = lightStrength;
		}
	}
}
