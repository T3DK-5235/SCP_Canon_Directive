using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GenericAchievement
{
    [SerializeField] private int achievementID;
	[SerializeField] private string achievementName;
    [SerializeField] private string achievementDescription;
    [SerializeField] private string achievementHint;
    [SerializeField] private string achievementIcon;
    [SerializeField] private bool completion;

    public GenericAchievement(int achievementID, string achievementName, string achievementDescription, 
                              string achievementHint, string achievementIcon, bool completion) {
        this.achievementID = achievementID;
        this.achievementName = achievementName;
        this.achievementDescription = achievementDescription;
        this.achievementHint = achievementHint;
        this.achievementIcon = achievementIcon;
        this.completion = completion;
    }

    // ==============================================================================================================
    // |                                  Code to get the affects of the proposal                                   |
    // ==============================================================================================================

    public int getAchievementID() {
		return this.achievementID;
	}

    public string getAchievementName() {
		return this.achievementName;
	}

    public string getAchievementDescription() {
		return this.achievementDescription;
	}

    public string getAchievementHint() {
		return this.achievementHint;
	}

    public string getAchievementIcon() {
		return this.achievementIcon;
	}

    public bool getAchievementCompletion() {
        return this.completion;
    }

    public void setAchievementCompletion(Boolean achievementStatus) {
        this.completion = achievementStatus;
    }

}
