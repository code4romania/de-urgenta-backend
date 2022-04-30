Feature: Group update
A user should be able to update his groups

Background: 
	Given Sasha is authenticated user
	And Sasha creates a group named "My group"

@group @group_update
Scenario: Group update should be reflected in `my groups` list of owner
	When owner edits group name to "My new group"
	And owner queries for his groups
	Then returned groups contain updated group

@group @group_update
Scenario: Group update should be reflected in `groups` list of owner
	When owner edits group name to "My new group"
	And owner queries for groups
	Then returned groups contain updated group

@group @group_update
Scenario: Group update can be performed only by owner
	Given Grisha is authenticated user
	When Grisha edits group created by Sasha
	Then gets BadRequest in response

@group @group_update
Scenario: Group update cannot be performed by a contributor
	Given Grisha is authenticated user
	And is a contributor to Sasha's group
	When Grisha edits group created by Sasha
	Then gets BadRequest in response

@group @group_update
Scenario: Group update should be reflected in `groups` list of contributors
	Given Grisha is authenticated user
	And is a contributor to Sasha's group
	When owner edits group name to "My new group"
	And Grisha queries for groups
	Then returned groups contain updated group

@group @group_update
Scenario: Group update cannot be performed by an un-authenticated
	Given Ion is un-authenticated user
	When Ion edits group created by Sasha
	Then gets Unauthorized in response