﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.Relations;
using ChickenAPI.Enums.Game.Relations;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.FriendsPlugin.Protocol;

namespace SaltyEmu.FriendsPlugin.Services
{
    public class RelationServiceClient : MqttIpcClient<RelationServiceClient>, IRelationService
    {
        public RelationServiceClient(MqttClientConfigurationBuilder builder) : base(builder)
        {
        }

        public async Task<IEnumerable<RelationDto>> GetRelationListByCharacterIdAsync(long characterId)
        {
            return (await RequestAsync<GetRelationsByCharacterId.Response>(new GetRelationsByCharacterId { CharacterId = characterId })).Relations;
        }

        public async Task<IEnumerable<RelationInvitationDto>> GetPendingInvitationByCharacterIdAsync(long characterId)
        {
            return (await RequestAsync<GetRelationsInvitationByCharacterId.Response>(new GetRelationsInvitationByCharacterId { CharacterId = characterId })).Invitations;
        }

        public Task<IEnumerable<RelationMessageDto>> GetPendingMessagesByCharacterIdAsync(long characterId)
        {
            Log.Warn("GetPendingMessagesByCharacterIdAsync not implemented");
            return null;
        }

        public async Task<RelationInvitationDto> RelationInviteAsync(long senderId, long targetId, CharacterRelationType relationType)
        {
            var tmp = new RelationInvitationDto
            {
                Id = Guid.NewGuid(),
                OwnerId = senderId,
                TargetId = targetId,
                RelationType = relationType
            };
            await RelationInviteAsync(tmp);
            return tmp;
        }

        public Task RelationInviteAsync(RelationInvitationDto invitation)
        {
            return BroadcastAsync(new SendInvitation { Invitation = invitation });
        }

        public async Task<List<RelationDto>> RelationInviteProcessAsync(Guid invitationId, RelationInvitationProcessType type)
        {
            return (await RequestAsync<ProcessInvitation.Response>(new ProcessInvitation { InvitationId = invitationId, ProcessType = type })).Relation;
        }

        public Task RelationAddAsync(IEnumerable<RelationDto> relations)
        {
            return BroadcastAsync(new RelationPacketRelationsAdd { Relations = relations });
        }

        public Task RelationAddAsync(RelationDto add)
        {
            return RelationAddAsync(new[] { add });
        }

        public Task RelationAddAsync(long senderId, long targetId, CharacterRelationType relationType)
        {
            return RelationAddAsync(new RelationDto
            {
                Id = Guid.NewGuid(),
                OwnerId = senderId,
                TargetId = targetId,
                Type = relationType
            });
        }

        public Task RelationRemoveAsync(IEnumerable<RelationDto> relations)
        {
            return BroadcastAsync(new RemoveRelations { Relations = relations });
        }

        public Task RelationRemoveAsync(RelationDto remove)
        {
            return RelationRemoveAsync(new[] { remove });
        }

        public Task RelationSendMessageAsync(RelationMessageDto message) => throw new NotImplementedException();

        public Task RelationSendMessageAsync(long senderId, long targetId, string message) => throw new NotImplementedException();
    }
}