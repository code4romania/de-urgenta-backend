Feature: Backpack update
	A user should be able to update his backpacks

Background: 
	Given Sasha is authenticated user
	And has a backpack "My backpack"

@backpack @backpack_update
Scenario:  Backpack update should be reflected in `my backpacks` list
	When edit backpack name to "My new backpack"
	And he queries for his backpacks
	Then returned backpacks contain updated backpack

@backpack @backpack_update
Scenario: Backpack update should be reflected in `backpacks` list
	When edit backpack name to "My new backpack"
	And he queries for backpacks
	Then returned backpacks contain updated backpack

@backpack @backpack_update
Scenario: Backpack update can be performed only by owner
	Given Grisha is authenticated user
	When Grisha edits backpack created by Sasha
	Then gets BadRequest in response

@backpack @backpack_update
Scenario: Backpack update cannot be performed by a contributor
	Given Grisha is authenticated user
	And is a contributor to Sasha's backpack
	When Grisha edits backpack created by Sasha
	Then gets BadRequest in response

@backpack @backpack_update
Scenario: Backpack update cannot be performed by an un-authenticated
	Given Ion is un-authenticated user
	When Ion edits backpack created by Sasha
	Then gets Unauthorized in response