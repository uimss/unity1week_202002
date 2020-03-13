using UnityEngine;
using UnityEditor;

internal sealed class PixelTextureImporter : AssetPostprocessor {
    void OnPreprocessTexture() {

        var targetFolder = "Assets/Textures/PixelArt";
        var importer = assetImporter as TextureImporter;
        Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
        if (asset) return;
        if (!importer.assetPath.StartsWith(targetFolder)) return;

        TextureImporterSettings settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);

        settings.spriteAlignment = (int)SpriteAlignment.BottomCenter;
        settings.spritePixelsPerUnit = 1;
        settings.filterMode = FilterMode.Point;

        importer.SetTextureSettings(settings);
    }
}
