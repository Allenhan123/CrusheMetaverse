using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void UpdateMeshRenders(this GameObject go)
    {
        var meshes = go.GetComponentsInChildren<Renderer>();

        foreach (var mesh in meshes)
        {
            if(mesh.sharedMaterial != null)
            {
                string oldShaderName = mesh.sharedMaterial.shader.name;
                mesh.sharedMaterial.shader = Shader.Find(oldShaderName);
            }
            
        }
    }


    /// <summary>
    /// 得到child 组件
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="childPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindComponent<T>(this Transform tf, string childPath = "")
    {
        if(string.IsNullOrEmpty(childPath))
        {
            return tf.GetComponent<T>();
        }

        Transform childTf = tf.Find(childPath);
        if(childTf == null)
        {
            Debug.LogErrorFormat("find child transform path is error => {0}", childPath);
            return default(T);
        }

        return childTf.GetComponent<T>();
    }

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>获取或增加的组件。</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象</param>
    /// <param name="displayName">组件的显示名字</param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject, string displayName) where T : Component
    {
        var go = new GameObject(displayName);
        go.transform.SetParent(gameObject.transform);
        return go.GetOrAddComponent<T>();
    }

        /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <param name="gameObject">目标对象。</param>
    /// <param name="type">要获取或增加的组件类型。</param>
    /// <returns>获取或增加的组件。</returns>
    public static Component GetOrAddComponent(this GameObject gameObject, Type type)
    {
        Component component = gameObject.GetComponent(type);
        if (component == null)
        {
            component = gameObject.AddComponent(type);
        }

        return component;
    }

}
