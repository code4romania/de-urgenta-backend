Feature: Update Safe Location to Group
A group admin should be able to update Safe Locations in their group

Background:
	Given Sasha is an authenticated user
	And Sasha creates a group named "Sashas group"
	And Grisha is an authenticated user
	And Grisha is a member of `Sasha's group`

@group @group_safe_locations
Scenario: Add Safe Location to the group
	When Sasha adds a Safe Location to `Sasha's group`
	Then the returned Safe Location object has the same latitude, longitude, name and a non-empty id

@group @group_safe_locations
Scenario: Created safe location is visible to all group members
	When Sasha adds a Safe Location to `Sasha's group`
	And Grisha queries for group safe locations
	Then list contains newly created safe location

@group @group_safe_locations
Scenario: Created safe location is visible to group admin
	When Sasha adds a Safe Location to `Sasha's group`
	And Sasha queries for group safe locations
	Then list contains newly created safe location

@group @group_safe_locations
Scenario: Non-admin group member can't add Safe Location to the group
	When Grisha adds a Safe Location to `Sasha's group`
	Then Grisha gets a BadRequest response

@group @group_safe_locations
Scenario: Unauthorized users cannot add group safe location
	Given Ion is un-authenticated user
	When Ion adds a Safe Location to `Sasha's group`
	Then gets Unauthorized in response

@group @group_safe_locations
Scenario: Non-group member can't add group Safe Locations
	Given Jora is an authenticated user
	When Jora adds a Safe Location to `Sasha's group`
	Then Jora gets a BadRequest response
