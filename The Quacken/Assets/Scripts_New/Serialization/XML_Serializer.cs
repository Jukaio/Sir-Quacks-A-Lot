using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization;
using System.Xml;

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
        if (File.Exists(p_path))
        {
            Debug.Log("No File in Directory");
            return default(T);
        }
        StreamReader reader = new StreamReader(p_path);
        T deserialzed = (T)serializer.Deserialize(reader.BaseStream);

        reader.Close();
        return deserialzed;
    }

    public static T Deserialize<T>(TextAsset p_file)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        var temp = XmlReader.Create(new MemoryStream(p_file.bytes));

        //StreamReader reader = new StreamReader(p_path);
        T deserialzed = (T)serializer.Deserialize(temp);
        temp.Close();
        return deserialzed;
    }
}
