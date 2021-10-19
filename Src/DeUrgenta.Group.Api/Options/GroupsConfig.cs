namespace DeUrgenta.Group.Api.Options
{
    public class GroupsConfig
    {
        public const string SectionName = "Groups";

        public int MaxCreatedGroupsPerUser { get; set; }
        public int UsersLimit { get; set; }
        public int MaxSafeLocations { get; set; }
    }
}