Feature: Add Safe Location to Group
	A group admin should be able to manage Safe Locations in their group

Background: 
	Given Sasha is an authenticated user
	And he creates a group named "Sasha's group"
    And Grisha is an authenticated user

Scenario: Add Safe Location to the group
	When Sasha adds a Safe Location to "Sasha's group"
    Then the returned Safe Location object has the same latitute, longitude, name and a non-empty id

Scenario: Non-admin group member can't add Safe Location to the group
	When Grisha adds a Safe Location to "Sasha's group"
    Then Grisha gets a BadRequest response

Scenario: Delete Safe Location from the group
	When Sasha deletes a Safe Location from "Sasha's group"
    Then the list of Safe Locations in "Sasha's gorup" does not include the deleted Location

Scenario: Non-admin group member can't delete Safe Location from the group
	When Grisha deletes a Safe Location from "Sasha's group"
    Then Grisha gets a BadRequest response

Scenario: Group member can see group Safe Locations
    Given Grisha is a member of "Sasha's group"
    When Grisha checks the Safe Locations in "Sasha's group"
    Then Grisha gets a list of all the Safe Locations set by the admin

Scenario: Non-group member can't see group Safe Locations
    Given Ion is an authenticated user
    And Ion is not a member of "Sasha's group" 
    When Ion checks the Safe Locations in "Sasha's group"
    Then Ion gets a BadRequest response

Scenario: Admin can update group Safe Location
    When Sasha changes the location of a Safe Location in "Sasha's group"
    Then the returned Safe Location object has the same latitute, longitude, name and a non-empty id

Scenario: Non-admin group member can't update group Safe Location
    When Grisha changes the location of a Safe Location from "Sasha's group"
    Then Grisha gets a BadRequest response
