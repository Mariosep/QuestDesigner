using System;
using System.Collections.Generic;
using System.Linq;
using Blackboard.Events;
using QuestDesigner;
using UnityEngine;

public static class ServiceProvider
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        ServiceLocator.Initialize();
        
        // Register all services
        ServiceLocator.Register(new InputManager());
        ServiceLocator.Register(new QuestChannel());
        
        // Register event channels
        RegisterEventChannels();
    }

    private static void RegisterEventChannels()
    {
        IEnumerable<Type> GetAllEventChannelsTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(EventChannel)) && !type.IsAbstract);
        }

        IEnumerable<Type> derivedTypes = GetAllEventChannelsTypes();
        foreach (Type derivedType in derivedTypes)
        {
            if (Activator.CreateInstance(derivedType) is EventChannel eventChannel)
            {
                var getMethod = typeof(ServiceLocator).GetMethod("Register").MakeGenericMethod(eventChannel.GetType());
                object result = getMethod.Invoke(null, new []{eventChannel} );
            }
        }
    }
}
