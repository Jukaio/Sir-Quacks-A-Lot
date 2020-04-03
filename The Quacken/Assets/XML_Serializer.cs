using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization;

public class XML_Serializer
{
    public static void Serialize(object p_item, string p_path)
    {
        XmlSerializer serializer = new XmlSerializer(p_item.GetType());
        StreamWriter writer = new StreamWriter(p_path);
        serializer.Serialize(writer.BaseStream, p_item);
        writer.Close();
    }

    public static T Deserialize<T>(string p_path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(p_path);
        T deserialzed = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialzed;
    }
}
