using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// Custom animation class if you don't want to deal with the Animator. It's basically an Animator in the Inspector.
/// </summary>
[Serializable]
public class CustomAnimation {
    [Tooltip("Name of this animation")]
    public string name;

    [Tooltip("Triggers that start this animation.")]
    public CustomTrigger[] startTriggers;

    [Tooltip("Speed at which animation plays.")] [Range(1, 10)]
    public float speed = 1;

    [Tooltip("Whether animation loops.")]
    public bool loop = false;

    [Tooltip("Whether animation must finish before moving on to next animation.")]
    public bool mustFinish = false;

    [Tooltip("Sprites used in the animation.")]
    public Sprite[] frames;

    [Tooltip("Triggers that need to be set when animation finishes. If the animation loops, nothing happens to these triggers.")]
    public SerializableKeyValuePairs[] finishTriggers;

    [Tooltip("Events that get called when the animation starts. If frame/time-specific event is needed, you must unfortunatly for now use a timer.")] // TODO: Find way to call events on specific keyframes.
    public UnityEvent startEvents;

    [Tooltip("Events that get called when the animation finishes. These do not get called if the animation loops.")]
    public UnityEvent finishEvents;

    // Sprite to be replaced in the Sprite Renderer
    [HideInInspector] public Sprite currentSprite;
    
    private static readonly float MAX_FRAME_TIME = 0.1f;    // Time between frames
    private float frameTimer = MAX_FRAME_TIME;              // Counts time between frames
    private int index = 0;                                  // Index of current frame

    private void Awake() {
        currentSprite = frames[0];
    }

    /// <summary>
    /// Goes through the array of frames of an animation.
    /// </summary>
    /// <param name="delta">Time since last frame was called.</param>
    /// <returns>Returns the current sprite of the animation.</returns>
    public Sprite Animate(float delta) {
        // Calls start event on frame 0
        if (index == 0) startEvents.Invoke();

        if (frames.Length > 1) {
            frameTimer -= delta;

            if (frameTimer <= 0) {
                if (index < frames.Length - 1) {
                    index++;
                    currentSprite = frames[index];
                    if (index == frames.Length - 1) {
                        if (loop) index = 0;
                        else {
                            // Try something with finishTriggers here, if finishTriggers is even necessary in the first place
                            finishEvents.Invoke();
                        }
                    }
                }
                frameTimer = MAX_FRAME_TIME;
            }
        } else {
            currentSprite = frames[0]; // Maybe could use List<> for frames and check hasNext() instead of a conditional for a one-frame animation?
        }

        return currentSprite;
    }

    /// <summary>
    /// Resets the frame index when switching animations.
    /// </summary>
    public void ResetIndex() { index = 0; }
}
