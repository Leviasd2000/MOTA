
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class PercentPropertyData : Luban.BeanBase
{
    public PercentPropertyData(JSONNode _buf) 
    {
        { if(!_buf["attribute"].IsString) { throw new SerializationException(); }  Attribute = _buf["attribute"]; }
        { if(!_buf["count"].IsNumber) { throw new SerializationException(); }  Count = _buf["count"]; }
    }

    public static PercentPropertyData DeserializePercentPropertyData(JSONNode _buf)
    {
        return new PercentPropertyData(_buf);
    }

    /// <summary>
    /// 屬性
    /// </summary>
    public readonly string Attribute;
    /// <summary>
    /// 數值
    /// </summary>
    public readonly int Count;
   
    public const int __ID__ = 1683197924;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "attribute:" + Attribute + ","
        + "count:" + Count + ","
        + "}";
    }
}

}

