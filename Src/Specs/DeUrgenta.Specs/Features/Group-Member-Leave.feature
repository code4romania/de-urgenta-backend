Feature: Member can leave Group
Group members can leave groups

Background:
	Given Sasha is an authenticated user
	And Sasha creates a group named "Sashas group"
	And Grisha is an authenticated user
	And Grisha is a member of `Sasha's group`

Scenario: Member leaves group is visible to admin
	When Grisha leaves "Sashas group"
	Then Sasha queries members list
	And the returned members list does not Grisha

Scenario: Delete safe location is visible to all group members
	Given Jora is an authenticated user
	And Jora is a member of `Sasha's group`
	When Grisha leaves "Sashas group"
	And Jora queries for group safe locations
	Then list not contains deleted safe location

Scenario: Member leaves group is reflected in user groups
	When Grisha leaves "Sashas group"
	Then Grisha queries for groups
	And the returned members list does not contain `Sashas group`

Scenario: Unauthorized users cannot leave groups
	Given Ion is un-authenticated user
	When Ion leaves from `Sasha's group`
	Then gets Unauthorized in response

Scenario: Non-group member can't leave groups
	Given Jora is an authenticated user
	When Jora leaves from `Sasha's group`
	Then Jora gets a BadRequest response
