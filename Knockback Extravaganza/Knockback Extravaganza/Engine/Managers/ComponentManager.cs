using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;


namespace ECS_Engine.Engine.Managers {
 
    public class ComponentManager {

        Entity[] entities = new Entity[10];
        Dictionary<Type, Dictionary<Entity, IComponent>> componentList = new Dictionary<Type, Dictionary<Entity, IComponent>>();


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
                componentList[type] = new Dictionary<Entity, IComponent>();
            }
            componentList[type][entity] = component;
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
                    componentList[type].Remove(entity);
                }
            }
        }
        public void RemoveAllComponents(Entity entity) {
            foreach(var dict in componentList) {
                foreach(var value in dict.Value) {
                    if(value.Key == entity) {
                        dict.Value.Remove(entity);
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
                if (componentList[type].ContainsValue(component)) {
                    foreach (KeyValuePair<Entity, IComponent> kvp in componentList[type]) {
                        if (kvp.Value == component) {
                            return kvp.Key;
                        }
                    }
                }
            }
            return null;
        }

        public Dictionary<Entity, IComponent> GetComponents<T>() where T : IComponent{
            Type type = typeof(T);
            if (componentList.ContainsKey(type)) {
                return componentList[type];
            }
            return null;
        }
    }
}
