using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component.Abstract_Classes {
    

    public abstract class ThreadedComponent : IComponent{
        private enum bufferNames {
            UPDATE_BUFFER = 0,
            FIRST_RENDER_BUFFER,
            SECOND_RENDER_BUFFER,
        }

        public const int updateBuffer = (int)bufferNames.UPDATE_BUFFER;

        private int idleRenderBuffer = (int)bufferNames.SECOND_RENDER_BUFFER;
        private int currentRenderBuffer = (int)bufferNames.FIRST_RENDER_BUFFER;

        public int UpdateBuffer { get { return updateBuffer; } private set { } }
        public int CurrentRenderBuffer { get { return currentRenderBuffer; } private set { currentRenderBuffer = value; } }
        public int IdleRenderBuffer { get { return idleRenderBuffer; } private set { idleRenderBuffer = value; } }
        public void ChangeRenderBuffer() {
            if(CurrentRenderBuffer == (int)bufferNames.FIRST_RENDER_BUFFER) {
                CurrentRenderBuffer = (int)bufferNames.SECOND_RENDER_BUFFER;
                IdleRenderBuffer = (int)bufferNames.FIRST_RENDER_BUFFER;
            }
            else {
                CurrentRenderBuffer = (int)bufferNames.FIRST_RENDER_BUFFER;
                IdleRenderBuffer = (int)bufferNames.SECOND_RENDER_BUFFER;
            }
        }

        public abstract void CopyThreadedData(int to, int from = updateBuffer);

    }
}
