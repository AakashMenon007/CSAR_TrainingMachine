using System.Collections.Generic;
using UnityEngine;

namespace Amused.XR
{
    public static class OnboardingDialogueData
    {
        private static readonly Dictionary<string, string> textLines = new Dictionary<string, string>
        {
            // --- Onboarding ---
            { "onboarding_1a", "Welcome to the CSAR training machine onboarding stage. I'll be your guide through the experience." },
            { "onboarding_1b", "This is your main text panel. It will display important information throughout the simulation." },
            { "onboarding_2a", "Move around using the left stick, and rotate using the right one. Push right joystick backwards to turn around 180 degrees." },
            { "onboarding_2b", "Move to the highlighted area to continue." },
            { "onboarding_2c", "Well done. Now please press the big green button on the table to proceed." },
            { "onboarding_3a", "Some objects in the environment can be grabbed using the grip button." },
            { "onboarding_3b", "Try picking up the watering can from the shelf." },
            { "onboarding_3c", "Nice job, levers work in similar way, try grabbing the lever and switch its position." },
            { "onboarding_3d", "Well done! Now, try grabbing the valve and rotate it." },
            { "onboarding_3e", "Great! Now you should be ready for the actual CSAR practice." },
            { "onboarding_4a", "This experience consists of few stages. You will learn OSH, then you will be introduced to how the machines operate, and after that there will be a little quiz testing your knowledge." },
            { "onboarding_4b", "If you feel ready to start, press Yes. If you would like to repeat the onboarding, press No." },
            { "onboarding_4b_no", "Restarting onboarding tutorial..." },
            { "onboarding_4b_yes", "Moving to the next stage, the OSH." },

            // --- OSH ---
            { "osh_1a", "Before we begin, it's important to wear the appropriate safety gear. Let's start with your lab coat." },
            { "osh_1b_coat", "Please grab the lab coat and equip it now. This will protect you from chemical exposure and contamination." },
            { "osh_1b_waiting", "Remember, you won’t be allowed into the main area without full protective gear." },
            { "osh_1b_warning", "Hey, you are not fully gear up yet. Please don’t interact with machines while unprepared." },
            { "osh_1b_gloves", "Next, put on the protective gloves. These are essential when interacting with surfaces and equipment in the facility. In this environment, your safety is our top priority. Always gear up before you interact with any machine." },
            { "osh_1c", "Great. Now that you're properly equipped, we can move to the next stage. Let's explore how the system works." }
        };

        public static string GetText(string lineKey)
        {
            return textLines.TryGetValue(lineKey, out var text) ? text : "[Missing text for this line.]";
        }
    }
}
