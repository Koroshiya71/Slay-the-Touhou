using UnityEngine;

public class BugTest
{
    // This is a conceptual test to demonstrate the bug fix in Player.cs.
    // This code is not meant to be executed as part of the game.
    // It is for documentation and verification purposes only.

    public class PlayerTest
    {
        public int hp;
        public int shield;

        public PlayerTest(int hp, int shield)
        {
            this.hp = hp;
            this.shield = shield;
        }

        public void TakeDamage_OriginalBug(int damage)
        {
            if (damage <= shield)
            {
                shield -= damage;
                damage = 0;
            }
            else
            {
                shield = 0;
                damage -= shield; // Bug was here: shield is 0, so damage is not reduced.
            }
            hp -= damage;
        }

        public void TakeDamage_Fixed(int damage)
        {
            if (damage <= shield)
            {
                shield -= damage;
                damage = 0;
            }
            else
            {
                damage -= shield; // Corrected logic: subtract shield from damage first.
                shield = 0;       // Then set shield to 0.
            }
            hp -= damage;
        }
    }

    public static void RunTest()
    {
        // Scenario: Player with 100 HP and 10 shield takes 20 damage.

        // Test with original buggy code
        PlayerTest player_buggy = new PlayerTest(100, 10);
        player_buggy.TakeDamage_OriginalBug(20);
        // Expected: hp = 90, shield = 0
        // Actual with bug: hp = 80, shield = 0
        Debug.Log("With Bug: HP = " + player_buggy.hp + ", Shield = " + player_buggy.shield);


        // Test with fixed code
        PlayerTest player_fixed = new PlayerTest(100, 10);
        player_fixed.TakeDamage_Fixed(20);
        // Expected: hp = 90, shield = 0
        // Actual with fix: hp = 90, shield = 0
        Debug.Log("With Fix: HP = " + player_fixed.hp + ", Shield = " + player_fixed.shield);
    }
}
