using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using TMPro;

namespace HallManager
{
    [System.Serializable]
    public class PlayerJonContainer
    {
        public TMP_Text name = null;
        public Image targetImage = null;
        public Sprite beforeJoined = null;
        public Sprite afterJoined = null;
    }

    public class HallManager : MonoBehaviour
    {
        [SerializeField] List<PlayerJonContainer> containers = new List<PlayerJonContainer>();

        public void GameStart()
        {
            Debug.Log("GameStart");
        }

        public void ControllerJoin(int id)
        {
            Player player = ReInput.players.GetPlayer(id);
            if (player.controllers.hasKeyboard)
            {
                containers[id].name.text = ReInput.players.GetPlayer(id).controllers.Keyboard.ToString();
            }
            else
            {
                containers[id].name.text = ReInput.players.GetPlayer(id).controllers.Joysticks.ToList()[0].ToString();
            }

            // players[i].controllers.joystickCount
            containers[id].targetImage.sprite = containers[id].afterJoined;
        }

        public void ControllerExit(int id)
        {
            Player player = ReInput.players.GetPlayer(id);
            player.controllers.ClearAllControllers();
            containers[id].targetImage.sprite = containers[id].beforeJoined;
            containers[id].name.text = "P" + id;
        }

        private void Update()
        {
            if (!ReInput.isReady) return;
            AssignJoysticksToPlayers();

            for (int i = 0; i < ReInput.players.playerCount; i++)
            {
                if (ReInput.players.GetPlayer(i).GetButtonDown("Disconnect"))
                {
                    ControllerExit(i);
                }
                else if (ReInput.players.GetPlayer(i).GetButtonDown("GameStart"))
                {
                    GameStart();
                }
            }
        }

        private void AssignJoysticksToPlayers()
        {

            JoystickPressed();
            KeyboardPressed();

            // If all players have joysticks, enable joystick auto-assignment
            // so controllers are re-assigned correctly when a joystick is disconnected
            // and re-connected and disable this script
            if (DoAllPlayersHaveControllers())
            {
                ReInput.configuration.autoAssignJoysticks = true;
                this.enabled = false; // disable this script
            }
        }

        private void JoystickPressed()
        {
            // Check all joysticks for a button press and assign it tp
            // the first Player foudn without a joystick
            IList<Joystick> joysticks = ReInput.controllers.Joysticks;
            for (int i = 0; i < joysticks.Count; i++)
            {

                Joystick joystick = joysticks[i];
                if (ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue; // joystick is already assigned to a Player

                // Chec if a button was pressed on the joystick
                if (joystick.GetAnyButtonDown())
                {

                    // Find the next Player without a Joystick
                    Player player = FindPlayerWithoutController();
                    if (player == null) return; // no free joysticks

                    // Assign the joystick to this Player
                    player.controllers.AddController(joystick, true);
                    ControllerJoin(player.id);
                }
            }
        }

        private void KeyboardPressed()
        {
            Keyboard keyboard = ReInput.controllers.Keyboard;

            if (ReInput.players.Players.Where(p => p.controllers.hasKeyboard).ToList().Count > 0) return;
            if (keyboard.GetAnyButtonDown())
            {
                // Find the next Player without a Joystick
                Player player = FindPlayerWithoutController();
                if (player == null) return; // no free joysticks

                // Assign the joystick to this Player
                player.controllers.AddController(keyboard, true);
                ControllerJoin(player.id);
            }
        }

        // Searches all Players to find the next Player without a Joystick assigned
        private Player FindPlayerWithoutController()
        {
            IList<Player> players = ReInput.players.Players;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].controllers.joystickCount > 0 || players[i].controllers.hasKeyboard)
                    continue;
                return players[i];
            }
            return null;
        }

        private bool DoAllPlayersHaveControllers()
        {
            return FindPlayerWithoutController() == null;
        }
    }

}