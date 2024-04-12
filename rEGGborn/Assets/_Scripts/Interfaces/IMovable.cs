using UnityEngine;

namespace Interfaces{
    interface IMovable{

        /// <summary>
        /// Pushes the object in the direction provided.
        /// </summary>
        /// <param name="position">Push to position.</param>
        void PushTo(Vector2 position);
        /// <summary>
        /// Returns whether or not the object can be moved.
        /// </summary>
        /// <param name="position">The position the player wants to move the object to.</param>
        /// <returns></returns>
        bool CanBePushedTo(Vector2 position);

    }
}