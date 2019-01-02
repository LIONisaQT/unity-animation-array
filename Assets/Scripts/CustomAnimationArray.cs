using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer), typeof(PlayerInput))]
public class CustomAnimationArray : MonoBehaviour {
    [Serializable]
    public class AnimationArray { public CustomAnimation[] animations; }

    public AnimationArray customAnimations;

    private CustomAnimation currentAnimation, previousAnimation;

    private SpriteRenderer sr;
    private PlayerInput playerInput;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update() {
        if (customAnimations.animations.Length > 0) {

            // Go through list of animations
            foreach (CustomAnimation ca in customAnimations.animations) {
                // No trigger == default/idle animation
                if (ca.startTriggers.Length == 0) {
                    currentAnimation = ca;
                }

                // Go through each start trigger in an animation
                /*
                 * Currently stacked-based hierarchy - the last animation with all its criteria met
                 * will be the current animation. For example, currently the order of animations
                 * placed in the "stack" is idle > run > jump. If you are holding a horizontal axis
                 * input and are airborne, both animations will become current animation, but
                 * because jump is last in the stack (end of the array, more specifically), it will
                 * always overwrite the run animation.
                */
                foreach (CustomTrigger ct in ca.startTriggers) {
                    if (playerInput.states.ContainsKey(ct) && playerInput.states[ct]) {
                        currentAnimation = ca;
                    }
                }
            }
        }
    }

    private void FixedUpdate() {
        // The game has a null currentAnimation on the very first frame of the game because update() hasn't run yet
        if (currentAnimation != null) {
            // Sets initial previousAnimation
            if (previousAnimation == null) previousAnimation = currentAnimation;

            // Resets index when switching animations
            if (currentAnimation != previousAnimation) {
                currentAnimation.ResetIndex();
                previousAnimation = currentAnimation;
            }

            sr.sprite = currentAnimation.Animate(Time.deltaTime);
        }
    }
}
