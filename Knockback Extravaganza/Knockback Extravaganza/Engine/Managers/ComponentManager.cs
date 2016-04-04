using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Managers {
    public class ComponentManager {

        Dictionary<Type, Dictionary<Entity, IComponent>> componentList = new Dictionary<Type, Dictionary<Entity, IComponent>>();

        public ComponentManager() {

        }

        void AddComponent(Entity entity, IComponent component) {
            Type type = component.GetType();
            if (!componentList.ContainsKey(type)){
                componentList[type] = new Dictionary<Entity, IComponent>();
            }
            componentList[type][entity] = component;
        }

        void RemoveComponent<T>(Entity entity) where T : IComponent {
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                if (componentList[type].ContainsKey(entity)) {
                    componentList[type].Remove(entity);
                }
            }

        }

        T GetComponent<T>(Entity entity) where T : IComponent {
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                if (componentList[type].ContainsKey(entity)) {
                    return (T)componentList[type][entity];
                }
            }
            return default(T);
        }

        Entity GetEntity<T>(IComponent component) {
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                if (componentList[type].ContainsValue(component)) {
                    foreach(KeyValuePair<Entity, IComponent> kvp in componentList[type]) {
                        if(kvp.Value == component) {
                            return kvp.Key;
                        }
                    }
                }
            }
            return null;
        }

        Dictionary<Entity, IComponent> GetComponents<T>(){
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                return componentList[type];
            }
            return null;
        }

    }
}
