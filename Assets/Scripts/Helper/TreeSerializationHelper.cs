using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class TreeSerializationHelper 
{
    #region public
    
    public static byte[] SerializeTree<T>(Tree<T> tree)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        try
        {
            formatter.Serialize(stream, tree);
            return stream.ToArray();
        }
        catch (Exception e)
        {
            Debug.LogError($"Serialization failed: {e.Message}");
            return null;
        }
        finally
        {
            stream.Close();
        }
    }

    public static Tree<T> DeserializeTree<T>(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Debug.LogError("Data is null or empty.");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(data);
        try
        {
            return (Tree<T>)formatter.Deserialize(stream);
        }
        catch (Exception e)
        {
            Debug.LogError($"Deserialization failed: {e.Message}");
            return null;
        }
        finally
        {
            stream.Close();
        }
    }
    #endregion
    
}
