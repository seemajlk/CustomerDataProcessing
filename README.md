# CustomerDataProcessing
Process and sort customer data



AppSheetProject Folder contains the Visual studio solution
with 2 projects

AppSheetProject -> is a console application and running this will give the expected result.

Expected Result is the youngest 5 users with valid US Phone numbers sorted by name.
I have added comments throughout the code to describe what my assumptions are about input, phone number validation etc


AppSheetTestProject -> is an MsTest project that has few sample tests. Note that this ia not a comprehensive list of tests, but is 
just intended to show how the application can be tested 
Basically the idea to enable TDD here is to define interfaces for all major components, so that the real component can be 
replaced with Mocks while testing.
Dependency reoslution can be done using frameworks like Mef or any custom resolver. In this project, I have not used Mef.



