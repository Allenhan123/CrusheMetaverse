using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemExtensions
{
    public static string MapToString<Tkey, Tvlaue>(this Dictionary<Tkey, Tvlaue> dic)
    {
        if(dic == null)
        {
            return string.Empty;
        }

        string ret = string.Empty;

        foreach(var item in dic)
        {
            ret += item.Key + "-" + item.Value + " | ";
        }

        return ret;
    }

    public static Tvlaue GetMapValue<Tkey, Tvlaue>(this Dictionary<Tkey, Tvlaue> dic, Tkey key)
    {
        if(dic == null)
        {
            return default(Tvlaue);
        }

        if(!dic.ContainsKey(key))
        {
            return default(Tvlaue);
        }

        return dic[key];
    }
}
