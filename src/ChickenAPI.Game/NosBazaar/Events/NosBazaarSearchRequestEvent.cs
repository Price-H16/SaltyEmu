﻿using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.NosBazaar.Events
{
    public class NosBazaarSearchRequestEvent : GameEntityEvent
    {
        public string SearchString { get; set; }
        // add filters
    }
}