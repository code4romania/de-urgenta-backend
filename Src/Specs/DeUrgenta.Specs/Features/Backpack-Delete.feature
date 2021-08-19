Feature: Backpack delete
	A user should be able to delete backpacks

Background: 
	Given Sasha is authenticated user
	And Sasha creates a backpack named "My backpack"

@backpack @backpack_delete
Scenario:  Backpack delete should be reflected in `my backpacks` list of owner
	When owner deletes backpack named "My backpack"
	And owner queries for his backpacks
	Then returned backpacks does not contain deleted backpack

@backpack @backpack_delete
Scenario: Backpack update should be reflected in `backpacks` list of owner
	When owner deletes backpack named "My backpack"
	And owner queries for backpacks
	Then returned backpacks does not contain deleted backpack

@backpack @backpack_delete
Scenario: Backpack delete can be performed only by owner
	Given Grisha is authenticated user
	When Grisha deletes backpack created by Sasha
	Then gets BadRequest in response

@backpack @backpack_delete
Scenario: Backpack delete cannot be performed by a contributor
	Given Grisha is authenticated user
	And is a contributor to Sasha's backpack
	When Grisha deletes backpack created by Sasha
	Then gets BadRequest in response


@backpack @backpack_delete
Scenario: Backpack delete cannot be performed by an un-authenticated
	Given Ion is un-authenticated user
	When Ion deletes backpack created by Sasha
	Then gets Unauthorized in response
