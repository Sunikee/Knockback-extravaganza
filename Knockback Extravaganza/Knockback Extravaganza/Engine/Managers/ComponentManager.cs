using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component.Abstract_Classes;
using System.Collections.Concurrent;

namespace ECS_Engine.Engine.Managers {
 
    public class ComponentManager {

        Entity[] entities = new Entity[10];
        ConcurrentDictionary<Type, ConcurrentDictionary<Entity, IComponent>> componentList = new ConcurrentDictionary<Type, ConcurrentDictionary<Entity, IComponent>>();


        public Entity MakeEntity() {
            int index = 0;
            for(int i = 0; i <= entities.Length; ++i) {
                if(i == entities.Length) {
                    Entity[] newEntities = new Entity[entities.Length * 2];
                    for(int j = 0; j < entities.Length; j++) {
                        newEntities[j] = entities[j];
                    }
                    entities = newEntities;
                    i = 0;
                }
                if(entities[i] == null) {
                    index = i;
                    break;
                }
            }
            int id = 0;
            for(int i = 0; i < entities.Length; i++) {
                if(entities[i] != null) {
                    id = Math.Min(id, entities[i].ID);
                }
            }
            for(int i = 0; i < entities.Length; i++) {
                if (entities[i] != null) {
                    if (id == entities[i].ID) {
                        id++;
                    }
                }
            }
            entities[index] = new Entity(id);
            return entities[index];
        }

        public Entity GetEntity(int id) {
            Entity entity = null;
            for(int i = 0; i < entities.Length; ++i) {
                if(entities[i] != null) {
                    if(entities[i].ID == id) {
                        entity = entities[i];
                        break;
                    }
                }
            }
            return entity;
        }

        public void AddComponent(Entity entity, IComponent component) {
            Type type = component.GetType();
            if (!componentList.ContainsKey(type)) {
                componentList.TryAdd(type, new ConcurrentDictionary<Entity, IComponent>());
            }
            componentList[type].TryAdd(entity, component); //[entity] = component;
        }

        public void AddComponent(Entity entity, params IComponent[] components) {
            foreach(IComponent component in components) {
                AddComponent(entity, component);
            }
        }

        public void RemoveComponent<T>(Entity entity) where T : IComponent {
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                if (componentList[type].ContainsKey(entity)) {
                    IComponent test;
                    componentList[type].TryRemove(entity, out test);
                }
            }
        }
        public void RemoveAllComponents(Entity entity) {
            foreach(var dict in componentList) {
                foreach(var value in dict.Value) {
                    if(value.Key == entity) {
                        IComponent test;
                        dict.Value.TryRemove(entity, out test);
                        break;
                    }
                }
            }
        }

        public void RemoveEntity(Entity entity) {
            if (entities.Contains(entity)) {
                RemoveAllComponents(entity);
                for(int i = 0; i < entities.Length; ++i ) {
                    if(entities[i] == entity) {
                        entities[i] = null;
                    }
                }
            }
        }

        public T GetComponent<T>(Entity entity) where T : IComponent {
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                if (componentList[type].ContainsKey(entity)) {
                    return (T)componentList[type][entity];
                }
            }
            return default(T);
        }

        public Entity GetEntity<T>(IComponent component) where T : IComponent{
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                ConcurrentDictionary<Entity, IComponent> typeList;
                componentList.TryGetValue(type, out typeList);
                foreach(var r in typeList.Keys) {
                    IComponent comp;
                    typeList.TryGetValue(r, out comp);
                    if(comp == component) {
                        return r;
                    }
                }
            }
            return null;
        }

        public ConcurrentDictionary<Entity, IComponent> GetComponents<T>() where T : IComponent{
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                ConcurrentDictionary<Entity, IComponent> result;
                componentList.TryGetValue(type, out result);
                return result;
            }
            return null;
        }

        public List<ThreadedComponent> GetThreadedComponents() {
            List<ThreadedComponent> result = new List<ThreadedComponent>();
            foreach(var type in componentList.Keys) {
                ConcurrentDictionary<Entity, IComponent> typeList;
                componentList.TryGetValue(type, out typeList);
                foreach(var components in typeList.Keys) {
                    IComponent comp;
                    typeList.TryGetValue(components, out comp);
                    if(comp is ThreadedComponent) {
                        result.Add(comp as ThreadedComponent);
                    }
                }
            }
            return result;
        }
    }
}
