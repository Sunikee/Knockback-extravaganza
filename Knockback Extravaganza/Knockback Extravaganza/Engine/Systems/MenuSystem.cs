using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;

namespace ECS_Engine.Engine.Systems {
    public class MenuSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager) {
            Dictionary<Entity, IComponent> menuComponents = componentManager.GetComponents<MenuComponent>();
            if (menuComponents != null) {
                foreach (KeyValuePair<Entity, IComponent> component in menuComponents) {
                    KeyBoardComponent menuKeys = componentManager.GetComponent<KeyBoardComponent>(component.Key);
                    MenuComponent menu = component.Value as MenuComponent;

                    if (menuKeys.GetActionState("Up") == BUTTON_STATE.PRESSED)
                        menu.ActiveChoice--;
                    if (menuKeys.GetActionState("Down") == BUTTON_STATE.PRESSED)
                        menu.ActiveChoice++;


                    MathHelper.Clamp(menu.ActiveChoice, 0, menu.MenuEntries.Count - 1);



                }
            }
        }

    }
}
