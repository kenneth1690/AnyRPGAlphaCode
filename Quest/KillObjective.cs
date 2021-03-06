using AnyRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AnyRPG {

    [System.Serializable]
    public class KillObjective : QuestObjective {

        public void UpdateKillCount(BaseCharacter character, float creditPercent) {
            //Debug.Log("KillObjective.UpdateKillCount()");

            bool completeBefore = IsComplete;
            if (completeBefore) {
                return;
            }

            // INVESTIGATE IF STRING MATCH CAN BE REPLACED WITH TYPE.GETTYPE DIRECT MATCH
            if (character.GetType() == Type.GetType(MyType) || SystemResourceManager.MatchResource(character.MyCharacterName, MyType) || SystemResourceManager.MatchResource(character.MyFaction.MyName, MyType)) {
                MyCurrentAmount++;
                quest.CheckCompletion();
                if (MyCurrentAmount <= MyAmount && !quest.MyIsAchievement && MyCurrentAmount != 0) {
                    MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", MyType, Mathf.Clamp(MyCurrentAmount, 0, MyAmount), MyAmount));
                }
                if (completeBefore == false && IsComplete && !quest.MyIsAchievement) {
                    MessageFeedManager.MyInstance.WriteMessage(string.Format("Learn {0} {1}: Objective Complete", MyCurrentAmount, MyType));
                }

            }
        }

        public override void OnAcceptQuest(Quest quest, bool printMessages = true) {
            //Debug.Log("KillObjective.OnAcceptQuest(): MyCurrentAmount: " + MyCurrentAmount);
            base.OnAcceptQuest(quest, printMessages);

            // don't forget to remove these later
            PlayerManager.MyInstance.MyCharacter.MyCharacterCombat.OnKillEvent += UpdateKillCount;
        }

        public override void OnAbandonQuest() {
            base.OnAbandonQuest();
            if (PlayerManager.MyInstance != null && PlayerManager.MyInstance.MyCharacter != null && PlayerManager.MyInstance.MyCharacter.MyCharacterCombat != null) {
                PlayerManager.MyInstance.MyCharacter.MyCharacterCombat.OnKillEvent -= UpdateKillCount;
            }
        }

    }
}