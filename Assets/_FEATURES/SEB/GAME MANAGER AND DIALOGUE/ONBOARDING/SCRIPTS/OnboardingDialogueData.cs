using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    /// <summary>
    /// Stores all onboarding dialogue text in a centralized dictionary.
    /// </summary>
    public static class OnboardingDialogueData
    {
        /// <summary>
        /// Dictionary storing all onboarding dialogue lines.
        /// </summary>
        private static readonly Dictionary<string, string> textLines = new Dictionary<string, string>
        {
            // 1. Introduction
            {"onboarding_1a", "Welcome to the CSAR training machine onboarding stage. I'll be your guide through the experience."},
            {"onboarding_1b", "This is your main text panel. It will display important information throughout the simulation."},
            {"onboarding_1c", "Point your controller on the button and press trigger to continue."},

            // 2. Movement Tutorial
            {"onboarding_2a", "Move around using the left stick, and rotate using the right one. Push right joystick to turn around 180 degrees."},
            {"onboarding_2b", "Move to the highlighted area to continue."},

            // 3. Interaction Tutorial
            {"onboarding_3a", "Some objects in the environment can be grabbed using the grip button."},
            {"onboarding_3b", "Try picking up the watering can from the shelf."},
            {"onboarding_3c", "Nice job, levers work in similar way, try grabbing the lever and switch its position."},
            {"onboarding_3d", "Well done! Now, try grabbing the valve and rotate it."},
            {"onboarding_3e", "Great! Now you should be ready for the actual CSAR practice."},

            // 4. Understanding the Experience
            {"onboarding_4a", "This experience consists of few stages. You will learn OSH, then you will be introduced to how the machines operate, and after that there will be a little quiz testing your knowledge."},
            {"onboarding_4b", "If you feel ready to start, press Yes. If you would like to repeat the onboarding, press No."},
            {"onboarding_4b_yes", "Moving to the next stage, the OSH."},
            {"onboarding_4b_no", "Restarting onboarding tutorial..."}
        };

        /// <summary>
        /// Retrieves a dialogue line based on the provided key.
        /// </summary>
        public static string GetText(string lineKey)
        {
            return textLines.TryGetValue(lineKey, out var text) ? text : "[Missing text for this line.]";
        }
    }
}
