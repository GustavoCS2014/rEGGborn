using UnityEngine;

namespace Music {
        
    [CreateAssetMenu(fileName = "MusicRefSO", menuName = "MusicRefSO", order = 0)]
    public class MusicRefSO : ScriptableObject {
        [Tooltip("The music reproduced in the main menu.")]
        [SerializeField] private AudioClip mainMenuMusic;
        public AudioClip MainMenuMusic => mainMenuMusic;

        [Tooltip("The music reproduced while playing.")]
        [SerializeField] private AudioClip gameplayMusic;
        public AudioClip GameplayMusic => gameplayMusic;
        
        [Tooltip("The music reproduced at the end of the game")]
        [SerializeField] private AudioClip toBeContinuedMusic;
        public AudioClip ToBeContinuedMusic => toBeContinuedMusic;
    }
}