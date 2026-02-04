using System;
using System.Collections.Generic;

/// <summary>
/// 配置表基类
/// </summary>
[Serializable]
public abstract class BaseConfig: global::ProtoBuf.IExtensible
{
    /// <summary>
    /// 编号
    /// </summary>
    public int id;

    public BaseConfig() { }
    public abstract void Initialize(Dictionary<string, object> _dataDic);

    private global::ProtoBuf.IExtension __pbn__extensionData;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createlfMissing)
        => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createlfMissing);
}
