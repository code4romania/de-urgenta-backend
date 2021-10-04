namespace DeUrgenta.Group.Api.Options
{
    public class GroupsConfig
    {
        public const string SectionName = "Groups";
        
        public int MaxJoinedGroupsPerUser { get; set; }
        public int MaxCreatedGroupsPerUser { get; set; }
    }
}