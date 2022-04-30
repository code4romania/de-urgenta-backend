Feature: Group creation
A user should be able to create groups

Background: 
	Given Sasha is authenticated user
	And he creates a group called "My group"

@group @group_create
Scenario: Create a group
	Then returned group has same name
	And it does not have an empty id

@group @group_create
Scenario: Query `my groups` should contain created group
	When he queries for his groups
	Then returned groups contain created group

@group @group_create
Scenario: Query `groups` should contain created group
	When he queries for groups
	Then returned groups contain created group

@group @group_create
Scenario: An non authenticated user cannot create groups
	Given Ion is a non authenticated user
	When Ion tries to create a group
	Then 401 is returned