using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems {
    public class SoundSystem : IUpdateSystem {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager) {
            var effectComponents = componentManager.GetComponents<SoundEffectComponent>();
            var songComponents = componentManager.GetComponents<SongComponent>();
            if (effectComponents != null) {
                foreach (KeyValuePair<Entity, IComponent> component in effectComponents) {

                    var messages = messageManager.GetMessages(component.Key.ID);

                    SoundEffectComponent soundEffectComp = (SoundEffectComponent)component.Value;
                    
                    Dictionary<string, SoundEffectPiece> effects = soundEffectComp.GetEffectDictionary();
                    
                    foreach (Message message in messages) {
                        switch (message.msg) {
                            case "START: footstep":
                                
                                    if (effects["footstep1"].CurrentActiveTime <= 0) {
                                        soundEffectComp.PlaySoundEffect(effects["footstep1"].SoundEffect, 0.1f);
                                        effects["footstep1"].CurrentActiveTime = effects["footstep1"].ActiveTime;
                                        
                                    } else {
                                        effects["footstep1"].CurrentActiveTime -= gameTime.ElapsedGameTime.Milliseconds;
                                    }
                                    
                                
                                
                                    if (effects["footstep2"].CurrentActiveTime <= 0) {
                                        soundEffectComp.PlaySoundEffect(effects["footstep2"].SoundEffect, 0.1f);
                                        effects["footstep2"].CurrentActiveTime = effects["footstep2"].ActiveTime + 100;
                                        
                                    } else {
                                        effects["footstep2"].CurrentActiveTime -= gameTime.ElapsedGameTime.Milliseconds;
                                    }
                                    
                                
                                  
                            break;
                            
                        }
                    }
                    //foreach (string key in effects.Keys) {
                    //    if (effects[key].IsActive) {
                    //        soundEffectComp.PlaySoundEffect(effects[key].SoundEffect);
                    //        effects[key].IsActive = true;
                    //    }
                    //}
                    
                }
            }

            if (songComponents != null) {
                foreach (KeyValuePair<Entity, IComponent> component in songComponents) {

                    var msg = messageManager.GetMessages(component.Key.ID);
   
                    SongComponent songComp = (SongComponent)component.Value;
                    
                    Dictionary<string, SongPiece> songs = songComp.GetSongDictionary();

                    foreach (string key in songs.Keys) {
                        if (songs[key].AboutToStart) {
                            songComp.PlaySong(songs[key].Song);
                            songs[key].AboutToStart = false;
                            songs[key].IsActive = true;
                        }

                        if (!songs[key].IsActive) {
                            songComp.StopSong();
                        }
                    }

                }
            }
        }
    }
}
