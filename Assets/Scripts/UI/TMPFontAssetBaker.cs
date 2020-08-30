#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;

using TMPro;
using TMPro.EditorUtilities;

namespace DTLocalization.Internal {
	public enum TMPFontPackingModes { Fast = 0, Optimum = 4 };

	public static class TMPFontAssetBaker {
		// PRAGMA MARK - Public Interface
		public static void Bake(Font font, bool useAutoSizing, int fontSize, int characterPadding, TMPFontPackingModes fontPackingMode, int atlasWidth, int atlasHeight, FaceStyles fontStyle, int fontStyleMod, RenderModes fontRenderMode, string charactersToBake, string outputFilePath, TMPFont.GlyphOverride[] glyphOverrides) {
			int errorCode = TMPro_FontPlugin.Initialize_FontEngine();
			if (errorCode != 0 && errorCode != 99) { // 99 means that engine was already initialized
				Debug.LogWarning("Error Code: " + errorCode + "  occurred while initializing TMPro_FontPlugin.");
				return;
			}

			string fontPath = AssetDatabase.GetAssetPath(font);
			errorCode = TMPro_FontPlugin.Load_TrueType_Font(fontPath);
			if (errorCode != 0 && errorCode != 99) { // 99 means that font was already loaded
				Debug.LogWarning("Error Code: " + errorCode + "  occurred while loading font: " + font + ".");
				return;
			}

			if (useAutoSizing) {
				fontSize = 72;
			}
			errorCode = TMPro_FontPlugin.FT_Size_Font(fontSize);
			if (errorCode != 0) {
				Debug.LogWarning("Error Code: " + errorCode + "  occurred while sizing font: " + font + " to size: " + fontSize + ".");
				return;
			}

			byte[] textureBuffer = new byte[atlasWidth * atlasHeight];

			int[] characterArray = charactersToBake.Select(c => (int)c).ToArray();
			int characterCount = charactersToBake.Length;

			var fontFaceInfo = new FT_FaceInfo();
			var fontGlyphInfo = new FT_GlyphInfo[characterCount];

			float strokeSize = fontStyleMod;
			if (fontRenderMode == RenderModes.DistanceField16) {
				strokeSize *= 16;
			} else if (fontRenderMode == RenderModes.DistanceField32) {
				strokeSize *= 32;
			}

			errorCode = TMPro_FontPlugin.Render_Characters(textureBuffer, atlasWidth, atlasHeight, characterPadding, characterArray, characterCount, fontStyle, strokeSize, useAutoSizing, fontRenderMode, (int)fontPackingMode, ref fontFaceInfo, fontGlyphInfo);
			if (errorCode != 0) {
				Debug.LogWarning("Error Code: " + errorCode + "  occurred while rendering font characters!");
				return;
			}

			string outputFilename = Path.GetFileNameWithoutExtension(outputFilePath);

			// check if font asset already exists
			TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath(outputFilePath, typeof(TMP_FontAsset)) as TMP_FontAsset;
			if (fontAsset == null) {
				fontAsset = ScriptableObject.CreateInstance<TMP_FontAsset>(); // Create new TextMeshPro Font Asset.
				AssetDatabase.CreateAsset(fontAsset, outputFilePath);
			}

			// Destroy Assets that will be replaced.
			UnityEngine.Object.DestroyImmediate(fontAsset.atlas, allowDestroyingAssets: true);

			fontAsset.fontAssetType = (fontRenderMode >= RenderModes.DistanceField16) ? TMP_FontAsset.FontAssetTypes.SDF : TMP_FontAsset.FontAssetTypes.Bitmap;
            
			fontAsset.AddFaceInfo(ConvertToFaceInfo(fontFaceInfo));

            // Apply glyph overrides
            var glyphs = ConvertToGlyphs(fontGlyphInfo);
            foreach (var glyphOverride in glyphOverrides)
            {
                var glyph = glyphs.FirstOrDefault(a => a.id == glyphOverride.id);
                glyph.width = glyphOverride.W;
                glyph.height = glyphOverride.H;
                glyph.xOffset = glyphOverride.OX;
                glyph.yOffset = glyphOverride.OY;
                glyph.xAdvance = glyphOverride.ADV;
                glyph.scale = glyphOverride.SF;
            }
            fontAsset.AddGlyphInfo(glyphs);

            var fontTexture = CreateFontTexture(atlasWidth, atlasHeight, textureBuffer, fontRenderMode);
			fontTexture.name = outputFilename + " Atlas";
			fontTexture.hideFlags = HideFlags.HideInHierarchy;

			fontAsset.atlas = fontTexture;
			AssetDatabase.AddObjectToAsset(fontTexture, fontAsset);

			// Find all Materials referencing this font atlas.
			Material[] materialReferences = TMP_EditorUtility.FindMaterialReferences(fontAsset).Where(m => m != null).ToArray();
			if (materialReferences == null || materialReferences.Length <= 0) {
				// Create new Material and add it as Sub-Asset
				Shader shader = Shader.Find("TextMeshPro/Distance Field");
				Material fontMaterial = new Material(shader);
				fontMaterial.name = outputFilename + " Material";

				fontAsset.material = fontMaterial;
				fontMaterial.hideFlags = HideFlags.HideInHierarchy;
				AssetDatabase.AddObjectToAsset(fontMaterial, fontAsset);

				materialReferences = new Material[] { fontMaterial };
			}

			foreach (var m in materialReferences) {
				m.SetTexture(ShaderUtilities.ID_MainTex, fontTexture);
				m.SetFloat(ShaderUtilities.ID_TextureWidth, fontTexture.width);
				m.SetFloat(ShaderUtilities.ID_TextureHeight, fontTexture.height);


				m.SetFloat(ShaderUtilities.ID_WeightNormal, fontAsset.normalStyle);
				m.SetFloat(ShaderUtilities.ID_WeightBold, fontAsset.boldStyle);

				m.SetFloat(ShaderUtilities.ID_GradientScale, characterPadding + 1);
			}

			AssetDatabase.SaveAssets();
			// Re-import font asset to get the new updated version.
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(fontAsset));
			fontAsset.ReadFontDefinition();
			AssetDatabase.Refresh();

			// NEED TO GENERATE AN EVENT TO FORCE A REDRAW OF ANY TEXTMESHPRO INSTANCES THAT MIGHT BE USING THIS FONT ASSET
			TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(true, fontAsset);
		}

		private static Texture2D CreateFontTexture(int atlasWidth, int altasHeight, byte[] textureBuffer, RenderModes fontRenderMode) {
			var fontTexture = new Texture2D(atlasWidth, altasHeight, TextureFormat.Alpha8, mipmap: false, linear: true);

			Color32[] colors = new Color32[atlasWidth * altasHeight];
			for (int i = 0; i < (atlasWidth * altasHeight); i++) {
				byte c = textureBuffer[i];
				colors[i] = new Color32(c, c, c, c);
			}

			if (fontRenderMode == RenderModes.RasterHinted) {
				fontTexture.filterMode = FilterMode.Point;
			}

			fontTexture.SetPixels32(colors, miplevel: 0);
			fontTexture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			return fontTexture;
		}

		// Convert from FT_FaceInfo to FaceInfo
		private static FaceInfo ConvertToFaceInfo(FT_FaceInfo ftFace) {
			FaceInfo face = new FaceInfo();

			face.Name = ftFace.name;
			face.PointSize = (float)ftFace.pointSize;
			face.Padding = ftFace.padding;
			face.LineHeight = ftFace.lineHeight;
			face.Baseline = 0;
			face.Ascender = ftFace.ascender;
			face.Descender = ftFace.descender;
			face.CenterLine = ftFace.centerLine;
			face.Underline = ftFace.underline;
			face.UnderlineThickness = ftFace.underlineThickness == 0 ? 5 : ftFace.underlineThickness; // Set Thickness to 5 if TTF value is Zero.
			face.SuperscriptOffset = face.Ascender;
			face.SubscriptOffset = face.Underline;
			face.SubSize = 0.5f;
			//face.CharacterCount = ftFace.characterCount;
			face.AtlasWidth = ftFace.atlasWidth;
			face.AtlasHeight = ftFace.atlasHeight;

			if (face.strikethrough == 0) {
				face.strikethrough = face.CapHeight / 2.5f;
			}

			return face;
		}


		// Convert from FT_GlyphInfo[] to GlyphInfo[]
		private static TMP_Glyph[] ConvertToGlyphs(FT_GlyphInfo[] ftGlyphs) {
			List<TMP_Glyph> glyphs = new List<TMP_Glyph>();

			for (int i = 0; i < ftGlyphs.Length; i++) {
				TMP_Glyph g = new TMP_Glyph();

				g.id = ftGlyphs[i].id;
				g.x = ftGlyphs[i].x;
				g.y = ftGlyphs[i].y;
				g.width = ftGlyphs[i].width;
				g.height = ftGlyphs[i].height;
				g.xOffset = ftGlyphs[i].xOffset;
				g.yOffset = ftGlyphs[i].yOffset;
				g.xAdvance = ftGlyphs[i].xAdvance;

				// Filter out characters with missing glyphs.
				if (g.x == -1) {
					continue;
				}

				glyphs.Add(g);
			}

			return glyphs.ToArray();
		}
	}
}
#endif