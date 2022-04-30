Feature: Remove Member from Group
A group admin should be able to remove members in their group

Background:
	Given Sasha is an authenticated user
	And Sasha creates a group named "Sashas group"
	And Grisha is an authenticated user
	And Grisha is a member of `Sasha's group`

Scenario: Remove member from the group
	When Sasha Removes Grisha from `Sasha's group`
	Then the returned group members list does not contain Grisha

Scenario: Remove member can be done only by group admin
	Given Jora is an authenticated user
	And Jora is a member of `Sasha's group`
	When Grisha Removes Jora from `Sasha's group`
	Then gets a BadRequest response

Scenario: Remove member is visible to group members
	Given Jora is an authenticated user
	And Jora is a member of `Sasha's group`
	When Sasha Removes Grisha from `Sasha's group`
	And Jora queries for group members
	Then list not contains Grisha

Scenario: Admin can't remove himself
	When Sasha Removes himself from `Sasha's group`
	Then gets a BadRequest response

Scenario: Non-group member can't add group Safe Locations
	Given Jora is an authenticated user
	When Jora removes Grisha from `Sasha's group`
	Then Jora gets a BadRequest response

Scenario: Unauthorized users cannot Remove group safe location
	Given Ion is un-authenticated user
	When Ion Removes Grisha from `Sasha's group`
	Then gets Unauthorized in response
