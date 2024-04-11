using UnityEngine;

namespace Interfaces{
    interface IMovable{

        /// <summary>
        /// Pushes the object in the direction provided.
        /// </summary>
        /// <param name="direction">Push direction.</param>
        void Push(Vector2 direction);
        /// <summary>
        /// Returns whether or not the object can be moved.
        /// </summary>
        /// <param name="direction">The direction the player wants to move the object.</param>
        /// <returns></returns>
        bool CanBePushed(Vector2 direction);

    }
}