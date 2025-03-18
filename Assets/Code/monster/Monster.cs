
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg.monster
{
public sealed partial class Monster : Luban.BeanBase
{
    public Monster(JSONNode _buf) 
    {
        { if(!_buf["id"].IsString) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["anime"].IsString) { throw new SerializationException(); }  Anime = _buf["anime"]; }
        { var __json0 = _buf["properties"]; if(!__json0.IsArray) { throw new SerializationException(); } Properties = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  Properties.Add(__v0); }   }
        { var __json0 = _buf["items"]; if(!__json0.IsArray) { throw new SerializationException(); } int _n0 = __json0.Count; Items = new ItemData[_n0]; int __index0=0; foreach(JSONNode __e0 in __json0.Children) { ItemData __v0;  { if(!__e0.IsObject) { throw new SerializationException(); }  __v0 = ItemData.DeserializeItemData(__e0);  }  Items[__index0++] = __v0; }   }
        { if(!_buf["system"].IsString) { throw new SerializationException(); }  System = _buf["system"]; }
        { if(!_buf["skill_tips"].IsString) { throw new SerializationException(); }  SkillTips = _buf["skill_tips"]; }
        { if(!_buf["desc"].IsString) { throw new SerializationException(); }  Desc = _buf["desc"]; }
    }

    public static Monster DeserializeMonster(JSONNode _buf)
    {
        return new monster.Monster(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public readonly string Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 動畫
    /// </summary>
    public readonly string Anime;
    /// <summary>
    /// 屬性(hp/atk/def/gold/exp/fatigue/breath/times)
    /// </summary>
    public readonly System.Collections.Generic.List<int> Properties;
    /// <summary>
    /// id
    /// </summary>
    public readonly ItemData[] Items;
    /// <summary>
    /// 系統
    /// </summary>
    public readonly string System;
    /// <summary>
    /// 討伐提示
    /// </summary>
    public readonly string SkillTips;
    /// <summary>
    /// 描述
    /// </summary>
    public readonly string Desc;
   
    public const int __ID__ = 1495842214;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        foreach (var _e in Items) { _e?.ResolveRef(tables); }
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "anime:" + Anime + ","
        + "properties:" + Luban.StringUtil.CollectionToString(Properties) + ","
        + "items:" + Luban.StringUtil.CollectionToString(Items) + ","
        + "system:" + System + ","
        + "skillTips:" + SkillTips + ","
        + "desc:" + Desc + ","
        + "}";
    }
}

}

