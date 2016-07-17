using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Managers {
    public class Message {
        public float activateInSeconds;
        public int sender;
        public int receiver;
        public string msg;

        public void Set(int sender, int receiver, float activateInSeconds, string msg) {
            this.sender = sender;
            this.receiver = receiver;
            this.activateInSeconds = activateInSeconds;
            this.msg = msg;
        }
    }
    public class MessageManager {

        private Message[] messages = new Message[50];
        private DateTime startTime;

        public MessageManager() {
            for(int i = 0; i < messages.Length; ++i) {
                messages[i] = new Message() {
                    activateInSeconds = -1,
                    sender = -1,
                    receiver = -1,
                    msg = "",
                };
            }
        }

        public void Begin(GameTime gametime) {
            startTime = DateTime.Now;
            for(int i = 0; i < messages.Length; ++i) {
                messages[i].activateInSeconds -= (float)gametime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void End() {
            for(int i = 0; i < messages.Length; ++i) {
                if(messages[i].activateInSeconds < 0) {
                    messages[i].Set(-1,-1,-1, "");
                }
            }
        }

        public void RegMessage(int sender, int receiver, float activateInSeconds, string msg) {
            for(int i = 0; i <= messages.Length; ++i) {
                if(i == messages.Length) {
                    Message[] newMsg = new Message[messages.Length * 2];
                    for(int j = 0; j < newMsg.Length; ++j) {
                        if (j < messages.Length) {
                            newMsg[j] = messages[j];
                        }
                        else {
                            newMsg[j] = new Message() {
                                activateInSeconds = -1,
                                sender = 0,
                                receiver = 0,
                                msg = "",
                            };
                        }
                    }
                    messages = newMsg;
                    i = 0;
                }
                if(messages[i].activateInSeconds <= -1) {
                    messages[i].Set(sender, receiver, activateInSeconds, msg);
                    break;
                }
            }
        }
        public List<Message> GetMessages(int id) {
            return messages.Where(x => x.activateInSeconds < 0 && x.receiver == id).ToList();
        }

        public void DestroyMessage(Message message) {
            messages.First(x => x == message).Set(-1, -1, -1, "");
        }

    }
}
