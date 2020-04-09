// ----------------------------------------------------------------------------------------------------------------------
// <summary>The Photon Chat Api enables clients to connect to a chat server and communicate with other clients.</summary>
// <remarks>ChannelCreationOptions is a parameter used when subscribing to a public channel for the first time.</remarks>
// <copyright company="Exit Games GmbH">Photon Chat Api - Copyright (C) 2018 Exit Games GmbH</copyright>
// ----------------------------------------------------------------------------------------------------------------------

namespace Photon.Chat
{
    public class ChannelCreationOptions
    {
        /// <summary>Default values of channel creation options.</summary>
#pragma warning disable CS0436 // Type conflicts with imported type
        public static ChannelCreationOptions Default = new ChannelCreationOptions();
#pragma warning restore CS0436 // Type conflicts with imported type
        /// <summary>Whether or not the channel to be created will allow client to keep a list of users.</summary>
        public bool PublishSubscribers { get; set; }
        /// <summary>Limit of the number of users subscribed to the channel to be created.</summary>
        public int MaxSubscribers { get; set; }
    }
}
