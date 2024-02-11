using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> m_adjacencyList;

    public Graph()
    {
        m_adjacencyList = new Dictionary<T, List<T>>();
    }

    public void AddVertex(T vertex)
    {
        if (!m_adjacencyList.ContainsKey(vertex))
        {
            m_adjacencyList[vertex] = new List<T>();
        }
    }

    public void AddEdge(T source, T destination)
    {
        if (!m_adjacencyList.ContainsKey(source))
        {
            AddVertex(source);
        }

        if (!m_adjacencyList.ContainsKey(destination))
        {
            AddVertex(destination);
        }

        m_adjacencyList[source].Add(destination);
    }

    public List<T> GetNeighbors(T vertex)
    {
        if (m_adjacencyList.ContainsKey(vertex))
        {
            return m_adjacencyList[vertex];
        }

        return null;
    }

    public bool ContainsVertex(T vertex)
    {
        return m_adjacencyList.ContainsKey(vertex);
    }

    public bool ContainsEdge(T source, T destination)
    {
        if (m_adjacencyList.ContainsKey(source))
        {
            return m_adjacencyList[source].Contains(destination);
        }

        return false;
    }
}

