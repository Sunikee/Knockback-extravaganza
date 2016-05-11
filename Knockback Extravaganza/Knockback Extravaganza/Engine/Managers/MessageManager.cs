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
    }
    public class MessageManager {

        private List<Message> messages = new List<Message>();
        private DateTime startTime;
        public void Begin(GameTime gametime) {
            startTime = DateTime.Now;
            for(int i = 0; i < messages.Count; ++i) {
                messages[i].activateInSeconds -= (float)gametime.ElapsedGameTime.TotalSeconds;
            }
        }
        public void End() {
            messages.RemoveAll(x => x.activateInSeconds < 0);
        }
        public void RegMessage(Message msg) {
            messages.Add(msg);
        }
        public List<Message> GetMessages(int id) {
            
            return messages.FindAll(x => x.receiver == id && x.activateInSeconds <= 0) as List<Message>;
        }

    }
}
