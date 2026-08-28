using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class JExtraUtility : MonoBehaviour {

    public static string planscapeGeneratedFolder = Path.Combine(Application.streamingAssetsPath, "ContentPacks", "PlanscapeGenerated");

    public static void SaveSprite (Sprite sprite) { SaveTexture(sprite.texture); }
    public static void SaveTexture (Texture2D texture) {
        string thumbnailFolderPath = Path.Combine(planscapeGeneratedFolder, "Images");
        if(!Directory.Exists(thumbnailFolderPath)) { Directory.CreateDirectory(thumbnailFolderPath); }
        //byte[] pngData = DeCompress(thumbnail.texture).EncodeToPNG();

        if(texture != null) { File.WriteAllBytes(Path.Combine(thumbnailFolderPath, texture.name) + ".png", texture.EncodeToPNG()); } //byte[] pngData = texture.EncodeToPNG();
    }

    /*public static Texture2D DeCompress (Texture2D source) {
        RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }*/

    public static Sprite LoadNewSprite(string FilePath) { return LoadNewSprite(FilePath, 100.0f); }
    public static Sprite LoadNewSprite (string FilePath, float PixelsPerUnit = 100.0f) {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);
        newSprite.name = Path.GetFileName(FilePath);
        return newSprite;
    }

    public static Texture2D LoadTexture (string FilePath) {
        // Load a PNG or JPG file from disk to a Texture2D, returns null if load fails
        Texture2D Tex2D;
        byte[] FileData;
        if(File.Exists(FilePath)) {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2); // Create new "empty" texture
            if(Tex2D.LoadImage(FileData)) { // Load the imagedata into the texture (size is set automatically)
                return Tex2D; // If data = readable -> return texture
            } 
        }
        return null; // Return null if load failed
    }

    /*public static AudioClip LoadAudioClip (string FilePath) {
        AudioClip audio;
        byte[] FileData;

        if(File.Exists(FilePath)) {
            FileData = File.ReadAllBytes(FilePath);
            audio = new AudioClip();
            if(audio.Load(FileData)) {
                return audio;
            }
        }
        return null;
    }*/

    public static void SaveAllResourcesToJson () {
        SaveObjectsToJson<Campaign>("Campaigns");
        SaveObjectsToJson<Week>("Weeks");
        SaveObjectsToJson<ActivityObject>("Activities");
        SaveObjectsToJson<EventObject>("Events");

        SaveObjectsToJson<LanguageFile>("Lang");
        SaveObjectsToJson<MenuTheme>("Themes");
    }

    public static void SaveObjectsToJson<T> (string typeName) where T : StuffedScriptableObject {
        string folderPath = Path.Combine(planscapeGeneratedFolder, typeName);
        if(!Directory.Exists(folderPath)) { Directory.CreateDirectory(folderPath); }
        //Debug.Log("Writing " + typeName + " data to " + folderPath);

        T[] objectList = Resources.LoadAll<T>(typeName);
        for(int i = 0; i < objectList.Length; i++) {
            Debug.Log("Generating \"" + objectList[i].name + "\"... (" + typeName + " " + (i + 1) + "/" + objectList.Length + ")");
            objectList[i].Save();
        }
    }

    /*public static T[] LoadObjectsFromJson<T> (string typeName) where T : IJDataHolder {
        List<T> list = new List<T>();
        foreach(string targetFolder in Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, "ContentPacks"), typeName, SearchOption.AllDirectories)) {
            foreach(string file in Directory.GetFiles(targetFolder, "*.json")) {
                //list.Add(JsonUtility.FromJson<T.JData>(File.ReadAllText(file)).LoadData());
                list.Add(T.LoadData(file));
            }
        }
        return list.ToArray();
    }*/

    public static string[] LoadJsonFilesOfType(string typeName) {
        List<string> list = new List<string>();
        foreach(string targetFolder in Directory.GetDirectories(Path.Combine(Application.streamingAssetsPath, "ContentPacks"), typeName, SearchOption.AllDirectories)) {
            foreach(string file in Directory.GetFiles(targetFolder, "*.json")) { list.Add(file); }
        }
        return list.ToArray();
    }

    /*public static T LoadObjectFromJson<T> (string filePath) where T : StuffedScriptableObject {

    }*/
}