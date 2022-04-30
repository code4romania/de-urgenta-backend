Feature: Backpack item update
A user should be able to update items in a backpack.
Backpack contributors can update any items in a backpack.

Background: 
	Given Sasha is authenticated user
	And Sasha creates a backpack
	Given Grisha is authenticated user
	And Grisha is a backpack contributor

@backpack @backpack_items @backpack_item_update
Scenario: Update a backpack item
	Given Sasha creates an item
	When Sasha updates created backpack item
	Then returned item has same properties 

@backpack @backpack_items @backpack_item_update
Scenario: Updated backpack item is visible to all backpack contributors
	Given Sasha creates an item
	And Sasha updates created backpack item
	When Grisha queries for backpack items
	Then backpack item list contains updated item

@backpack @backpack_items @backpack_item_update
Scenario: Backpack contributors can update backpack items created by them
	Given Grisha creates an item
	And Grisha updates created backpack item
	When Sasha queries for backpack items
	Then backpack item list contains updated item

@backpack @backpack_items @backpack_item_update
Scenario: Backpack contributors can update backpack items created by others
	Given Sasha creates an item
	And Grisha updates created backpack item
	When Sasha queries for backpack items
	Then backpack item list contains updated item

@backpack @backpack_items @backpack_item_update
Scenario: Users that are not contributors cannot update backpack items
	Given Jora is a registered user
	And Sasha creates an item
	When Jora updates created item 
	Then gets BadRequest in response

@backpack @backpack_items @backpack_item_update
Scenario: Unauthorized users cannot update backpack items
	Given Ion is un-authenticated user
	And Sasha creates an item
	When Ion updates created item
	Then gets Unauthorized in response