using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ResourceSourcesList 
{   
    public static UnityEvent<ResourceSource> ResourceSourceAdded;

    private static List<ResourceSource> _list;

    public static void SetupList() 
    {
        _list = new List<ResourceSource>();
    
        ResourceSourceAdded = null;
    }
    
    public static List<ResourceSource> GetList() => new List<ResourceSource>(_list);

    public static void Add(ResourceSource resourceSourceToAdd)
    {
        _list.Add(resourceSourceToAdd);

        ResourceSourceAdded?.Invoke(resourceSourceToAdd);
    }

    public static void Remove(ResourceSource resourceSourceToRemove) => _list.Remove(resourceSourceToRemove);
}
