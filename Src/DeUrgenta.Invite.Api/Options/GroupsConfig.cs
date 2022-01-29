namespace DeUrgenta.Invite.Api.Options
{
    public class GroupsConfig
    {
        public const string SectionName = "Groups";

        public int MaxUsers { get; set; }
        public int MaxJoinedGroupsPerUser { get; set; }
    }
}