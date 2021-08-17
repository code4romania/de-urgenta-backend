Feature: Backpack creation
	A user should be able to create backpacks

Background: 
	Given Sasha is authenticated user
	And he creates a backpack called "My backpack"

@backpack @backpack_create
Scenario: Create a backpack
	Then returned backpack has same name
	And it does not have an empty id

@backpack @backpack_create
Scenario: Query `my backpacks` should contain created backpack
	When he queries for his backpacks
	Then returned backpacks contain created backpack

@backpack @backpack_create
Scenario: Query `backpacks` should contain created backpack
	When he queries for backpacks
	Then returned backpacks contain created backpack