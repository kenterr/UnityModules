﻿using System;
using Leap;

namespace InteractionEngine {

  public class HandAlreadyUntrackedException : Exception {
    public HandAlreadyUntrackedException(string methodName, int handId) :
      base("Cannot call " + methodName + " because there is already an untracked hand of id " + handId +
           " grasping this InteractionObject.") { }
  }

}