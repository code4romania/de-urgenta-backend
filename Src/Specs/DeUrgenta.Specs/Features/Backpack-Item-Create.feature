Feature: Backpack item creation
A user should be able to create items in a backpack

Background: 
	Given Sasha is authenticated user
	And Grisha is authenticated user
	And Sasha creates a backpack
	And Grisha is a backpack contributor

@backpack @backpack_items @backpack_item_create
Scenario: Create a backpack item
	When Sasha creates an item
	Then returned item has same properties 
	And it does not have an empty id

@backpack @backpack_items @backpack_item_create
Scenario: Created backpack item is visible to all backpack contributors
	When Sasha creates an item
	And Grisha queries for backpack items
	Then list contains newly created item

@backpack @backpack_items @backpack_item_create
Scenario: Backpack contributors can create backpack items
	When Grisha creates an item
	And Sasha queries for backpack items
	Then list contains newly created item

@backpack @backpack_items @backpack_item_create
Scenario: Users should be able to create items with indefinite expiration date
	When Sasha creates an item with indefinite expiration date
	Then returned item has same properties 
	And it does not have an empty id

@backpack @backpack_items @backpack_item_create
Scenario: Users that are not contributors cannot create backpack items
	Given Jora is a registered user
	When Jora creates an item
	Then gets BadRequest in response

@backpack @backpack_items @backpack_item_create
Scenario: Unauthorized users cannot create backpack items
	Given Ion is un-authenticated user
	When Ion creates an item
	Then gets Unauthorized in response