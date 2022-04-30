Feature: Backpack item delete
A user should be able to delete items in a backpack.
Backpack contributors can delete any items in a backpack.

Background: 
	Given Sasha is authenticated user
	And Sasha creates a backpack
	Given Grisha is authenticated user
	And Grisha is a backpack contributor

@backpack @backpack_items @backpack_item_delete
Scenario: Delete a backpack item
	Given Sasha creates an item
	And Sasha deletes created backpack item
	When Sasha queries for backpack items
	Then backpack item list does not contains updated item

@backpack @backpack_items @backpack_item_delete
Scenario: Deletion of backpack item is visible to all backpack contributors
	Given Sasha creates an item
	And Sasha deletes created backpack item
	When Grisha queries for backpack items
	Then backpack item list does not contains updated item

@backpack @backpack_items @backpack_item_delete
Scenario: Backpack contributors can delete backpack items created by them
	Given Grisha creates an item
	And Grisha deletes created backpack item
	When Sasha queries for backpack items
	Then backpack item list does not contains updated item

@backpack @backpack_items @backpack_item_delete
Scenario: Backpack contributors can delete backpack items created by others
	Given Sasha creates an item
	And Grisha deletes created backpack item
	When Sasha queries for backpack items
	Then backpack item list does not contains updated item

@backpack @backpack_items @backpack_item_delete
Scenario: Users that are not contributors cannot delete backpack items
	Given Jora is a registered user
	And Sasha creates an item
	When Jora deletes created item 
	Then gets BadRequest in response

@backpack @backpack_items @backpack_item_delete
Scenario: Unauthorized users cannot update backpack items
	Given Ion is un-authenticated user
	And Sasha creates an item
	When Ion deletes created item
	Then gets Unauthorized in response