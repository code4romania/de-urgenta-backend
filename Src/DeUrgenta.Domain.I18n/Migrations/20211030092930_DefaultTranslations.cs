using Microsoft.EntityFrameworkCore.Migrations;

namespace DeUrgenta.Domain.I18n.Migrations
{
    public partial class DefaultTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "event-type-not-exist", "Event type does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "event-type-not-exist", "[Ro] Event type does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "event-type-not-exist", "[Hu] Event type does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "event-not-exist", "Event does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "event-not-exist", "[Ro] Event does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "event-not-exist", "[Hu] Event does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "blogpost-not-exist", "Blog post does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "blogpost-not-exist", "[Ro] Blog post does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "blogpost-not-exist", "[Hu] Blog post does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "event-type-not-exist-message", "Requested event type with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "event-type-not-exist-message", "[Ro] Requested event type with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "event-type-not-exist-message", "[Hu] Requested event type with id {0} does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "event-not-exist-message", "Requested event with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "event-not-exist-message", "[Ro] Requested event with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "event-not-exist-message", "[Hu] Requested event with id {0} does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "blogpost-not-exist-message", "Requested blogpost with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "blogpost-not-exist-message", "[Ro] Requested blogpost with id {0} does not exist");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "blogpost-not-exist-message", "[Hu] Requested blogpost with id {0} does not exist");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner", "You are not a backpack owner");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner", "[Ro] You are not a backpack owner");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner", "[Hu] You are not a backpack owner");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner-delete-message", "Only backpack owners can delete backpacks.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner-delete-message", "[Ro] Only backpack owners can delete backpacks.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner-delete-message", "[Hu] Only backpack owners can delete backpacks.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner-delete-contributor-message", "Only backpack owners can remove backpack contributors.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner-delete-contributor-message", "[Ro] Only backpack owners can remove backpack contributors.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner-delete-contributor-message", "[Hu] Only backpack owners can remove backpack contributors.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "backpack-owner-leave", "You cannot leave a backpack while you are an owner.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "backpack-owner-leave", "[Ro] You cannot leave a backpack while you are an owner.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "backpack-owner-leave", "[Hu] You cannot leave a backpack while you are an owner.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "backpack-owner-leave-message", "You cannot remove yourself from contributors if you are owner.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "backpack-owner-leave-message", "[Ro] You cannot remove yourself from contributors if you are owner.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "backpack-owner-leave-message", "[Hu] You cannot remove yourself from contributors if you are owner.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "certification-not-exist", "Certification does not exists.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "certification-not-exist", "[Ro] Certification does not exists.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "certification-not-exist", "[Hu] Certification does not exists.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "certification-not-exist-message", "Requested certification could not be found.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "certification-not-exist-message", "[Ro] Requested certification could not be found.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "certification-not-exist-message", "[Hu] Requested certification could not be found.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-create-groups", "Cannot create more groups");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-create-groups", "[Ro] Cannot create more groups");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-create-groups", "[Hu] Cannot create more groups");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-create-groups-max-message", "You have reached maximum number of groups per user.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-create-groups-max-message", "[Ro] You have reached maximum number of groups per user.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-create-groups-max-message", "[Hu] You have reached maximum number of groups per user.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-backpack-owner-can-update-message", "Only backpack owners can update backpacks.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-backpack-owner-can-update-message", "[Ro] Only backpack owners can update backpacks.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-backpack-owner-can-update-message", "[Hu] Only backpack owners can update backpacks.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-add-safe-location", "Cannot add safe locations");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-add-safe-location", "[Ro] Cannot add safe locations");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-add-safe-location", "[Hu] Cannot add safe locations");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-add-locations-message", "Only group admins can add safe locations.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-add-locations-message", "[Ro] Only group admins can add safe locations.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-add-locations-message", "[Hu] Only group admins can add safe locations.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "group-safe-location-limit", "Cannot add more safe locations");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "group-safe-location-limit", "[Ro] Cannot add more safe locations");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "group-safe-location-limit", "[Hu] Cannot add more safe locations");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "group-safe-location-limit-message", "You have reached maximum number of safe locations per group.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "group-safe-location-limit-message", "[Ro] You have reached maximum number of safe locations per group.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "group-safe-location-limit-message", "[Hu] You have reached maximum number of safe locations per group.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-delete-group", "Cannot delete group");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-delete-group", "[Ro] Cannot delete group");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-delete-group", "[Hu] Cannot delete group");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-delete-group-message", "Only group admin can delete group.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-delete-group-message", "[Ro] Only group admin can delete group.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-delete-group-message", "[Hu] Only group admin can delete group.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-delete-safe-location", "Cannot delete safe location");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-delete-safe-location", "[Ro] Cannot delete safe location");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-delete-safe-location", "[Hu] Cannot delete safe location");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-delete-safe-location-message", "Only group admins can delete safe locations.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-delete-safe-location-message", "[Ro] Only group admins can delete safe locations.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-delete-safe-location-message", "[Hu] Only group admins can delete safe locations.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-leave-group", "Cannot leave group");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-leave-group", "[Ro] Cannot leave group");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-leave-group", "[Hu] Cannot leave group");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-leave-administered-group-message", "You cannot leave the group you are currently administering.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-leave-administered-group-message", "[Ro] You cannot leave the group you are currently administering.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-leave-administered-group-message", "[Hu] You cannot leave the group you are currently administering.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-remove-user", "Cannot remove user");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-remove-user", "[Ro] Cannot remove user");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-remove-user", "[Hu] Cannot remove user");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-remove-yourself-message", "You cannot remove yourself from group.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-remove-yourself-message", "[Ro] You cannot remove yourself from group.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-remove-yourself-message", "[Hu] You cannot remove yourself from group.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-remove-users-message", "Only group admins can remove users from group.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-remove-users-message", "[Ro] Only group admins can remove users from group.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-remove-users-message", "[Hu] Only group admins can remove users from group.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-update-group", "Cannot update group");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-update-group", "[Ro] Cannot update group");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-update-group", "[Hu] Cannot update group");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-admin-cab-update-group", "Only group admin can update group.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-admin-cab-update-group", "[Ro] Only group admin can update group.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-admin-cab-update-group", "[Hu] Only group admin can update group.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-update-safe-location", "Cannot update safe location");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-update-safe-location", "[Ro] Cannot update safe location");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-update-safe-location", "[Hu] Cannot update safe location");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "only-admin-cab-update-safe-location", "Only group admin can update safe location.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "only-admin-cab-update-safe-location", "[Ro] Only group admin can update safe location.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "only-admin-cab-update-safe-location", "[Hu] Only group admin can update safe location.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-accept-invite", "Cannot accept invite");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-accept-invite", "[Ro] Cannot accept invite");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-accept-invite", "[Hu] Cannot accept invite");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "already-backpack-contributor", "User is already a backpack contributor.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "already-backpack-contributor", "[Ro] User is already a backpack contributor.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "already-backpack-contributor", "[Hu] User is already a backpack contributor.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "max-backpack-contributors-reached", "Current maximum number of contributors is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "max-backpack-contributors-reached", "[Ro] Current maximum number of contributors is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "max-backpack-contributors-reached", "[Hu] Current maximum number of contributors is reached.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "max-group-members-reached", "Current maximum number of group members is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "max-group-members-reached", "[Ro] Current maximum number of group members is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "max-group-members-reached", "[Hu] Current maximum number of group members is reached.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "max-group-per-user-reached", "Current maximum number of groups user is part of is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "max-group-per-user-reached", "[Ro] Current maximum number of groups user is part of is reached.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "max-group-per-user-reached", "[Hu] Current maximum number of groups user is part of is reached.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "invite-already-accepted", "Invite was already accepted.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "invite-already-accepted", "[Ro] Invite was already accepted.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "invite-already-accepted", "[Hu] Invite was already accepted.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "cannot-create-invite", "Cannot create invite");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "cannot-create-invite", "[Ro] Cannot create invite");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "cannot-create-invite", "[Hu] Cannot create invite");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "already-a-group-member-message", "User is already a group memeber.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "already-a-group-member-message", "[Ro] User is already a group memeber.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "already-a-group-member-message", "[Hu] User is already a group memeber.");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "language-culture-not-exist", "Language not found");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "language-culture-not-exist", "[Ro] Language not found");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "language-culture-not-exist", "[Hu] Language not found");

            migrationBuilder.AddTranslation(I18nDefaults.EnUsCultureId, "language-culture-not-exist-message", "Requested {0} culture was not found.");
            migrationBuilder.AddTranslation(I18nDefaults.RoRoCultureId, "language-culture-not-exist-message", "[Ro] Requested {0} culture was not found.");
            migrationBuilder.AddTranslation(I18nDefaults.HuHuCultureId, "language-culture-not-exist-message", "[Hu] Requested {0} culture was not found.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "event-type-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "event-type-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "event-type-not-exist");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "event-type-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "event-type-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "event-type-not-exist-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "event-type-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "event-type-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "event-type-not-exist-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner-delete-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner-delete-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner-delete-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner-delete-contributor-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner-delete-contributor-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner-delete-contributor-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "backpack-owner-leave");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "backpack-owner-leave");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "backpack-owner-leave");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "backpack-owner-leave-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "backpack-owner-leave-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "backpack-owner-leave-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "certification-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "certification-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "certification-not-exist");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "certification-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "certification-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "certification-not-exist-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-create-groups");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-create-groups");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-create-groups");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "not-backpack-owner");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "not-backpack-owner");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "not-backpack-owner");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-create-groups-max-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-create-groups-max-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-create-groups-max-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-backpack-owner-can-update-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-backpack-owner-can-update-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-backpack-owner-can-update-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-add-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-add-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-add-safe-location");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-add-locations-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-add-locations-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-add-locations-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "group-safe-location-limit");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "group-safe-location-limit");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "group-safe-location-limit");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "group-safe-location-limit-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "group-safe-location-limit-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "group-safe-location-limit-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-delete-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-delete-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-delete-group");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-delete-group-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-delete-group-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-delete-group-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-delete-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-delete-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-delete-safe-location");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-delete-safe-location-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-delete-safe-location-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-delete-safe-location-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-leave-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-leave-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-leave-group");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-leave-administered-group-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-leave-administered-group-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-leave-administered-group-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-remove-user");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-remove-user");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-remove-user");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-remove-yourself-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-remove-yourself-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-remove-yourself-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-group-admin-can-remove-users-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-group-admin-can-remove-users-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-group-admin-can-remove-users-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-update-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-update-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-update-group");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-admin-cab-update-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-admin-cab-update-group");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-admin-cab-update-group");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-update-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-update-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-update-safe-location");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "only-admin-cab-update-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "only-admin-cab-update-safe-location");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "only-admin-cab-update-safe-location");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-accept-invite");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-accept-invite");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-accept-invite");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "already-backpack-contributor");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "already-backpack-contributor");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "already-backpack-contributor");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "max-backpack-contributors-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "max-backpack-contributors-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "max-backpack-contributors-reached");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "max-group-members-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "max-group-members-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "max-group-members-reached");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "max-group-per-user-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "max-group-per-user-reached");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "max-group-per-user-reached");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "invite-already-accepted");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "invite-already-accepted");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "invite-already-accepted");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "cannot-create-invite");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "cannot-create-invite");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "cannot-create-invite");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "already-a-group-member-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "already-a-group-member-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "already-a-group-member-message");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "language-culture-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "language-culture-not-exist");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "language-culture-not-exist");

            migrationBuilder.RemoveTranslation(I18nDefaults.EnUsCultureId, "language-culture-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.RoRoCultureId, "language-culture-not-exist-message");
            migrationBuilder.RemoveTranslation(I18nDefaults.HuHuCultureId, "[Hu] Requested {0} culture was not found.");
        }
    }
}
