Feature: Group edit
	A group admin should be able to edit a group they created

Background: 
	Given Sasha is an authenticated user
	And he creates a group named "Sasha's group"
    And Grisha is an authenticated user
	
Scenario: Change group name
	When Sasha changes the name of the group to "Awesome group"
    Then the returned group contains the group name "Awesome group" 

Scenario: Non-admin group member can't change group name
    Given Grisha is a member of "Sasha's group" but not the admin
	When Grisha changes the name of the group to "Awesome group"
    Then Grisha gets a BadRequest response

Scenario: Add member
    Given Grisha is not a member of "Sasha's group"
    When Sasha invites Grisha to be part of "Sasha's group"
    And Grisha accepts the inivitation
    Then the number of users in "Sasha's group" increases by 1
    And the list of group members in "Sasha's group" includes Grisha

Scenario: Non-admin group member can't add member
    Given Grisha is a member of "Sasha's group" but not the admin
	When Grisha invites a new member to be part of "Sasha's group"
    Then Grisha gets a BadRequest response

Scenario: Delete member
    When Sasha deletes Grisha from "Sasha's group"
    Then the number of users in "Sasha's group" decreases by 1
    And the list of group members in "Sasha's group" does not include Grisha

Scenario: Non-admin group member can't delete member
    Given Grisha is a member of "Sasha's group" but not the admin
	When Grisha deletes a member of "Sasha's group"
    Then Grisha gets a BadRequest response
