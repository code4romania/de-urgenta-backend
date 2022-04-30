Feature: Delete Safe Location from Group
A group admin should be able to delete Safe Locations in their group

Background:
	Given Sasha is an authenticated user
	And Sasha creates a group named "Sashas group"
	And Sasha adds a safe location "Parking lot" to the group
	And Sasha adds a safe location "Park" to the group
	And Grisha is an authenticated user
	And Grisha is a member of `Sasha's group`

@group @group_safe_locations
Scenario: Delete Safe Location from the group
	When Sasha deletes Safe Location `Park` from `Sasha's group`
	Then the returned Safe Locations list does not contain deleted location

@group @group_safe_locations
Scenario: Delete safe location is visible to all group members
	When Sasha deletes Safe Location `Park` from `Sasha's group`
	And Grisha queries for group safe locations
	Then list not contains deleted safe location

@group @group_safe_locations
Scenario: Delete safe location is visible to group admin
	When Sasha deletes Safe Location `Park` from `Sasha's group`
	And Sasha queries for group safe locations
	Then list contains newly created safe location

@group @group_safe_locations
Scenario: Non-admin group member can't delete Safe Location from group
	When Grisha deletes Safe Location `Park` from `Sasha's group`
	Then Grisha gets a BadRequest response

@group @group_safe_locations
Scenario: Unauthorized users cannot delete group safe location
	Given Ion is un-authenticated user
	When Ion deletes Safe Location `Park` from `Sasha's group`
	Then gets Unauthorized in response

@group @group_safe_locations
Scenario: Non-group member can't add group Safe Locations
	Given Jora is an authenticated user
	When Jora deletes Safe Location `Park` from `Sasha's group`
	Then Jora gets a BadRequest response
