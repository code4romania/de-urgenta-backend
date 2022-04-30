Feature: Group delete
A user should be able to delete groups

Background: 
	Given Sasha is authenticated user
	And Sasha creates a group named "Friends"

@group @group_delete
Scenario: group delete should be reflected in `Friends` list of owner
	When owner deletes group named `Friends`
	And owner queries for his groups
	Then returned groups does not contain deleted group

@group @group_delete
Scenario: group update should be reflected in `groups` list of owner
	When owner deletes group named `Friends`
	And owner queries for groups
	Then returned groups does not contain deleted group

@group @group_delete
Scenario: group delete can be performed only by owner
	Given Grisha is authenticated user
	And is a member of Sasha's group
	When Grisha deletes group created by Sasha
	Then gets BadRequest in response

@group @group_delete
Scenario: group delete cannot be performed by a member
	Given Grisha is authenticated user
	And is a member of Sasha's group
	When Grisha deletes group created by Sasha
	Then gets BadRequest in response

@group @group_delete
Scenario: admin cant delete group with members
	Given is a member of Sasha's group
	When Grisha deletes group created by Sasha
	Then gets BadRequest in response

@group @group_delete
Scenario: group delete cannot be performed by an un-authenticated
	Given Ion is un-authenticated user
	When Ion deletes group created by Sasha
	Then gets Unauthorized in response
