/*
 Generalised event handler class using delegates
 Author: Miltiadis Nedelkos, nedelkosm at gmail com
 Date: June 2017
 Source: https://github.com/NedelkosM/XMLParser
 Version: v1.1
*/
using System;
using System.IO;
using System.Xml.Serialization;

public class XMLParser {
    /// <summary>
    /// Parses an .XML file and deserializes its contents to an object of type T
    /// </summary>
    /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
    /// <param name="file">The path and filename of the file to be parsed.</param>
    /// <param name="extraTypes">Extra types to consider when deserializing (for custom classes/structs). 
    /// All types besides primitives must be declared. 
    /// It is used to match exactly to your custom classes.</param>
    /// <returns>The deserialized object.</returns>
    public static T XMLFileToObject<T>(string file, Type[] extraTypes = null) {
        var serializer = new XmlSerializer((typeof(T)), extraTypes);
        var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var streamReader = new StreamReader(stream);
        var fileContents = streamReader.ReadToEnd();
        fileContents = fileContents.Replace(',', '.');
        stream.Close();

        var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContents ?? ""));
        memoryStream.Position = 0;
        var container = (T) serializer.Deserialize(memoryStream);
        memoryStream.Close();
        return container;
    }

    /// <summary>
    /// Serializes an object and saves it to a file.
    /// </summary>
    /// <typeparam name="T">The type of the object to be serialized.</typeparam>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="file">The path and filename of the file to be created/overwritten.</param>
    /// <param name="extraTypes">Extra types to consider when serializing (for custom classes/structs). 
    /// All types besides primitives must be declared. 
    /// It is used to match exactly to your custom classes.</param>
    public static bool ObjectToXMLFile<T>(T obj, string file, Type[] extraTypes = null) {
        try {
            var serializer = new XmlSerializer((typeof(T)), extraTypes);
            var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            serializer.Serialize(stream, obj);
            stream.Close();
            return true;
        } catch(Exception e) {
            Console.WriteLine("Could not serialize to file: " + e.Message);
            return false;
        }
    }

    /// <summary>
    /// Parses XML content and deserializes it to an object of type T
    /// </summary>
    /// <typeparam name="T">The type of object to be deserialized.</typeparam>
    /// <param name="content">The XML content to be parsed.</param>
    /// <param name="extraTypes">Extra types to consider when deserializing (for custom classes/structs). 
    /// All types besides primitives must be declared. 
    /// It is used to match exactly to your custom classes.</param>
    /// <returns>The deserialized object.</returns>
    public static T XMLToObject<T>(string content, Type[] extraTypes = null) {
        var serializer = new XmlSerializer((typeof(T)), extraTypes);
        var memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content ?? ""));
        memoryStream.Position = 0;
        var container = (T) serializer.Deserialize(memoryStream);
        memoryStream.Close();
        return container;
    }

    /// <summary>
    /// Serializes an object to XML format.
    /// </summary>
    /// <typeparam name="T">The type of the object to be serialized.</typeparam>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="extraTypes">Extra types to consider when serializing (for custom classes/structs). 
    /// All types besides primitives must be declared. 
    /// It is used to match exactly to your custom classes.</param>
    /// <returns>The serialized object in XML format.</returns>
    public static string ObjectToXML<T>(T obj, Type[] extraTypes = null) {
        try {
            var serializer = new XmlSerializer((typeof(T)), extraTypes);
            var stream = new MemoryStream();
            serializer.Serialize(stream, obj);
            stream.Position = 0;
            var streamReader = new StreamReader(stream);
            var xml = streamReader == null ? "" : streamReader.ReadToEnd();
            streamReader.Close();
            return xml;
        } catch(Exception e) {
            Console.WriteLine("Could not create XML object: " + e.Message);
        }
        return "Invalid object";
    }
}
