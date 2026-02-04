using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class ResourceLoader
{
    // ====== 内部资源池 ======
    private static Dictionary<string, Queue<UnityEngine.Object>> pool
        = new Dictionary<string, Queue<UnityEngine.Object>>();

    // ====== 公共加载入口 ======
    public static void Load<T>(string pathOrKey, Action<T> onDone, bool usePool = false) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        // ========== 编辑器：保持本地使用方式 ==========
        if (typeof(T) == typeof(TextAsset))
        {
            string json = File.ReadAllText(pathOrKey);
            onDone?.Invoke(new TextAsset(json) as T);
        }
        else if (typeof(T) == typeof(Sprite))
        {
            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(pathOrKey);
            onDone?.Invoke(sprite as T);
        }
        else
        {
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(pathOrKey);
            onDone?.Invoke(asset);
        }
#else
         if (usePool)
        {
            // 先尝试从池里取
            if (pool.TryGetValue(pathOrKey, out var q) && q.Count > 0)
            {
                onDone?.Invoke(q.Dequeue() as T);
            }
        }
        // ========== 运行时：走 Addressables ==========
        if (typeof(T) == typeof(Sprite)){ LoadSpriteAsync(pathOrKey,onDone);}
        else { LoadAssetAsync(pathOrKey, onDone, usePool); }
#endif
    }
    private static async Task LoadAssetAsync<T>(string pathOrKey, Action<T> onDone, bool usePool = false) where T : UnityEngine.Object
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(pathOrKey);
        await handle.Task;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load : {pathOrKey}");
            onDone?.Invoke(null);
        }

        if (usePool && handle.Result != null)
        {
            if (!pool.ContainsKey(pathOrKey))
                pool[pathOrKey] = new Queue<UnityEngine.Object>();
        }

        onDone?.Invoke(handle.Result);
    }
    private static async Task LoadSpriteAsync<T>(string pathOrKey, Action<T> onDone) where T : UnityEngine.Object
    {
        string spriteName = Path.GetFileNameWithoutExtension(pathOrKey);
        string atlasName = Path.GetFileName(Path.GetDirectoryName(pathOrKey));
        var handleAtlas = Addressables.LoadAssetAsync<SpriteAtlas>(atlasName);
        await handleAtlas.Task;

        if (handleAtlas.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load atlas : {atlasName}");
            onDone?.Invoke(null);
            return;
        }
        Sprite sprite = handleAtlas.Result.GetSprite(spriteName);
        if (sprite != null)
        {
            if (!pool.ContainsKey(pathOrKey))
                pool[pathOrKey] = new Queue<UnityEngine.Object>();
        }

        onDone?.Invoke(sprite as T);
    }
    // ====== 回收接口（给“由本类管理池”的资源用） ======
    public static void Release(string key, UnityEngine.Object obj)
    {
        if (obj == null) return;

#if UNITY_EDITOR
        // 编辑器阶段不真正回收，什么都不做
#else
        if (!pool.ContainsKey(key))
            pool[key] = new Queue<UnityEngine.Object>();

        pool[key].Enqueue(obj);
#endif
    }
}
